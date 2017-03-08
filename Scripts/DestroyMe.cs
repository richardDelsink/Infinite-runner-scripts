using UnityEngine;


public class DestroyMe : MonoBehaviour 
{
	public static bool destroy;
	private bool meDestroy = false;
	
	void Start()
	{
		// scene açıldığında destroy'u false yap
		destroy = false;
	}
	
	void Update()
	{
		//Desttroy object
		if( destroy && !meDestroy)
		{
			meDestroy = true;
			Destroy( transform.gameObject, 4f );
		}
	}
}
