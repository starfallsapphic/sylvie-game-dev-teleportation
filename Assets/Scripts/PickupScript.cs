using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour
{
    public float degreesPerSecond;
    public int teleportsToAdd = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, degreesPerSecond, 0) * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player"){
            var playerMovement = other.gameObject.GetComponentInParent<PlayerMovement>();
            playerMovement.IncreaseMaxTeleports(teleportsToAdd); 
            Destroy(gameObject);
        }
    }
}
