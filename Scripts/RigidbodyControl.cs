using UnityEngine;


public class RigidbodyControl : MonoBehaviour 
{
	private Rigidbody r;
	
	void Start()
	{
      
        r = transform.parent.GetComponent<Rigidbody>();
	}
	
	void RigidbodyAc()
	{
		
		r.isKinematic = false;

       
        r.velocity = new Vector3( r.velocity.x, -10f, r.velocity.z );
	}
	
	void RigidbodyCon()
	{
       
        r.isKinematic = true;
	}
}
