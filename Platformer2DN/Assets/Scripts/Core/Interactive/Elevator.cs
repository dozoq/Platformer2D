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
            starting,
            fullSpeed,
            finishing,
            waiting
        }

        [SerializeField] private Transform elevatorWaypoints;
        [SerializeField] private float waypointWaitTime = 1f;
        [SerializeField] private float elevatorSpeed = 3f;
        [SerializeField] private float acceleration = 1.1f;
        [SerializeField] private float waypointDistanceTolerance = 0.5f;

        Transform[] waypoints;
        private bool isActivated = false;
        private Transform currentWaypoint = null;
        private bool isAtWaypoint = true; //TODO: change to false

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
            if(currentWaypoint == null)
            {
                currentWaypoint = waypoints[0];
            }
        }

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (!isActivated) return; // Don`t check anything else if elevator is disabled

            if(isAtWaypoint && timeAtWaypoint >= waypointWaitTime)
            {
                isAtWaypoint = false;
                timeAtWaypoint = 0;

                currentWaypoint = GetNextWaypoint();
                
                Vector2 direction = CalculateDirection(this.transform.position, currentWaypoint.position).normalized;
                rb.velocity = direction * elevatorSpeed;
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
                if (currentWaypoint == waypoints[i] && i+1 < waypoints.Length)
                {
                    print("Next waypoint = " + waypoints[i+1]);
                    return waypoints[i + 1];
                }
            }

            print("No next waypoint, returning to 0");
            return waypoints[0];
        }

        private void CheckIfReachedNextWaypoint()
        {
            if (isAtWaypoint) return;

            if(Vector2.Distance(this.transform.position, currentWaypoint.position) <= waypointDistanceTolerance)
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