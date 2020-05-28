using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
namespace platformer.control
{
    public class PhysicsObject : MonoBehaviour
    {
        public float gravityModifier = 1f;
        public float minGroundNormalY = 0.9f;

        protected Vector2 targetVelocity;
        protected bool isGrounded;
        protected Vector2 groundNormal;
        protected Vector2 velocity;
        protected Rigidbody2D rigidBody2D = null;
        protected const float minMoveDistance = 0.001f;
        protected const float shellRadius = 0.01f;
        protected ContactFilter2D contactFilter;
        protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
        protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);
        protected GameObject groundDetector = null; //TODO: might be removed
        protected bool hasFloorChanged = false; //TODO: might be removed
        protected float maxWalkableRotation = 0.3f; //TODO: might be removed
        protected bool isGroundWalkable = true; //TODO: might be removed

        private GameObject currentFloor = null; //TODO: might be removed

        protected virtual void Awake()
        {
            groundDetector = this.transform.GetChild(0).gameObject;
        }

        void OnEnable()
        {
            rigidBody2D = GetComponent<Rigidbody2D>();
        }

        protected virtual void Start()
        {
            contactFilter.useTriggers = false;
            contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(this.gameObject.layer));
            contactFilter.useLayerMask = true;
        }

        protected virtual void Update()
        {
            targetVelocity = Vector2.zero;
            ComputeVelocity();
        }

        protected virtual void ComputeVelocity()
        {
            // Overriden in PlayerController
        }

        void FixedUpdate()
        {
            velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
            velocity.x = targetVelocity.x;

            isGrounded = false;

            // CheckIfGroundHasChanged();  //TODO: If not needed, remove after testing

            Vector2 deltaPosition = velocity * Time.deltaTime;

            Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);

            Vector2 move = moveAlongGround * deltaPosition.x;

            Movement(move, false);

            move = Vector2.up * deltaPosition.y;

            Movement(move, true);


        }

        private void Movement(Vector2 move, bool yMovement) //yMovement - vertical movement
        {
            float distance = move.magnitude;

            if (distance > minMoveDistance)
            {
                int count = rigidBody2D.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
                hitBufferList.Clear();
                for (int i = 0; i < count; i++)
                {
                    hitBufferList.Add(hitBuffer[i]);
                }

                for (int i = 0; i < hitBufferList.Count; i++)
                {
                    Vector2 currentNormal = hitBufferList[i].normal;
                    var currentRotation = hitBufferList[i].transform.rotation;
                    if (currentNormal.y > minGroundNormalY)
                    {
                        isGrounded = true;

                        SetIsGroundWalkable(currentRotation.z);
                        if (yMovement) //TODO: Check if not causing null reference exepctions
                        {
                            groundNormal = currentNormal;
                            currentNormal.x = 0;
                            // Debug.Log("ground normal changed");
                        }
                    }

                    float projection = Vector2.Dot(velocity, currentNormal);
                    if (projection < 0)
                    {
                        velocity = velocity - projection * currentNormal;
                    }

                    float modifiedDistance = hitBufferList[i].distance - shellRadius;
                    distance = modifiedDistance < distance ? modifiedDistance : distance;
                }

            }

            if (!isGroundWalkable)
            {
                // gravityModifier = 10f;
            }

            rigidBody2D.position = rigidBody2D.position + move.normalized * distance;


        }

        private void SetIsGroundWalkable(float zRotation)
        {
            //  print(zRotation);
            if (Mathf.Abs(zRotation) > maxWalkableRotation)
            {
                isGroundWalkable = false;
                //print("ground not walkable");
            }
            else
            {
                isGroundWalkable = true;
                //print("ground walkable");
            }
        }

        private void CheckIfGroundHasChanged() //TODO: remove function
        {
            //Debug.DrawRay(groundDetector.transform.position, Vector2.down, Color.green); // TODO: Remove this line

            var hitInfo = Physics2D.Raycast(groundDetector.transform.position, Vector2.down);
            if (hitInfo.collider == null)
            {
                return;
            }

            if (hitInfo.collider.gameObject == currentFloor)
            {
                return;
            }
            if (hitInfo.collider == this.GetComponent<BoxCollider2D>())
            {
                Debug.LogError("in PhysicsObject Raycast hit player - should not happen");
                return;
            }

            currentFloor = hitInfo.collider.gameObject;
            hasFloorChanged = true;

            print(currentFloor);


        }
    }

}
*/