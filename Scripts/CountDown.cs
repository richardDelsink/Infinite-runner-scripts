using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CountDown : MonoBehaviour {


    public CamControl cameraControle;
    public Player playerScript;
    public Animation playerAnimation;
    public ScoreScript scoreScript;
    public Text counter;
    // public Canvas ui;
    private float timer;
   

    // Use this for initialization
    void Start () {

        cameraControle.enabled = false;
        playerScript.enabled = false;
        playerAnimation.enabled = false;
        scoreScript.enabled = false;
        Time.timeScale = 1f;
        timer = 4;

    }
	
	// Update is called once per frame
	void Update () {

        timer -= Time.deltaTime;
        int minutes = Mathf.FloorToInt(timer/ 60F);
        int seconds = Mathf.FloorToInt(timer-minutes* 60);
        string niceTime = string.Format("{00}", seconds);
        counter.text = niceTime;
         

        if (timer <= 0)
        {
            counter.text = null;
            StartGame();
        }

    }

    void StartGame()
    {
        cameraControle.enabled = true;
        playerScript.enabled = true;
        playerAnimation.enabled = false;
        scoreScript.enabled = true;
        //ui.gameObject.SetActive(true);
    }


}
