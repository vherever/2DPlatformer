using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private Rigidbody2D myRigidbody;

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

    // Use this for initialization
    void Start () {
		facingRight = true;
		myRigidbody = GetComponent<Rigidbody2D> ();
        myAnimator = GetComponent<Animator>();
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
        
        myRigidbody.velocity = new Vector2(horizontal * movementSpeed, myRigidbody.velocity.y);               	

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

        if(Input.GetKeyDown(KeyCode.RightControl))
            attack = true;

        if (grounded)
            doubleJumped = false;

        if(touchingWall)
        {
            grounded = false;
            doubleJumped = false;
        }

        if(Input.GetKeyDown(KeyCode.Space) && grounded)
            jump = true;

        if (Input.GetKeyDown(KeyCode.Space) && !doubleJumped && !grounded)
        {
            jump = true;
            doubleJumped = true;
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