using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace platformer.interactive
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Elevator : MonoBehaviour, IInteractionHandler
    {
        enum ElevatorState
        {
            accelerating, // Accelerate the elevator
            fullSpeed, // When reached fullsped
            braking, // Close to waypoint
            finish, // At waypoint, GetNextWaypoint() and switch to waiting
            waiting, // Wait for configurable time at waypoint, after time switch to accelerate
            disabled // When console disabled elevator
        }

        [Tooltip("Parent of empty Waypoints Gameobjects")]
        [SerializeField] private Transform elevatorWaypoints;
        [Tooltip("How long to wait after reaching waypoint?")]
        [SerializeField] private float waypointWaitTime = 1f;
        [Tooltip("Max available full speed of this elevator")]
        [SerializeField] private float elevatorSpeed = 3f;
        [Tooltip("How fast elevator will accelerate?")]
        [Range(1.01f, 1.1f)]
        [SerializeField] private float accelerationMultiplier = 1.03f;
        [Tooltip("At this distance elevator will start to slow down")]
        [SerializeField] private float brakingDistance = 2f;
        [Tooltip("If activated, elevator will turn off after reaching next waypoint")]
        [SerializeField] private bool oneWaypointMode = false;

        private ElevatorState currentState;

        private Rigidbody2D rb;
        private Vector2 direction;
        private Transform[] waypoints;       
        private Transform currentWaypointDestination = null;

        private bool isAtWaypoint = true; //TODO: change to false
        private bool isActivated = false;
        private bool isBrakeDistanceSet = false;

        private float currentAccelerationMultiplier = 0.1f;
        private float brakeDistance = 0f;
        private float startingAcceleration = 0.1f;
        private float currentBrakeStrength = 1f;

        //Timers
        private float timeAtWaypoint = Mathf.Infinity;

        public void HandleInteraction(GameObject who)
        {
            // if isActivated elevator will automaticly try to reach next waypoints
            isActivated = !isActivated; // Switch whenever pressed the button to true or false
            if(isActivated)
            {
                currentState = ElevatorState.waiting;
                currentBrakeStrength = 1f;
                //ResetAllForces();
            }
            else if (!isActivated)
            {
                ResetAllForces();
            }
            print("Console is activated: " + isActivated);
        }


        private void Start()
        {
            // Cast all childrens into waypoints array
            waypoints = elevatorWaypoints.transform.Cast<Transform>().ToArray();

            if(currentWaypointDestination == null) // Set default waypoint (Elevator should be placed on 0 on scene)
            {
                currentWaypointDestination = waypoints[1];
                print("set waypoint to " + currentWaypointDestination);
            }
        }

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            print(isActivated);


            if (!isActivated && (currentState != ElevatorState.finish && currentState != ElevatorState.waiting))
            {
                if (currentWaypointDestination == null)
                {
                    rb.velocity = Vector2.zero;
                    Debug.LogError("Null waypointDestination in elevator. Should be initialized earlier");
                    return;
                }

                EmergencyStop();
            }
            
            
            if (!isActivated) return; // Don`t check anything else if elevator is disabled


            switch (currentState)
            {
                case ElevatorState.accelerating:
                    isAtWaypoint = false;
                    Accelerate(direction);
                    CheckIfShouldStartBraking();
                    break;

                case ElevatorState.fullSpeed:
                    CheckIfShouldStartBraking();
                    break;

                case ElevatorState.braking:
                    Brake();
                    break;

                case ElevatorState.finish:
                    currentWaypointDestination = GetNextWaypoint();
                    ResetAllForces();
                    isAtWaypoint = true;
                    timeAtWaypoint = 0f;
                    currentState = ElevatorState.waiting;
                    if(oneWaypointMode)
                    {
                        isActivated = false;
                    }
                    break;

                case ElevatorState.waiting:
                    if (timeAtWaypoint >= waypointWaitTime || !isAtWaypoint) //Check if should switch to accelerate
                    {
                        direction = CalculateDirection(this.transform.position, currentWaypointDestination.position).normalized;
                        currentState = ElevatorState.accelerating;
                    }
                    break;

            }

        }
        
        /// <summary>
        /// Used only for timers
        /// </summary>
        private void Update()
        {
            if (!isActivated) return; // Dont calculate timers if elevator is disabled

            if(isAtWaypoint)
            {
                timeAtWaypoint += Time.deltaTime;
            }
        }
        

        /// <summary>
        /// Whenever elevator reaches waypoint destination it`s calling this function
        /// so it`s searching for next possible waypoint (first one if current was last one)
        /// </summary>
        /// <returns>Next waypoint transform</returns>
        private Transform GetNextWaypoint()
        {

            for(int i = 0; i < waypoints.Length; i++)
            {
                if (currentWaypointDestination == waypoints[i] && i+1 < waypoints.Length)
                {
                    print("Next waypoint = " + waypoints[i+1]);
                    return waypoints[i + 1];
                }
            }

            print("No next waypoint, returning to 0");
            return waypoints[0];
        }

        /// <summary>
        /// Accelerate the elevator to slowly the max speed
        /// </summary>
        /// <param name="direction">Destination that the elevator will move to</param>
        private void Accelerate(Vector2 direction)
        {
            currentAccelerationMultiplier *= accelerationMultiplier;
            currentAccelerationMultiplier = Mathf.Clamp01(currentAccelerationMultiplier);

            rb.velocity = direction * elevatorSpeed * currentAccelerationMultiplier;
            
            if(currentAccelerationMultiplier >= 1f) // Reached full speed
            {
                //currentAccelerationMultiplier = startingAcceleration; // Starting acceleration
                currentState = ElevatorState.fullSpeed; 
            }
        }

        /// <summary>
        /// Whenever elevator reached waypoint or player turned off the elevator
        /// reset all braking and accelerate forces to defaults
        /// </summary>
        private void ResetAllForces()
        {
            currentBrakeStrength = 1f; //TODO: Check if not causing problems
            currentAccelerationMultiplier = startingAcceleration;
            isBrakeDistanceSet = false;
        }

        /// <summary>
        /// Check if elevator entered the braking zone
        /// </summary>
        private void CheckIfShouldStartBraking()
        {
            if (Vector2.Distance(this.transform.position, currentWaypointDestination.position) <= brakingDistance)
            {
                currentState = ElevatorState.braking;
            }
        }


        /// <summary>
        /// When the elevator is in the configurable brake distance range, it will based on current speed
        /// smoothly stop
        /// </summary>
        private void Brake()
        {
            if(!isBrakeDistanceSet) // Set once when entering the brake state
            {
                brakeDistance = Vector2.Distance(this.transform.position, currentWaypointDestination.position);              
                isBrakeDistanceSet = true;
            }
            //current position to the end
            float currentBrakeDistance = Vector2.Distance(this.transform.position, currentWaypointDestination.position);
            
            // Fraction is decreasing from 1 to 0 as the distance is getting closer
            float brakeFraction = currentBrakeDistance / brakeDistance;

            brakeFraction = Mathf.Clamp01(brakeFraction); // Clamp values between 0 and 1 (infinity case)

            // Slow down elevator
            rb.velocity = direction * elevatorSpeed * brakeFraction * currentAccelerationMultiplier;

            if (brakeFraction <= 0.03f) // Check if almost stopped
            {
                rb.velocity = new Vector2(0, 0);
                isBrakeDistanceSet = false;
                currentState = ElevatorState.finish;
            }
           
        }

        /// <summary>
        /// Called when user deactivate elevator. Slowing down elevator very quick.
        /// </summary>
        private void EmergencyStop()
        {
            // If the elevator is really close to the checkpoint, stop it instantly
            if(Vector2.Distance(this.transform.position, currentWaypointDestination.position) <= 0.1f)
            {
                currentBrakeStrength = 0f;
                rb.velocity = Vector2.zero;
                timeAtWaypoint = 0f;
                currentWaypointDestination = GetNextWaypoint();
            }
            else
            {
                currentBrakeStrength -= 0.01f; // Fast slowdown
            }
            

            currentBrakeStrength = Mathf.Clamp01(currentBrakeStrength);

            //rb.velocity = direction * elevatorSpeed * currentBrakeStrength;

            rb.velocity *= currentBrakeStrength; // Quick slow
            
            if (currentBrakeStrength <= 0.05f)
            {
                rb.velocity = new Vector2(0, 0);
                ResetAllForces();
                currentState = ElevatorState.waiting;
            }
        }

        /// <summary>
        /// Calculating the direction that the elevator should move to
        /// </summary>
        /// <param name="origin">Position of object that is going to be moved</param>
        /// <param name="direction">Position of target</param>
        /// <returns>Direction that the object should move to</returns>
        private Vector2 CalculateDirection(Vector3 origin, Vector3 direction)
        {
            float x = direction.x - origin.x;
            float y = direction.y - origin.y;
            return new Vector2(x, y);
        }

    }

}