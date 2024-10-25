using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class EventHandler : MonoBehaviour
{
    public GameObject player;
    public Transform spawnPosition;
    public Canvas canvas;
    public TMP_Text timerText;
    public KeyCode resetKey = KeyCode.R;
    public KeyCode exitKey = KeyCode.Escape;
    public Transform endHeight;
    public GameObject finishScreen;
    public TMP_Text finishTimer;
    public Material newSkybox;
    
    private PlayerMovement playerMovement;
    private CanvasTimer timerScript;

    private float _exitTimer;


    // Start is called before the first frame update
    void Start()
    {
        finishScreen.SetActive(false);

        playerMovement = player.GetComponent<PlayerMovement>();
        timerScript = canvas.GetComponent<CanvasTimer>();
        timerScript.StartTimer();

        player.transform.position = spawnPosition.position;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        OutOfBoundsHandler();
        FinishHandler();
    }

    void GetInput()
    {
        if(Input.GetKeyDown(resetKey)){
            ResetScene();
        }
        
        if(Input.GetKey(exitKey)){
            advanceExit();
        }else{
            _exitTimer = 0;
        }
    }

    private void OutOfBoundsHandler()
    {
        if(playerMovement.IsOutOfBounds()){
            player.transform.position = spawnPosition.position;
        }
    }

    private void FinishHandler()
    {
        if(player.transform.position.y > endHeight.position.y){
            timerScript.StopTimer();
            timerText.gameObject.SetActive(false);

            finishTimer.text = string.Format("your time: {0}", timerScript.GetTime());
            finishScreen.SetActive(true);
        }
    }

    private void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void advanceExit()
    {
        _exitTimer += Time.deltaTime;
        if(_exitTimer > 1){
            Application.Quit();
        }
    }

    public void changeSkyBox()
    {
        RenderSettings.skybox = newSkybox;
        DynamicGI.UpdateEnvironment();
    }
}
