using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// poolContainer
public class PoolContainer
{

    // This way of storing all objects contained road
    private Transform[] pObj;
    
    // Determines that the order in which prefab clone spawns
    private int[] poolIndexer;

    // store coin obj
    private List<Transform> coinList;

    // The end point of the road
    private Vector3 endDir;

    // Direction of the road (forward right direction, backward or left)
    private Vector3 direction;

    //determine which direction to go
    //Determines whether the road consists of many floors
    public void DirectionObj(ObjectPool objP, float[] objIndex,
							Vector3 end, Vector3 direc, int indexer )
	{
        pObj = new Transform[indexer];
        poolIndexer = new int[indexer];
        coinList = new List<Transform>();

        // Will have the ground object 
        Vector3 angle = GroundDirection(direc);
	
		for( int i = 0; i < indexer; i++ )
		{
            // Choose a random pool of prefab floor 
            int randomIndex = Random.Range( 0, objIndex.Length );
			Transform obje = objP.getPoolObj( randomIndex );

            // Adjusts the position of this ground
            obje.position = end;
			obje.eulerAngles = angle;
			obje.gameObject.SetActive( true );

            // Ground
            pObj[i] = obje;
            poolIndexer[i] = randomIndex;

            // The length of the floor 
            end += direc * objIndex[randomIndex];
		}

        //endpoint and direction of the road
        endDir = end;
        direction = direc;
	}

    // Point object from the pool 
    public void CoinObjPoint(ObjectPool poolOb, int points )
	{
        // We are working on a set of points 
        int coinObjDirection = 0;
        // We determine the gap between 
        int coinGap = 1;

        // 0 spawn left
        // 1 spawn right
        // Points can determine whether the objects lined the road on which side

        int dir = Random.Range( 0, 2 );
		
		for( int i = 0; i < pObj.Length; i++ )
		{
			int transPoint;
			Transform parentObje;

           
            if (dir == 0 )
			{
				parentObje = pObj[i].Find( "coinSpawnLeft" );
			}
			else
			{
				parentObje = pObj[i].Find( "coinSpawnRight" );
			}


         
            transPoint = parentObje.childCount;

           
            for ( int j = 0; j < transPoint &&
                 coinObjDirection < points; j++ )
			{
                // having a score object clone
                Transform obje = poolOb.coinPointClone();


                // Points to the spawn point is positioning objects and make it active
                obje.position = parentObje.GetChild( j ).position;
				obje.gameObject.SetActive( true );

                // We add the list of objects
                coinList.Add( obje );

                // We are increasing the number of objects
                coinObjDirection++;
			}

            // If the sequence is complete 
        
            if (coinObjDirection == points)
			{
               
                // spawn points in a direction 
                coinObjDirection = 0;
				i += coinGap;
                dir = Random.Range( 0, 2 );
			}
		}
	}

    //if player has collected coin
    public void CollectedCoin( Transform obje )
	{
		if(coinList != null )
            coinList.Remove( obje );
	}

    // coin 
    public void CoinPoint(ObjectPool poolObj )
	{
		if(pObj != null )
		{

           
            for ( int i = 0; i < pObj.Length; i++ )
			{
				Transform obje = pObj[i];
				obje.gameObject.SetActive( false );
                poolObj.poolObjClones(poolIndexer[i], obje );
			}


            // Points to deactivate
            while (coinList.Count > 0 )
			{
				Transform obje = coinList[0];
				obje.gameObject.SetActive( false );
                coinList.RemoveAt( 0 );
                poolObj.pointObj( obje );
			}

            pObj = null;
		}
	}

    // Function that returns the end point of the road
    public Vector3 DirEnd()
	{
		return endDir;
	}

    // Function that returns the direction of the road
    public Vector3 Direction()
	{
		return direction;
	}

  
    public Vector3 RightDir()
	{
		if(direction == Vector3.forward )
			return Vector3.right;
			
		if(direction == Vector3.right )
			return Vector3.back;
			
		if(direction == Vector3.back )
			return Vector3.left;
			
		return Vector3.forward;
	}

    // Function that returns consisting of several floors of the road
    public int Road()
	{
		if(pObj == null )
			return 0;
		
		return pObj.Length;
	}

    
    public static Vector3 GroundDirection(Vector3 direction)
    {
        if (direction == Vector3.forward)
            return Vector3.zero;
        else if (direction == Vector3.right)
            return Vector3.up * 90;
        else if (direction == Vector3.left)
            return Vector3.up * 270;
        else if (direction == Vector3.back)
            return Vector3.up * 180;
        else
        {
            // As a parameter only forward, backward, left or right directions
            return Vector3.zero;
        }
    }
}
