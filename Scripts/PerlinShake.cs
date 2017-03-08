using UnityEngine;
using System.Collections;

public class PerlinShake : MonoBehaviour {
	
	public float duration = 0.5f;
	public float speed = 1.0f;
	public float magnitude = 0.1f;
    public Transform obj;
    public bool test = false;

    private float randNrX;
    private float randNrY;
    private float randNrZ;

    // -------------------------------------------------------------------------
    public void PlayShake() {
		
		StopAllCoroutines();
		StartCoroutine("Shake");
	}
	
	// -------------------------------------------------------------------------
	void Update() {
		if (test) {
			test = false;
			PlayShake();
		}
	}
	
	// -------------------------------------------------------------------------
	IEnumerator Shake() {
		
		float elapsed = 0.0f;
		
		Vector3 originalCamPos = Camera.main.transform.position;

        

        while (elapsed < duration) {

            elapsed += Time.deltaTime;

            float randNrX = Random.Range(0.7f, -0.7f);
            float randNrY = Random.Range(0.7f, -0.7f);
            float randNrZ = Random.Range(0.7f, -0.7f);
            obj.transform.position += new Vector3(randNrX, randNrY, randNrZ);
            yield return null;


		}
		
		Camera.main.transform.position = originalCamPos;
	}
}
