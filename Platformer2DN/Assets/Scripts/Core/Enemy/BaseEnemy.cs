using Pathfinding;
using platformer.attributes;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace platformer.enemy
{
    public class BaseEnemy : MonoBehaviour, IDieable
    { 
        public float maxDetectionRange;
        public GameObject[] patrolPaths;
        public Transform Target;
        public bool isFlying = false;
        public float speed = 200f;
        public float nextWaypointDistance = 3f;

        protected bool reachedEndOfPath;
        protected bool isChasing;
        protected Path path;
        protected int pointNumber;
        protected int currentWaypoint;
        protected Seeker seeker;
        protected Rigidbody2D rb;
        //private AIDestinationSetter destinationSetter;
        //private AIPath aipath;

        protected virtual void Behave() 
        {            if (Target==null)
            {
                throw new System.Exception("Instantiate player first");
            }
            float tempX;
            float tempY;
            if (Target.transform.position.x>this.gameObject.transform.position.x)
            {
                tempX=Target.transform.position.x-this.gameObject.transform.position.x;
            }
            else
            {
                tempX=this.gameObject.transform.position.x-Target.transform.position.x;
            }
            if (Target.transform.position.x>this.gameObject.transform.position.x)
            {
                tempY=Target.transform.position.y-this.gameObject.transform.position.y;
            }
            else
            {
                tempY=this.gameObject.transform.position.y-Target.transform.position.y;
            }

            if (
                tempX<maxDetectionRange &&
                tempY<maxDetectionRange
                )
            {
                isChasing=true;
            }
            else
            {
                isChasing=false;
            }
            if (isChasing)
            {
                seeker.StartPath(rb.position, Target.position, OnPathComplete);
                //destinationSetter.target=this.Target.transform;
                //Sprawdź czy w zasięgu
                    //zaatakuj

            }
            else
            {
                if(reachedEndOfPath)
                {
                    if(pointNumber<(patrolPaths.Length-1))
                    {
                        pointNumber++;
                    }
                    else
                    {
                        pointNumber=0;
                    }
                }
                seeker.StartPath(rb.position, patrolPaths[pointNumber].transform.position, OnPathComplete);
                //destinationSetter.target=patrolPaths[ pointNumber ].transform;
            }
            //jeżeli przeszkoda
            //skacz

            //FT
            //RaycastHit2D[] hits = null;
            //Vector3 temp = new Vector3(gameObject.transform.position.x+2,gameObject.transform.position.y,0);
            //hits = Physics2D.RaycastAll(new Vector2(gameObject.transform.position.x,gameObject.transform.position.y+2.75f) ,new Vector2(gameObject.transform.position.x+2,gameObject.transform.position.y+2.75f),3f);
            ////Debug raycast line
            //Debug.DrawLine(gameObject.transform.position, temp);
            //foreach(var hit in hits)
            //{
            //    if(hit.collider.gameObject.layer==10)
            //    {
            //        Debug.Log("We have a wall:"+hit.point);

            //    }
            //}


        }

        public virtual void die()
        {
            Destroy( this.gameObject );
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            //aipath=GetComponent<AIPath>();
            //destinationSetter=GetComponent<AIDestinationSetter>();
            pointNumber=0;
            seeker=GetComponent<Seeker>();
            rb=GetComponent<Rigidbody2D>();
            Target=GameObject.FindGameObjectWithTag("Player").transform;
            isChasing=false;
            InvokeRepeating("Behave",0f,0.2f);
        }
        protected virtual void OnPathComplete(Path p)
        {
            if(!p.error)
            {
                path=p;
                currentWaypoint=0;
            }
            else
            {
                Debug.LogError(p.errorLog);
            }
        }

        protected virtual void FixedUpdate()
        {
            Move();
        }
        // Update is called once per frame
        protected virtual void Update()
        {
            //Vector2 force = Vector2.right * speed * Time.deltaTime;
            //rb.AddForce(force);
        }

        protected virtual void Move()
        {
            if(path==null)
            {
                return;
            }
            if(currentWaypoint>=path.vectorPath.Count)
            {
                reachedEndOfPath=true;
                return;
            }
            else
            {
                reachedEndOfPath=false;
            }
            Vector2 force = Vector2.zero;
            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            if(direction.y>0.3)
            {
                force = direction * speed * 3 * Time.deltaTime;
            }
            else
            {

                force = direction * speed * Time.deltaTime;
                //Debug.Log("Force: "+force);
                
            }
            rb.AddForce(force);

            float distance = Vector2.Distance(rb.position,path.vectorPath[currentWaypoint]);

            if(distance<nextWaypointDistance)
            {
                currentWaypoint++;
            }
            if(force.x>=0.01f)
            {
                transform.localScale=new Vector3(-1f, 1f, 1f);
            }
            else if(force.x<=-0.01f)
            {
                transform.localScale=new Vector3(1f, 1f, 1f);
            }
        }
        protected virtual void Jump()
        {
            Debug.Log("isJumping");
        }
    }
}
