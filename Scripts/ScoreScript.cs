using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
	private float score;

    public Text textPanel;
	
	void Start()
	{
        textPanel.enabled = true;
        score = 0f;
    }

    // Function for turning the score as integer
    public int ScoreReturn()
	{
		return (int) score;
	}
	
	public void ScoreCount( float delta )
	{
		score += delta;

       
        textPanel.text = "Score:     <color=yellow>" + (int) score + "</color>";
	}
	
	public void YuksekSkorKaydet()
	{
        if ( score > PlayerPrefs.GetInt( "HighScore" ) )
		{
			PlayerPrefs.SetInt( "HighScore", (int) score );
			PlayerPrefs.Save();
		}
	}
}
