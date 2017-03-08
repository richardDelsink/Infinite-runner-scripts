using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// Stores objects that can be pooled
public class ObjectPool: MonoBehaviour
{
   
    // object Instantiate whether the pool) script
    private GameObject[] ListObjects;
	
    private GameObject coinObject;

    
    // Array to the pool at that index (List <Transform>) are accessing it and we draw our objects
    private List<Transform>[] poolObjectList;

    // left transform
    private Transform left;

    // right transform
    private Transform right;


    // tJunction transform
    private Transform tJunction;
    
    // coin transform child ob
    private List<Transform> coinOb;
   


    // fillpool
    public void fillPool( GameObject[] roadList, GameObject leftJunction, 
							  GameObject rightJunction, GameObject tBoneJunction,
							  GameObject coinPrefab, int size, 
							  int coinObCount )
	{
		this.ListObjects = roadList;
        coinObject = coinPrefab;

        // The rest of the functions
        Vector3 pos = Vector3.zero;
		Quaternion rot = Quaternion.identity;
		GameObject ob;
		
		ob = Instantiate( leftJunction, pos, rot ) as GameObject;
		ob.SetActive( false );
        left = ob.transform;
		
		ob = Instantiate( rightJunction, pos, rot ) as GameObject;
		ob.SetActive( false );
        right = ob.transform;
		
		ob = Instantiate( tBoneJunction, pos, rot ) as GameObject;
		ob.SetActive( false );
        tJunction = ob.transform;

        poolObjectList = new List<Transform>[roadList.Length];
        coinOb = new List<Transform>();
		
		for( int i = 0; i < roadList.Length; i++ )
		{
            poolObjectList[i] = new List<Transform>();
			
			for( int j = 0; j < size; j++ )
			{
				ob = Instantiate(roadList[i], pos, rot ) as GameObject;
				ob.SetActive( false );
                poolObjectList[i].Add( ob.transform );
			}
		}
		
		int transformCoin = (int)( coinObCount * size * 2.5f );
		for( int i = 0; i < transformCoin; i++ )
		{
			ob = Instantiate( coinPrefab, pos, rot ) as GameObject;
			ob.SetActive( false );
            coinOb.Add( ob.transform );
		}
		
	}

    // Addingclonse
    public void poolObjClones( int index, Transform obj )
	{
        // Add element
        poolObjectList[index].Add( obj );
	}

    // adding a point object 
    public void pointObj( Transform obje )
	{
        // Add element
        coinOb.Add( obje );
	}


    // For a way to get the objects from the pool clone function
    // Here the index, which indicates the way we want to take a clone of prefab
    public Transform getPoolObj( int index )
	{
		Transform obje;
		
		if(poolObjectList[index].Count <= 0 )
		{
            // If it's empty,  create a new clone and returns it
            obje = ( Instantiate(ListObjects[index], Vector3.zero, Quaternion.identity ) as GameObject ).transform;
		}
		else
		{
            // If it is not empty
            obje = poolObjectList[index][0];
            poolObjectList[index].RemoveAt(0);
		}
		
		return obje;
	}

    // A point for getting to clone objects from the pool function
    public Transform coinPointClone()
	{
		Transform obje;
		
		if(coinOb.Count <= 0 )
        {
            // If the pool is empty, a new score points objects 
            // Instantiate 
            obje = ( Instantiate(coinObject, Vector3.zero, Quaternion.identity ) as GameObject ).transform;
		}
		else
		{
            // If the object scores pool is not empty 
            // Returns the first element and remove that element from the pool
            obje = coinOb[0];
            coinOb.RemoveAt(0);
		}
		
		return obje;
	}

    // Returns the left 
    public Transform ReturnLeft()
	{
		return left;
	}

    // Returns the right junction object
    public Transform ReturnRight()
	{
		return right;
	}


    // Returns the two-way intersection objects
    public Transform TWayJunction()
	{
		return tJunction;
	}
}
