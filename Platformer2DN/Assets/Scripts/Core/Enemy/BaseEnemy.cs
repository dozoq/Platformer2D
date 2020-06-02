using Pathfinding;
using platformer.attributes;
using platformer.combat;
using UnityEngine;
using platformer.utils;
using System;
using UnityEngine.Rendering.UI;

namespace platformer.enemy
{
    public class BaseEnemy : MonoBehaviour, IDieable
    {
        //Difference between waypoint and path
        /*
         *Path - is full path form object to a destination
         *Waypoint - is path from object to (determined by pathfinding) node
             */

        //ToDo can't reach path
        [Tooltip("Max range within which enemy start chasing player")]
        public float MaxDetectionRange = 10f;
        [Tooltip("Container of all patrol paths")]
        public GameObject[] PatrolPaths;
        [Tooltip("Does enemy fly?")]
        public bool isFlying = false;
        [Tooltip("enemy is dying?")]
        private bool isDying = false;
        [Tooltip("enemy can attack and move simultaneously?")]
        public bool canAttackAndMove = false;
        [Tooltip("speed of enemy movement(and jump)")]
        public float Speed = 200f;
        [Tooltip("Distance within which waypoints are checked as done")]
        public float NextWaypointDistance = 3f;
        [Tooltip("Max distance of attack")]
        public float MaxAttackRange = 3f;
        [Tooltip("how much damage single projectile will do")]
        public int EnemyDamage = 2;
        [Tooltip("projectile to instantiate")]
        public Bullet Projectile;
        [Tooltip("How long projectile will live")]
        public float ProjectileLifespan = 3f;
        [Tooltip("How quickly projectile is")]
        public float ProjectileSpeed = 3f;
        [Tooltip("Handles effects of projectile")]
        public GameObject ProjectileVFX;
        [Tooltip("how many seconds need to pass between attacks(Less mean more attacks per sec)")]
        public float FireRate = 0.5f;
        [Tooltip("Attack timer variable")]
        protected float fireTimer = 0.0f;
        [Tooltip("How fast ai should refresh(in seconds, default 0.2 sec)")]
        [SerializeField] protected float behaveRefreshTime = 0.2f;
        [Tooltip("Jump force(multiply normal movement speed to simulate jump)")]
        [SerializeField] protected float jumpforce = 3;
        [Tooltip("Determine where jump starts")]
        [Range(0,1)]
        [SerializeField] protected float jumpDetectionStart = 0.3f;
        //Transform of searched target
        protected Transform Target;
        //reched end of path?
        protected bool reachedEndOfPath;
        //Chasing a target?
        protected bool isChasing;
        //Actual path container
        protected Path path;
        protected int lastPoint = -1;
        //Actual point of patrol path
        protected int pointNumber;
        //Actual waypoint in path
        protected int currentWaypoint;
        //External script that handles pathfinding
        protected Seeker seeker;
        //Rigidbody2D that is use to handle movement
        protected Rigidbody2D rb;

        //is waiting?
        bool isWaiting = false;

        protected bool haveOwnBehavior = false;
        protected bool haveOwnAttack = false;

        //Animator component to handle animation
        protected Animator animator;

        //Timer properties
        //Time on start of timer
        private float time = 0.0f;
        //how many seconds must pass to do tick
        public float interpolationPeriod = 10;

        //Patrol wait checking
        private float actualWaitTime = 0.0f;
        // Start is called before the first frame update
        protected virtual void Start()
        {
            //set point of patrol paths to first
            pointNumber=0;
            //Get seeker to handle paths finding
            seeker=GetComponent<Seeker>();
            //Get rigidbody to handle movement of enemy
            rb=GetComponent<Rigidbody2D>();
            //Find target on scene
            Target=GameObject.FindGameObjectWithTag("Player").transform;
            //Set to not chase on start
            isChasing=false;
            //Repeat function behave every behave refresh time(in seconds, default 0.2 sec)
            InvokeRepeating("Behave", 0f, behaveRefreshTime);
            animator=GetComponent<Animator>();
        }
        // Physics update
        protected virtual void FixedUpdate()
        {
            //Call move to handle movement
            Move();
        }
        // Update is called once per frame
        protected virtual void Update()
        {
            //If enemy is dying
            if(isDying)
            {
                //if enemy is invoking function called behave()
                if(IsInvoking("Behave"))
                {
                    //Then stop invoking
                    CancelInvoke("Behave");
                }
                //if animator done playing animation
                if(!animator.IsPlaying())
                {
                    //Destroy this object
                    Destroy(gameObject);
                }
            }
            //if fire timer have dealy
            if(fireTimer>0)
            {
                //remove passed time from delay
                fireTimer-=Time.deltaTime;
            }
            //add time to patrol timer
            time+=Time.deltaTime;
            //if on end of path and not chasing
            if(reachedEndOfPath&&!isChasing)
            {
                //add passed time to wait timer
                actualWaitTime+=Time.deltaTime;

                //if there exist animator
                if(animator!=null)
                {
                    //set IsWalking boolean to false
                    animator.SetBool("IsWalking", false);
                }
            }

            //Temp variable for calculation and conditions check
            float additionalWaitTime = 0;
            //check if timer passed interpolation period
            PatrolPath patrolpath = null;
            //if there are any patrol paths
            if(PatrolPaths.Length>0)
            {
                //take PatrolPath component from actual path
                patrolpath=PatrolPaths[ pointNumber ].GetComponent<PatrolPath>();
            }
            //if there exist patrol path component
            if(patrolpath!=null)
            {
                //add wait time from component to temp var
                additionalWaitTime=patrolpath.waitTime;
            }
            //if wait timer is bigger or equal to wait time + debug time
            if(time>=(interpolationPeriod+additionalWaitTime))
            {
                //Timer reset
                time=0.0f;

                //if there is no last point
                if(lastPoint==-1)
                {
                    //set last point to this point
                    lastPoint=pointNumber;
                    //exit
                    return;
                }
                //if last point is the same as {Period} ago
                if(lastPoint==pointNumber)
                {
                    //Then go to the next point with debug flag
                    GoToTheNextPoint(true);
                    //Log where node is bugged
                    Debug.LogError($"Can't reach from {lastPoint} to {lastPoint+1} Patrol waypoints");
                }
                //if they are diffrent
                else
                {
                    //Save this point as last
                    lastPoint=pointNumber;
                }
            }
        }
        //Handles all base anamy behavior: moving, chasing, jumping etc.
        protected virtual void Behave()
        {

            //If there isn't any player(object with tag="Player") on map
            if(Target==null)
            {
                //Throw new exception
                throw new System.Exception("Instantiate player first");
            }
            //Temporary X and Y coordinates
            float tempX;
            float tempY;
            //Check with coordinates are bigger and remove from bigger the smaller one to calculate distance
            if(Target.transform.position.x>this.gameObject.transform.position.x)
            {
                tempX=Target.transform.position.x-this.gameObject.transform.position.x;
            }
            else
            {
                tempX=this.gameObject.transform.position.x-Target.transform.position.x;
            }
            if(Target.transform.position.x>this.gameObject.transform.position.x)
            {
                tempY=Target.transform.position.y-this.gameObject.transform.position.y;
            }
            else
            {
                tempY=this.gameObject.transform.position.y-Target.transform.position.y;
            }

            //If target is in max detection range on X and Y distance
            if(
                tempX<MaxDetectionRange&&
                tempY<MaxDetectionRange
                )
            {
                //set chasing to true
                isChasing=true;
            }
            //if target is not in max detection range on X and Y distance
            else
            {
                //stop chasing
                isChasing=false;
            }

            //If enemy is chasing a target
            if(isChasing)
            {
                //Set new path to a target
                //if target in attack range
                if(
                tempX<MaxAttackRange&&
                tempY<MaxAttackRange
                )
                {
                    if((gameObject.transform.position.x-Target.transform.position.x)>0)
                    {
                        //rotate to right
                        transform.localScale=new Vector3(1f, 1f, 1f);
                    }
                    else
                    {
                        //rotate to left
                        transform.localScale=new Vector3(-1f, 1f, 1f);
                    }
                    //call attack function
                    Attack();
                    if(canAttackAndMove)
                    {
                        seeker.StartPath(rb.position, Target.position, OnPathComplete);
                    }
                }
                else {
                    seeker.StartPath(rb.position, Target.position, OnPathComplete);

                }

            }
            //If not chasing
            else
            {
                GoToTheNextPoint();
            }
            if(haveOwnBehavior)
            {
                specialBehave();
            }
        }

        //special enemies can override this method to don't change general behavior but add own
        protected virtual void specialBehave()
        {

        }
        //special enemies can override this method to don't change general attack but add own
        protected virtual void specialAttack()
        {

        }

        protected virtual void Attack()
        {
            if(!haveOwnAttack)
            {

                //if there is no delay on attack
                if(fireTimer<=0)
                {
                    //if animator exists
                    if(animator!=null)
                    {
                        //Call Attack triger from animator
                        animator.SetTrigger("Attack");
                    }
                    //add delay to timer
                    fireTimer=FireRate;
                    //instantiate projectile
                    Bullet bulletInstance = Instantiate(Projectile,new Vector3(transform.position.x,transform.position.y,0), Quaternion.identity,null);
                    //Config projectile
                    bulletInstance.ConfigBullet(EnemyDamage, ProjectileLifespan, ProjectileSpeed, false, ProjectileVFX, true);
                    //Set destination and force to bullet
                    bulletInstance.SetTarget(bulletInstance.transform.position, new Vector2(Target.position.x, Target.position.y));
                }
            }
            else
            {
                specialAttack();
            }
        }

        protected void GoToTheNextPoint(bool debug = false)
        {
            //check if reached end of his actual path
            if(reachedEndOfPath||debug)
            {
                //set that enemy is waiting on point
                isWaiting=true;
                //set animator booleant IsWalking to false
                animator.SetBool("IsWalking", false);
                //create temp var for conditions
                PatrolPath patrolpath = null;
                //if there are any patrol path
                if(PatrolPaths.Length>0)
                {
                    //take PatrolPath component from current patrol path
                    patrolpath =PatrolPaths[ pointNumber ].GetComponent<PatrolPath>();
                }
                //if PatrolPath component is empty or wait timer is bigger or equal to patrol path wait time
                if(patrolpath==null||actualWaitTime>=patrolpath.waitTime)
                {
                    //stop waiting
                    isWaiting=false;
                    //set animator boolean IsWalking to true
                    animator.SetBool("IsWalking", true);
                    //reset wait timer
                    actualWaitTime=0;
                    //If so, check if there are more points in patrol nodes container
                    if(pointNumber<(PatrolPaths.Length-1))
                    {
                        //if so, set actual point to the next
                        pointNumber++;
                    }
                    else
                    {
                        //if not, set actual point to first
                        pointNumber=0;
                    }
                }
            }
            //If there are any patrol paths
            if(PatrolPaths.Length>0)
            {
                //set path to actual point from patrol nodes
                seeker.StartPath(rb.position, PatrolPaths[ pointNumber ].transform.position, OnPathComplete);
            }
        }

        //implementation of die interface
        public virtual void Die()
        {
            //if there is animator
            if(animator!=null)
            {
                //Call Die triger from animator
                animator.SetTrigger("Die");
            }
            //set that enemy start dying
            isDying=true;
        }

        
        //Function called on path completion(takes path as argument)
        protected virtual void OnPathComplete(Path p)
        {
            //If there is no error
            if(!p.error)
            {
                //Set this enemy path to path p
                path=p;
                //Set waypoint of path to first point
                currentWaypoint=0;
            }
            //If there is an error
            else
            {
               //Log that error
                Debug.LogError(p.errorLog);
            }
        }
       

        //Function that handles enemy movement
        protected virtual void Move()
        {
            //if there is no path
            if(path==null)
            {
                //Do nothing
                return;
            }
            //if actual waypoint on path is equal to number of waypoints
            if(currentWaypoint>=path.vectorPath.Count)
            {
                //set that path is ended
                reachedEndOfPath=true;
                //end
                return;
            }
            //in other situation
            else
            {
                //set that path is not ended
                reachedEndOfPath=false;
            }
            //create force var
            Vector2 force = Vector2.zero;
            //calculate direction to next point
            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            //if is not waiting or is chasing
            if(!isWaiting || isChasing)
            {
                //if ai want to go to much upward and enemy is not flying type
                if(direction.y>jumpDetectionStart&&!isFlying)
                {
                    //add more force(to simulate jump)
                    force=direction*Speed*jumpforce*Time.deltaTime;
                }
                //if enemy go as far as forward
                else
                {
                    //use normal speed
                    force=direction*Speed*Time.deltaTime;
                    //set animator boolean IsWalking to true
                    animator.SetBool("IsWalking",true);
                }
            }
            //Add calculated force to rigid body
            rb.AddForce(force);

            //calculate distance from enemy to next path waypoint
            float distance = Vector2.Distance(rb.position,path.vectorPath[currentWaypoint]);

            //if distance is less than acteptable distance
            if(distance<NextWaypointDistance)
            {
                //go to the next waypoint on path
                currentWaypoint++;
            }
            //if force is directed to left
            if(force.x>=0.01f)
            {
                //Rotate to left
                transform.localScale=new Vector3(-1f, 1f, 1f);
            }
            //if force is directed to right
            else if(force.x<=-0.01f)
            {
                //rotate to right
                transform.localScale=new Vector3(1f, 1f, 1f);
            }
        }
    }
}
