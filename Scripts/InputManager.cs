using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class InputManager : MonoBehaviour 
{
	
	private Player player;
    private Animator animator;

    public float mobilSensorSensitivity = 3.5f;
	public float mobilTouchSensitivity = 30f;
	
	#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
	
	private float mousePoint;


#else	
	private int id = -1;
	private Vector2 position;
	private float sensorOutput = 0f;


    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered
    private List<Vector3> touchPositions = new List<Vector3>(); //store all the touch positions in list

#endif

    void Start()
	{
		player = GetComponent<Player>();
		
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
		
		mousePoint = Input.mousePosition.x;
#else
		mobilTouchSensitivity *= mobilTouchSensitivity;
        dragDistance = Screen.height*20/100; //dragDistance is 20% height of the screen 
#endif
    }

    void Update() 
	{
        

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
    
        if ( Input.GetKeyDown( KeyCode.Space ) || Input.GetMouseButtonDown( 0 ) )
		{
			player.Jumping();
            
        }
		
		if( Input.GetKeyDown( KeyCode.A ) )
		{
			player.LeftTurn();
		}
		
		
		if( Input.GetKeyDown( KeyCode.D ) )
		{
			player.TurnRight();
		}

        if (Input.GetKeyDown(KeyCode.S))
        {
            player.Down();
        }


#else

        float sensor = ( Input.acceleration.x ) * mobilSensorSensitivity;
		sensorOutput = Mathf.Lerp( sensorOutput, sensor, 0.25f );

        foreach (Touch touch in Input.touches)  //use loop to detect more than one swipe
    { //can be ommitted if you are using lists 
    /*if (touch.phase == TouchPhase.Began) //check for the first touch
    {
        fp = touch.position;
        lp = touch.position;
 
    }*/
 
    if (touch.phase == TouchPhase.Moved) //add the touches to list as the swipe is being made
    {
    touchPositions.Add(touch.position);
    }
 
    if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
    {
        //lp = touch.position;  //last touch position. Ommitted if you use list
        fp =  touchPositions[0]; //get first touch position from the list of touches
        lp =  touchPositions[touchPositions.Count-1]; //last touch position 
 
        //Check if drag distance is greater than 20% of the screen height
        if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
        {//It's a drag
              //check if the drag is vertical or horizontal 
              if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
              {   //If the horizontal movement is greater than the vertical movement...
                  if ((lp.x>fp.x))  //If the movement was to the right)
                  {   //Right swipe
                      player.TurnRight();
                  }
                  else
                  {   //Left swipe
                      player.LeftTurn();
                  }
              }
            else
            {   //the vertical movement is greater than the horizontal movement
                 if (lp.y>fp.y)  //If the movement was up
                 {   //Up swipe
                     player.Jumping();
                 }
                 else
                 {   //Down swipe
                     player.Down();
                 }
            }
        } 
    }
    else
    {   //It's a tap as the drag distance is less than 20% of the screen height
 
    }
   }
	
#endif
    }


    public float Sensor()
	{
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
	
		return Mathf.Clamp( ( Input.mousePosition.x - mousePoint ) / 50f, -1f, 1f );
#else

		return Mathf.Clamp( sensorOutput, -1f, 1f );
#endif
	}
}
