using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace platformer.interactive
{
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
        [Tooltip("Torelance between current position and waypoint position")]
        [Range(0.03f, 0.2f)]
        [SerializeField] private float waypointDistanceTolerance = 0.1f;
        [Tooltip("At this distance elevator will start to slow down")]
        [SerializeField] private float brakingDistance = 2f;

        private ElevatorState currentState;

        private Transform[] waypoints;
        private bool isActivated = false;
        private Transform currentWaypointDestination = null;
        private bool isAtWaypoint = true; //TODO: change to false

        private float currentAccelerationMultiplier = 0.1f;
        private float brakeDistance = 0f;
        private float startingAcceleration = 0.1f;
        private Rigidbody2D rb;

        private Vector2 direction;
        private bool isBrakeDistanceSet = false;

        //Timers
        private float timeAtWaypoint = Mathf.Infinity;


        private float currentBrakeStrength = 1f;

        


        public void HandleInteraction(GameObject who)
        {
            // if isActivated elevator will automaticly try to reach next waypoints
            isActivated = !isActivated; // Switch whenever pressed the button to true or false
            if(isActivated)
            {
                currentState = ElevatorState.waiting;
                currentBrakeStrength = 1f;
            }
            else if (!isActivated)
            {
                ResetAllForces();
            }
            print(isActivated);
        }

        private void Start()
        {
            waypoints = elevatorWaypoints.transform.Cast<Transform>().ToArray();

            if(currentWaypointDestination == null)
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
        {   if (!isActivated && (currentState != ElevatorState.finish && currentState != ElevatorState.waiting))
            {
                if (currentWaypointDestination == null) return;

                currentBrakeStrength -= 0.01f;

                //rb.velocity = direction * elevatorSpeed * currentBrakeStrength;

                rb.velocity *= currentBrakeStrength;

                if (currentBrakeStrength <= 0.01f)
                {
                    rb.velocity = new Vector2(0, 0);
                    ResetAllForces();
                    print("Reseting brake strength to 1");
                    currentState = ElevatorState.waiting;
                }
            }
            
            
            if (!isActivated) return; // Don`t check anything else if elevator is disabled


            switch(currentState)
            {
                case ElevatorState.accelerating:
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
                    timeAtWaypoint = 0f;
                    currentState = ElevatorState.waiting;
                    break;

                case ElevatorState.waiting:
                    if(timeAtWaypoint >= waypointWaitTime)
                    {
                        direction = CalculateDirection(this.transform.position, currentWaypointDestination.position).normalized;
                        currentState = ElevatorState.accelerating;
                    }
                    break;

            }

           // CheckIfReachedNextWaypoint();

        }

        private void Update()
        {
            if (!isActivated) return; // Dont calculate timers if elevator is disabled

            if(isAtWaypoint)
            {
                timeAtWaypoint += Time.deltaTime;
            }
        }

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

        private void Accelerate(Vector2 direction)
        {
            currentAccelerationMultiplier *= accelerationMultiplier;
            currentAccelerationMultiplier = Mathf.Clamp01(currentAccelerationMultiplier);

            rb.velocity = direction * elevatorSpeed * currentAccelerationMultiplier;
            
            if(currentAccelerationMultiplier >= 1f)
            {
                currentAccelerationMultiplier = startingAcceleration; // Starting acceleration
                currentState = ElevatorState.fullSpeed; 
            }
        }

        private void ResetAllForces()
        {
            currentBrakeStrength = 1f;
            currentAccelerationMultiplier = startingAcceleration;
            isBrakeDistanceSet = false;
        }

        private void CheckIfReachedNextWaypoint()
        {
            if (currentState == ElevatorState.finish || currentState == ElevatorState.waiting) return;

            if(Vector2.Distance(this.transform.position, currentWaypointDestination.position) <= waypointDistanceTolerance)
            {
                currentState = ElevatorState.finish;
                rb.velocity = new Vector2(0, 0);
            }
        }

        private void CheckIfShouldStartBraking()
        {
           // if (isAtWaypoint) return;

            if (Vector2.Distance(this.transform.position, currentWaypointDestination.position) <= brakingDistance)
            {
                currentState = ElevatorState.braking;
            }
        }

        private void Brake()
        {
            if(!isBrakeDistanceSet)
            {
                brakeDistance = Vector2.Distance(this.transform.position, currentWaypointDestination.position);
                isBrakeDistanceSet = true;
            }

            float currentBrakeDistance = Vector2.Distance(this.transform.position, currentWaypointDestination.position);
            float brakeFraction = currentBrakeDistance / brakeDistance;

            rb.velocity = direction * elevatorSpeed * brakeFraction;
            //rb.velocity *= brakeFraction;

            if (brakeFraction <= 0.01f)
            {
                rb.velocity = new Vector2(0, 0);
                isBrakeDistanceSet = false;
                currentState = ElevatorState.finish;
            }
           
        }


        private Vector2 CalculateDirection(Vector3 origin, Vector3 direction)
        {
            float x = direction.x - origin.x;
            float y = direction.y - origin.y;
            return new Vector2(x, y);
        }

    }

}