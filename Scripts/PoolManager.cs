using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent( typeof(ObjectPool) )]
public class PoolManager : MonoBehaviour
{
	// the path will have a minimum number of objects
	public int minLenght = 10;
    // the path will have a maximum number of objects
    public int maxLength = 20;
	// coin density
	public int coinDensity = 6;
	
	// prefab coin
	public GameObject coins;

	// array roadObjects
	public GameObject[] roadList;
	// Elke path heeft xx length 
	public float[] margin;
	
	// junction trigger 1
	public GameObject cornerTrigger1;
	
	public Vector3 cornerTrigger1Place;
	
	
	public GameObject left;
	
	public Vector3 leftPlace;
	
	
	public GameObject tJunction;

    public Vector3 tJunctionSpace;
    public GameObject playerObj;

	private PoolContainer junctionT;

	private PoolContainer rightJunction;
	
	private PoolContainer leftJunction;

	
	// pool object
	private ObjectPool pool;
	

	public bool isLeft = false;
	public bool isRight = false;
	
	void Start() 
	{
		if( roadList.Length != margin.Length )
		{				
			Destroy( this );
		}
		else
		{
            // Pool (pool) filled
            pool = GetComponent<ObjectPool>();
			
			int length = maxLength / 3;
            pool.fillPool( roadList, left, cornerTrigger1,
                                tJunction, coins, length,
                                coinDensity);
         


            // Create a random way
            junctionT = new PoolContainer();
			rightJunction = new PoolContainer();
			leftJunction = new PoolContainer();

            RandomPath(junctionT, Vector3.forward * 15, Vector3.forward );
            Connector(junctionT.DirEnd(), junctionT.Direction() );
		}
	}
	
	// random creation of path
	void RandomPath(PoolContainer c, Vector3 dirForward, Vector3 dirForward2 )
	{
		// random determine length of read
		int roadLenght = Random.Range(minLenght, maxLength + 1 );

        // Create the path

        c.DirectionObj(pool, margin, dirForward, dirForward2, roadLenght);
		// coin object
		c.CoinObjPoint(pool, coinDensity);
	}

    // Interchanges and junctions connected straight paths 
    void Connector( Vector3 endPath, Vector3 directionStraight )
	{

        // Find the value of the rotation 
        Vector3 angle = PoolContainer.GroundDirection(directionStraight);


        // [0-2] and returned a random integer in the range:
        // 0- left junction is formed
        // 1. Right-junction is formed
        // 2 t junction is created
        switch ( Random.Range( 0, 3 ) )
		{
			case 0:
                // Create only left turn
                
                Transform leftObject = pool.ReturnLeft();
                leftObject.position = endPath;
                leftObject.eulerAngles = angle;
                leftObject.gameObject.SetActive( true );

                // turn left
                if (directionStraight == Vector3.forward )
				{
                    directionStraight = Vector3.left;
                    endPath += new Vector3( -leftPlace.x / 2, 0, leftPlace.z / 2 );
				}
				else if(directionStraight == Vector3.left )
				{
                    directionStraight = Vector3.back;
                    endPath += new Vector3( -leftPlace.z / 2, 0, -leftPlace.x / 2 );
				}
				else if(directionStraight == Vector3.back )
				{
                    directionStraight = Vector3.right;
                    endPath += new Vector3(leftPlace.x / 2, 0, -leftPlace.z / 2 );
				}
				else
				{
                    directionStraight = Vector3.forward;
                    endPath += new Vector3(leftPlace.z / 2, 0, leftPlace.x / 2 );
				}


                // Create a new flat road at the junction
                RandomPath( leftJunction, endPath, directionStraight);
				break;
			case 1:

              //create right turn
                Transform rightObj = pool.ReturnRight();
                rightObj.position = endPath;
                rightObj.eulerAngles = angle;
                rightObj.gameObject.SetActive( true );


                // turn right
                if (directionStraight == Vector3.forward )
				{
                    directionStraight = Vector3.right;
                    endPath += new Vector3(cornerTrigger1Place.x / 2, 0, cornerTrigger1Place.z / 2 );
				}
				else if(directionStraight == Vector3.left )
				{
                    directionStraight = Vector3.forward;
                    endPath += new Vector3( -cornerTrigger1Place.z / 2, 0, cornerTrigger1Place.x / 2 );
				}
				else if(directionStraight == Vector3.back )
				{
                    directionStraight = Vector3.left;
                    endPath += new Vector3( -cornerTrigger1Place.x / 2, 0, -cornerTrigger1Place.z / 2 );
				}
				else
				{
                    directionStraight = Vector3.back;
                    endPath += new Vector3( cornerTrigger1Place.z / 2, 0, -cornerTrigger1Place.x / 2 );
				}


                // Create a new flat road at the junction
                RandomPath( rightJunction, endPath, directionStraight);
				break;
			case 2:

                // Create both left and right turn
                Transform tObj = pool.TWayJunction();
                tObj.position = endPath;
                tObj.eulerAngles = angle;
                tObj.gameObject.SetActive( true );

                //variable in both the left and right direction
                Vector3 endPath2 = endPath;
				if(directionStraight == Vector3.forward )
				{
                    directionStraight = Vector3.left;
                    endPath += new Vector3( -tJunctionSpace.x / 2, 0, tJunctionSpace.z / 2 );
                    endPath2 += new Vector3(tJunctionSpace.x / 2, 0, tJunctionSpace.z / 2 );
				}
				else if(directionStraight == Vector3.left )
				{
                    directionStraight = Vector3.back;
                    endPath += new Vector3( -tJunctionSpace.z / 2, 0, -tJunctionSpace.x / 2 );
                    endPath2 += new Vector3( -tJunctionSpace.z / 2, 0, tJunctionSpace.x / 2 );
				}
				else if(directionStraight == Vector3.back )
				{
                    directionStraight = Vector3.right;
                    endPath += new Vector3(tJunctionSpace.x / 2, 0, -tJunctionSpace.z / 2 );
                    endPath2 += new Vector3( -tJunctionSpace.x / 2, 0, -tJunctionSpace.z / 2 );
				}
				else
				{
                    directionStraight = Vector3.forward;
                    endPath += new Vector3(tJunctionSpace.z / 2, 0, tJunctionSpace.x / 2 );
                    endPath2 += new Vector3(tJunctionSpace.z / 2, 0, -tJunctionSpace.x / 2 );
				}

                // Create a new flat at both ends of the junction in the road
                RandomPath( leftJunction, endPath, directionStraight);
                RandomPath( rightJunction, endPath2, -directionStraight);
				break;
		}
	}
	
	void Update()
	{
		if(isLeft)
		{
            // If you are instructed to turn left 
            isLeft = false;

            // If you have a leftJunction 
            if ( leftJunction.Road() > 0 )
			{
                // rotate 90 degrees to the left
                Player player = playerObj.GetComponent<Player>();
				player.transform.Rotate( Vector3.down * 90f );

                // Set direction variables player
                Vector3 bn = leftJunction.DirEnd();
				Vector3 iy = leftJunction.Direction();
				player.forwards = iy;
				player.goRight = leftJunction.RightDir();

                // Player can go on the horizontal axis minimum and maximum limits
               
                if ( iy == Vector3.forward || iy == Vector3.back )
				{
					player.limitMinDeger = bn.x - 2.75f;
					player.limitMaxDeger = bn.x + 2.75f;
				}
				else
				{
					player.limitMinDeger = bn.z - 2.75f;
					player.limitMaxDeger = bn.z + 2.75f;
				}


                // Path is no longer 
                PoolContainer temp = junctionT;
                junctionT = leftJunction;
				leftJunction = temp;

                // Create the rest of the way
                StartCoroutine(newIntersection() );
			}
		}
		else if(isRight)
		{
            // If you are instructed to turn right 
            isRight = false;

           
            if ( rightJunction.Road() > 0 )
			{
                //player Rotate Right 90 degrees
                Player player = playerObj.GetComponent<Player>();
				player.transform.Rotate( Vector3.up * 90f );

                // Set the direction variables player
                Vector3 bn = rightJunction.DirEnd();
				Vector3 iy = rightJunction.Direction();
				player.forwards = iy;
				player.goRight = rightJunction.RightDir();

                // Player can go on the horizontal axis minimum and maximum limits
          
                if ( iy == Vector3.forward || iy == Vector3.back )
				{
					player.limitMinDeger = bn.x - 2.75f;
					player.limitMaxDeger = bn.x + 2.75f;
				}
				else
				{
					player.limitMinDeger = bn.z - 2.75f;
					player.limitMaxDeger = bn.z + 2.75f;
				}

                // Path right 
                PoolContainer temp = junctionT;
                junctionT = rightJunction;
				rightJunction = temp;

                // Create the rest of the way
                StartCoroutine(newIntersection() );
			}
		}
	}

    // // Put a new intersections from the new straight road intersection into the end of the current path
    IEnumerator newIntersection()
	{
        // Wait 2 seconds junction 
       
        yield return new WaitForSeconds( 2f );

        leftJunction.CoinPoint(pool);
		rightJunction.CoinPoint(pool);

        // deactivate (make it invisible)
        pool.ReturnLeft().gameObject.SetActive( false );
        pool.ReturnRight().gameObject.SetActive( false );
        pool.TWayJunction().gameObject.SetActive( false );

       
        // Path to create
        Connector(junctionT.DirEnd(), junctionT.Direction() );
	}

   
    // Points is normally automatically added to the pool when no objects old path

    public void CoinPointObject( Transform obje )
	{
        pool.pointObj(obje);


        // Points to the ways in which objects to object if it points the way to delete the same object
       
        junctionT.CollectedCoin( obje );
		rightJunction.CollectedCoin( obje );
		leftJunction.CollectedCoin( obje );
	}

   
}
