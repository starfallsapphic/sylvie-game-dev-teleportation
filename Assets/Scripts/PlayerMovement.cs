using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;

    public float jumpForce = 10;
    public float jumpCooldown;
    public float airMultiplier;
    public float gravityScale = 1;
    bool readyToJump = true;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode teleportKey = KeyCode.Mouse1;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("Teleport")]
    public int maxAirTeleports;
    public float teleportDistance;
    public float speedBurst = 50;
    public float verticalBurstScale = 0.25f;
    public GameObject noTeleportsEffect;
    private int teleportsLeft;

    [Header("Other")]
    public Transform orientation;
    public Transform playerCam;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private bool teleportStorage = false;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        teleportsLeft = maxAirTeleports;
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if(Input.GetKeyDown(teleportKey) && teleportsLeft > 0){
            teleportStorage = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        
        LimitSpeed();

        if(grounded){
            rb.drag = groundDrag;
            teleportsLeft = maxAirTeleports;
        }else{
            rb.drag = 0;
        }

        GetInput();

        if(teleportsLeft > 0){
            noTeleportsEffect.SetActive(false);
        }else{
            noTeleportsEffect.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
        rb.AddForce((Vector3.up * 9.81f) + (Vector3.down * 9.81f * gravityScale), ForceMode.Force);
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(OnSlope() && !exitingSlope){
            // Debug.DrawRay(transform.position, GetSlopeMoveDirection(), Color.green, 5f);
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);
            if(rb.velocity.y > 0){
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }else if(grounded){
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }else if(!grounded){
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }

        rb.useGravity = !OnSlope();

        if (teleportStorage) {
            Teleport();
            teleportStorage = false;
        }
    }

    private void LimitSpeed()
    {
        // don't limit air speed
        if(grounded){
            return;
        }

        // limit speed on slope
        if(OnSlope() && !exitingSlope)
        {
            if(rb.velocity.magnitude > moveSpeed){
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }

        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        
        // general limitation on ground
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        exitingSlope = true;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        exitingSlope = false;
        readyToJump = true;
    }

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    private void Teleport(){
        Vector3 displacement = playerCam.forward * teleportDistance;
        transform.position = transform.position + displacement;

        // cancel y-velocity
        rb.velocity = new Vector3(rb.velocity.x*0.25f, 0f, rb.velocity.z*0.25f);
        Vector3 burstForce = playerCam.forward * speedBurst;
        rb.AddForce(new Vector3(burstForce.x, burstForce.y*verticalBurstScale, burstForce.z), ForceMode.Impulse);

        teleportsLeft--;
    }
}
