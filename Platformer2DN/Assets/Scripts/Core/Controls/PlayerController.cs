using platformer.combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// New player controller from scratch
namespace platformer.control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float maxSpeed = 6f;
        [SerializeField] private float jumpHeight = 8f;
        [SerializeField] private float ladderClimbSpeed = 3f;
        [SerializeField] private Transform groundDetector = null;
        [SerializeField] private bool controlsEnabled = true;


        [Range(0, 1)]
        [SerializeField] private float lowJumpGravityModifier = 0.5f;

        [Range(0, 1)]
        [SerializeField] private float ladderHorizontalMovementModifier = 0.5f;

        private float currentLadderMovementModifier = 1f;
        private bool isGrounded = false;
        private bool isOnLadder = false;
        private bool interactedWithLadder = false;
        private Rigidbody2D rb = null;
        private Fighter fighter;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            fighter = GetComponent<Fighter>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Shoot(); // Launch Bullet
            }
            
        }
        private void FixedUpdate()
        {
            SetIsPlayerGrounded(); // Check whether player is on ground or not
            InteractWithMovement(); // X,Y axis (horizontal and ladders)
            InteractWithJumping(); // along Y axis (jumping)
        }

        // Check whether player is on the ground or not. Called each fixed update
        private void SetIsPlayerGrounded()
        {
            if (Physics2D.Linecast(transform.position, groundDetector.position, 1 << LayerMask.NameToLayer("Obstacles")))
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }

        //Called each fixed update
        private void InteractWithMovement()
        {
            if (Input.GetKey(KeyCode.A))
            {
                rb.velocity = new Vector2(-maxSpeed * currentLadderMovementModifier, rb.velocity.y); // No overriding y velocity
                spriteRenderer.flipX = true;

            }
            else if (Input.GetKey(KeyCode.D))
            {
                rb.velocity = new Vector2(maxSpeed * currentLadderMovementModifier, rb.velocity.y);
                spriteRenderer.flipX = false;
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }

            //Ladders
            if(Input.GetKey(KeyCode.Space) && isOnLadder && interactedWithLadder)
            {
                interactedWithLadder = false;
                isGrounded = false;
                rb.gravityScale = 1;
                print("jumponladder");
            }

            if(Input.GetKey(KeyCode.W) && isOnLadder)
            {
                rb.velocity = new Vector2(rb.velocity.x, ladderClimbSpeed);
                currentLadderMovementModifier = ladderHorizontalMovementModifier;
                interactedWithLadder = true;
                rb.gravityScale = 0;
                //isGrounded = true;
            }
            else if(Input.GetKey(KeyCode.S) && isOnLadder)
            {
                rb.velocity = new Vector2(rb.velocity.x, -ladderClimbSpeed);
                currentLadderMovementModifier = ladderHorizontalMovementModifier;
                interactedWithLadder = true;
                rb.gravityScale = 0;
                //isGrounded = true;
            }
            else if(isOnLadder && interactedWithLadder)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                currentLadderMovementModifier = ladderHorizontalMovementModifier;
            }

        }

        //Called each fixed update
        private void InteractWithJumping()
        {
            // If player pressed space and is on the ground, jump
            if (Input.GetKey(KeyCode.Space) && isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            }
            //If player release space, he`s in the air and not falling down, stop jumping higher
            if (Input.GetKeyUp(KeyCode.Space) && !isGrounded && rb.velocity.y > 0)
            {
                print(rb.velocity.y);
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * lowJumpGravityModifier);
            }

        }


        //Launch bullet. Called each update if the button is pressed
        private void Shoot()
        {
             fighter.Shoot(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                             Input.mousePosition.y, Camera.main.nearClipPlane)));
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            //Check if colliding object is a Ledder
            if(collision.CompareTag("Ledder"))
            {
                isOnLadder = true;
                isGrounded = true;
               // rb.gravityScale = 0; // Won`t fall down when is sticked to the ladder
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if(collision.CompareTag("Ledder"))
            {
                isOnLadder = false;
                isGrounded = false;
                interactedWithLadder = false;
                rb.gravityScale = 1;
                currentLadderMovementModifier = 1; //Default
            }
        }


    }

}
