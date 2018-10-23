using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{

    //Config
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    

    // State
    bool isAlive = true;

    // Cached References
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    Collider2D myCollider2D;

    float playerGravityAtStart;
    

    // Use this for initialization
    void Start ()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCollider2D = GetComponent<Collider2D>();
        playerGravityAtStart = myRigidBody.gravityScale;
	}
	
	// Update is called once per frame
	void Update ()
    {
        Run();
        FlipSprite();
        Jump();
        ClimbLadder();
	}

   private void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal"); // value is between -1 and +1
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;
        

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    private void ClimbLadder()
    {
        if (!myCollider2D.IsTouchingLayers(LayerMask.GetMask("Ladders")))
        {
            myAnimator.SetBool("isClimbing", false);
            myRigidBody.gravityScale = playerGravityAtStart;
            return;
        }
        float climbThrow = CrossPlatformInputManager.GetAxis("Vertical");
        Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, climbThrow * climbSpeed);
        myRigidBody.velocity = climbVelocity;
        myRigidBody.gravityScale = 0f;

        bool playerHasVeritcalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("isClimbing", playerHasVeritcalSpeed);

    }



    private void Jump()
    {


        if (CrossPlatformInputManager.GetButtonDown("Jump") && myCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            myRigidBody.velocity += jumpVelocityToAdd;
        }
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);

        }
    }
}
