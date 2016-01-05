using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public Rigidbody2D myRigidbody;

    private Animator myAnimator;

	[SerializeField]
	private float movementSpeed;

    [SerializeField]
    private float jumpHeight;
    
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask whatIsGround;    
    private bool grounded;
    

    public Transform wallCheck;
    public float wallTouchRadius;
    public LayerMask whatIsWall;
    private bool touchingWall;


    private bool doubleJumped;

    private bool attack;

    private bool jump;
    
	private bool facingRight;

	public Transform firePoint;
	public GameObject ninjaStar;

	public float shotDelay;
	private float shotDelayCounter;
	private float timestamp;

	public float knockback;
	public float knockBackLength;
	public float knockbackCount;
	public bool knockFromRight;

	public AudioClip[] Clips;
	private AudioSource[] audioSources;



    // Use this for initialization
    void Start () {
		facingRight = true;
		myRigidbody = GetComponent<Rigidbody2D> ();
        myAnimator = GetComponent<Animator>();

		audioSources = new AudioSource[Clips.Length];
		int i = 0;
		while (i < Clips.Length) {
			GameObject child = new GameObject("Player");
			child.transform.parent = gameObject.transform;
			
			audioSources[i] = child.AddComponent<AudioSource>() as AudioSource;
			
			audioSources[i].clip = Clips[i];
			
			i++;
		}
	}

    void Update()
    {
        HandleInput();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        float horizontal = Input.GetAxis ("Horizontal");
		HandleMovement (horizontal);

        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
       
        touchingWall = Physics2D.OverlapCircle(wallCheck.position, wallTouchRadius, whatIsWall);
        

        Flip(horizontal);

        HandleAttack();

        ResetValues();
    }

	private void HandleMovement(float horizontal) {        
		if (knockbackCount <= 0) {
				myRigidbody.velocity = new Vector2 (horizontal * movementSpeed, myRigidbody.velocity.y);               	
		} else {
			if(knockFromRight) 
				myRigidbody.velocity = new Vector2(-knockback, knockback);
			if(!knockFromRight)
				myRigidbody.velocity = new Vector2(knockback, knockback);
			knockbackCount -= Time.deltaTime;
		}

        myAnimator.SetFloat("speed", Mathf.Abs(horizontal));

        if(jump)
            myRigidbody.velocity = new Vector2(0, jumpHeight);
    }

    private void HandleAttack()
    {
        if(attack)
            myAnimator.SetTrigger("attack");
    }
    
    private void HandleInput()
    {  

		if (Time.time >= timestamp && Input.GetKeyDown (KeyCode.RightControl)) {
			attack = true;
			Instantiate(ninjaStar, firePoint.position, firePoint.rotation);
			timestamp = Time.time + shotDelay;
			//* Use * commented shotDelayCounter to fire while button pressed
			//*shotDelayCounter = shotDelay;
		} 


		//*if (Input.GetKey (KeyCode.RightControl)) {
		//* shotDelayCounter -= Time.deltaTime;

			//*if(shotDelayCounter <= 0) {
			//*	shotDelayCounter = shotDelay;
			//*	Instantiate(ninjaStar, firePoint.position, firePoint.rotation);
			//*}
		//*}


        if (grounded)
            doubleJumped = false;

        if(touchingWall)
        {
            grounded = false;
            doubleJumped = false;
        }

		if (Input.GetKeyDown (KeyCode.Space) && grounded) {
			jump = true;
			audioSources[0].Play();
		}            

        if (Input.GetKeyDown(KeyCode.Space) && !doubleJumped && !grounded)
        {
            jump = true;
            doubleJumped = true;
			audioSources[0].Play();
        }

        if (touchingWall)
        {
            Input.ResetInputAxes();
        }
    }

	private void Flip(float horizontal) {
		if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight) {
			facingRight = !facingRight;

			Vector3 theScale = transform.localScale;
			theScale.x *= -1;

			transform.localScale = theScale;
		}
	}

    private void ResetValues()
    {
        attack = false;
        jump = false;        
    }
}