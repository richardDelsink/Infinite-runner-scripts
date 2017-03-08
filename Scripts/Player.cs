using UnityEngine;
using UnityEngine.UI;
using System.Collections;
// Player (character) control script
// Buy the script assigned to the same object in order to work properly
// Requires a InputManager script (requirecomponent)
[RequireComponent( typeof( InputManager ) )]
public class Player : MonoBehaviour 
{

    // poolmanager
	public PoolManager poolGen;
	public ScoreScript score;
	public GameObject gameOverMenu;

    // GameOver Audio
    public AudioClip gameOver;
    public AudioClip collison;
    public AudioClip star;
    public AudioClip jump;
    // for animation playing 
    public Transform targetMesh;
    public GameObject anim;
    public GameObject ghosts;
    public ParticleSystem particle;
    public Collider col;
    public Text stars;
    public PerlinShake shake;
    public Text boost;

    // transforms
    private Transform tr;
	private Rigidbody r;
    private int starCount;
	private AudioSource ses;
	private InputManager inputManager;
    private bool magnet = false;
    private int hitCounter = 0;





    // Start running speed
    public float startSpeed = 20f;
    // movement speed for left and right
    public float speedSide = 5f;
    public GameObject mag;
   // speed increase
    public float increaseSpeed = 10f;
	private float maxIncrease;

    // Fall down
    //private bool fall = false;
    
    // character dies
    private bool death = false;

   
    [HideInInspector]
	public float limitMinDeger = -2.75f;
	[HideInInspector]
	public float limitMaxDeger = 2.75f;

    // -1 Left
    // 0 no direction
    // 1 right
    private int directionIs = 0;
    // Variables which we entered Intersection
    private bool isCorner = false;

 
    [HideInInspector]
	public Vector3 forwards = Vector3.forward;
	[HideInInspector]
	public Vector3 goRight = Vector3.right;
	
	void Start()
	{
        starCount = 0;
		r = GetComponent<Rigidbody>();
		tr = transform;
		ses = GetComponent<AudioSource>();
		inputManager = GetComponent<InputManager>();

        mag.gameObject.SetActive(false);
        inputManager.enabled = true;

        // time to increase the speed 
        maxIncrease = Time.time + increaseSpeed;
	}
	
	void FixedUpdate()
	{

        // Character is still alive:
        if (  !death )
		{
            // Character moves forward
            tr.Translate( Vector3.forward * startSpeed * Time.deltaTime );
            tr.Translate( Vector3.right * inputManager.Sensor() * speedSide * Time.deltaTime );
		}
	}
	
	void Update()
	{
     
        Vector3 pos = tr.position;
        Debug.Log(startSpeed);
        GetComponentInChildren<Collider>().enabled = true;
        // death
        if ( !death && pos.y < -5f )
		{
			death = true;
			GameOver();
			Time.timeScale = 0f;
		}

        // If the character is still alive Things to do:
        if ( !death )
		{
            // Increases speed by checking score
            score.ScoreCount( Time.deltaTime * startSpeed);
            
        

        if ( Time.time >= maxIncrease)
			{
                maxIncrease = Time.time + increaseSpeed;
                startSpeed++;
            }

            // Move the characters minimum and maximum limit
            if (forwards == Vector3.forward || forwards == Vector3.back )
                pos.x = Mathf.Clamp(pos.x, limitMinDeger, limitMaxDeger );
			else
                pos.z = Mathf.Clamp(pos.z, limitMinDeger, limitMaxDeger );
			
			tr.position = pos;

          
            if (isCorner && directionIs != 0 )
			{
				if(directionIs == -1 )
				{
                    poolGen.isLeft = true;
				}
				else
				{
                    poolGen.isRight = true;
				}

                isCorner = false;
                directionIs = 0;
			}
		}
	}


   
    public void Down()
    {
        targetMesh.transform.localScale = new Vector3(0.4f,0.4f,0.4f);
        anim.GetComponent<Animation>().Play("dragon_panic");
        StartCoroutine("Wait");

    }

    private IEnumerator Magnet()
    {
        yield return new WaitForSeconds(10);
        mag.gameObject.SetActive(false);
        magnet = false;
    }

    private IEnumerator WaitAnim()
    {
        yield return new WaitForSeconds(1);
        anim.GetComponent<Animation>().Play("dragon_idle");
        boost.gameObject.SetActive(false);
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
        targetMesh.transform.localScale = new Vector3(1f, 1f, 1f);
        anim.GetComponent<Animation>().Play("dragon_idle");
    }

    private IEnumerator CollReset()
    {
        yield return new WaitForSeconds(10);
        hitCounter = 0;
        ghosts.gameObject.SetActive(false);

    }

    private IEnumerator Shake()
    {
        yield return new WaitForSeconds(0.5f);
        shake.test = false;

    }
    // Function for the character to bounce
    public void Jumping()
	{
     
        if (!death && Mathf.Abs(r.velocity.y) < 0.5f)
        {
            anim.GetComponent<Animation>().Play("dragon_idle");
            r.AddForce(new Vector3(0, 25, 0), ForceMode.Impulse);
            ses.PlayOneShot(jump);
            StartCoroutine("WaitAnim");
        }


    }

    // turn left
    public void LeftTurn()
	{

        // If the character is still alive
        if ( !death )
		{
            // Set the input turning left
            directionIs = -1;
            CancelInvoke();
			Invoke( "ResetTurn", 1f );
		}
	}

    // Character to function when it comes to telling the intersection turn right
    public void TurnRight()
	{

        // If the character is still alive
        if ( !death )
		{
            // Set the input to turn right
            directionIs = 1;

            CancelInvoke();
			Invoke( "ResetTurn", 1f );
		}
	}

    // Reset corner turn
    void ResetTurn()
	{
        directionIs = 0;
	}


    //  finished 
    void GameOver()
	{
		death = true;

        // Save highscore 
        score.YuksekSkorKaydet();

        // Game Over menu Enable
        gameOverMenu.SetActive(true);

        // Play the end game sound
        ses.PlayOneShot( gameOver);
	}

    // collider triggers
    void OnTriggerEnter(Collider c)
    {
        if (c.tag == "Turn")
        {
            isCorner = true;
        }
        else if (c.tag == "Coin" && !magnet)
        {
            score.ScoreCount(10);
            starCount = starCount + 1;
            stars.text = starCount.ToString();
            c.gameObject.SetActive(false);
            poolGen.CoinPointObject(c.transform);
            particle.Play();
            // Play the sound 
            ses.PlayOneShot(star);
          
        }
        else if (c.tag == "magnet" && !death)
        {
            starCount = starCount + 1;
            stars.text = starCount.ToString();
            mag.gameObject.SetActive(true);
            magnet = true;
            StartCoroutine("Magnet");

        }

        
    }


    void OnTriggerExit( Collider c )
	{
     
        if ( c.tag == "Turn" )
            isCorner = false;

        else if (c.tag == "Coin" && magnet)
        {
            score.ScoreCount(10);

            c.gameObject.SetActive(false);
            poolGen.CoinPointObject(c.transform);
            particle.Play();
            // Play the sound 
            ses.PlayOneShot(star);
        }

        else if (c.tag == "boost" && !death)
        {
            startSpeed += 3f;
            boost.gameObject.SetActive(true);
            StartCoroutine("WaitAnim");
        }


    }


    void OnCollisionEnter( Collision c )
	{
      
        if ( c.gameObject.tag == "obstacle" && !death )
		{
            if (hitCounter == 0)
            {
                hitCounter = 1;
                startSpeed = 10f;
                ghosts.gameObject.SetActive(true);
                ses.PlayOneShot(collison);
                StartCoroutine("CollReset");
                shake.test = true;
                StartCoroutine("Shake");

            }
            else if (hitCounter == 1)
            {
                hitCounter = 0;
                r.isKinematic = true;
                ses.PlayOneShot(collison);
                GameOver();

            }
        }
       else if (c.gameObject.tag == "death" && !death)
        {
            ses.PlayOneShot(collison);
            r.isKinematic = true;
            GameOver();


        }


    }

   

}
