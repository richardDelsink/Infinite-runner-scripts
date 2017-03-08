using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    public Canvas can;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Play()
    {
        SceneManager.LoadScene("Main");
    }

    public void Back()
    {
        can.gameObject.SetActive(false);
    }

    public void Controls()
    {
        can.gameObject.SetActive(true);
    }
}
