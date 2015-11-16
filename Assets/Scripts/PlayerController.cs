using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private Rigidbody2D myRigidbody;

    private Animator myAnimator;

	[SerializeField]
	private float movementSpeed;

    private bool attack;
    
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

		Flip(horizontal);

        HandleAttack();

        ResetValues();
    }

	private void HandleMovement(float horizontal) {
       
        myRigidbody.velocity = new Vector2(horizontal * movementSpeed, myRigidbody.velocity.y);
        	

        myAnimator.SetFloat("speed", Mathf.Abs(horizontal));
	}

    private void HandleAttack()
    {
        if(attack)
        {
            myAnimator.SetTrigger("attack");           
        }
    }

    private void HandleInput()
    {
        if(Input.GetKeyDown(KeyCode.RightControl))
        {
            attack = true;
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
    }
}



















































