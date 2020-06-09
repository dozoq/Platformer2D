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
        [SerializeField] private float accelerationMultiplier = 1.1f;
        [Tooltip("Distance to waypoint tolerance")]
        [SerializeField] private float waypointDistanceTolerance = 0.05f;

        private ElevatorState currentState;

        private Transform[] waypoints;
        private bool isActivated = false;
        private Transform currentWaypointDestination = null;
        private bool isAtWaypoint = true; //TODO: change to false

        private float currentAccelerationMultiplier = 0.1f;

        private Vector2 direction;

        //Timers
        private float timeAtWaypoint = Mathf.Infinity;

        private Rigidbody2D rb;


        public void HandleInteraction(GameObject who)
        {
            // if isActivated elevator will automaticly try to reach next waypoints
            isActivated = !isActivated; // Switch whenever pressed the button to true or false
            if(!isActivated)
            {
                rb.velocity = new Vector2(0, 0);
            }
            print(isActivated);
        }

        private void Start()
        {
            //currentWaypoint = transform.GetChildCount(0);
            waypoints = elevatorWaypoints.transform.Cast<Transform>().ToArray();
            if(currentWaypointDestination == null)
            {
                currentWaypointDestination = waypoints[0];
            }
        }

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (!isActivated) return; // Don`t check anything else if elevator is disabled

            if (isAtWaypoint && timeAtWaypoint >= waypointWaitTime)
            {
                isAtWaypoint = false;
                timeAtWaypoint = 0;

                currentWaypointDestination = GetNextWaypoint();


                //rb.velocity = direction * elevatorSpeed;
                currentState = ElevatorState.accelerating;
            }

            switch(currentState)
            {
                case ElevatorState.accelerating:
                    // Optimize it later
                    direction = CalculateDirection(this.transform.position, currentWaypointDestination.position).normalized;
                    Accelerate(direction);
                    break;

                case ElevatorState.fullSpeed:
                    // Check if distance to currentwaypointdestionation is greater than braking distance
                    break;

                case ElevatorState.braking:
                    // Slow down elevator
                    break;

                case ElevatorState.finish:
                    // Stop elevator
                    // GetNextWaypoint
                    // Switch to waiting

                    break;

                case ElevatorState.waiting:
                    // Wait for waitTime
                    // After switch to accelerate
                    break;

                case ElevatorState.disabled:
                    // Might change at any time to stop, will implement later
                    break;

            }


            CheckIfReachedNextWaypoint();
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
            Mathf.Clamp01(currentAccelerationMultiplier);
            rb.velocity = direction * elevatorSpeed * currentAccelerationMultiplier;
            
            if(currentAccelerationMultiplier >= 1f)
            {
                currentState = ElevatorState.fullSpeed;
                currentAccelerationMultiplier = 0.1f; // Starting acceleration
            }
        }

        private void CheckIfReachedNextWaypoint()
        {
            if (isAtWaypoint) return;

            if(Vector2.Distance(this.transform.position, currentWaypointDestination.position) <= waypointDistanceTolerance)
            {
                isAtWaypoint = true;
                timeAtWaypoint = 0;
                rb.velocity = new Vector2(0, 0);
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