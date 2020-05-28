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
        [SerializeField] private Transform groundDetector = null;

        [Range(0, 1)]
        [SerializeField] private float lowJumpGravityModifier = 0.5f;

        private bool isGrounded = false;
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
            InteractWithMovement(); // along X axis
            InteractWithJumping(); // along Y axis
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
                rb.velocity = new Vector2(-maxSpeed, rb.velocity.y); // No overriding y velocity
                spriteRenderer.flipX = true;

            }
            else if (Input.GetKey(KeyCode.D))
            {
                rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
                spriteRenderer.flipX = false;
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
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


    }

}
