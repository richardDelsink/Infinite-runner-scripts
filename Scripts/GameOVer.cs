using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOVer : MonoBehaviour {


    public ScoreScript scoreScript;
    public Text text;
    // Use this for initialization
    void Start () {


       text.text = "Score : " + scoreScript.ScoreReturn() + "\nHighScore : " + PlayerPrefs.GetInt("HighScore");
    }
	
	// Update is called once per frame
	void Update () {
	
        
	}

    public void Reload()
    {
        SceneManager.LoadScene("Main");
    }

    public void goToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
