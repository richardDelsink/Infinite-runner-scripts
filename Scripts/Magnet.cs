using UnityEngine;
using System.Collections;

public class Magnet : MonoBehaviour
{
    public float MagnetPower = 0f;
    public GameObject player;

    // Use this for initialization
    void Start()
    {
        this.transform.position = player.transform.position;
    }

    void OnTriggerStay(Collider other)
    {
        //if (other.attachedRigidbody)
        //    other.attachedRigidbody.AddForce(Vector3.up * 10);

        if (other.attachedRigidbody && other.transform.name.Contains("star(Clone)"))
        {
            //other.attachedRigidbody.velocity = new Vector3(0, 0, 0);
            other.transform.position = Vector3.Lerp(other.transform.position, this.transform.position, Time.deltaTime * MagnetPower);
        }
    }

    void FixedUpdate()
    {
        this.transform.position = player.transform.position;
    }
}
