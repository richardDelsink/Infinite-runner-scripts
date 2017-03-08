using UnityEngine;


public class CamControl : MonoBehaviour 
{
    // To follow objects
    public Transform obj;

    
    private Transform tr;

    
    public float distance2 = 6f;
 
    public float distance = 5f;
	
	void Start()
	{
		tr = transform;
	}
	
	void FixedUpdate()
	{
        // Need to go to the camera position
        Vector3 objPos= obj.position - obj.forward * distance2 + Vector3.up * distance;

        if ( objPos.y < distance)
			objPos.y = distance;


        transform.position = Vector3.Slerp( transform.position, objPos, 0.1f );

        
        Quaternion rot = Quaternion.LookRotation( obj.position - tr.position );
		tr.rotation = Quaternion.Slerp(tr.rotation, rot, 0.1f );
	}
}
