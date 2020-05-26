using Pathfinding;
using platformer.attributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;

namespace platformer.enemy
{
    public class BaseEnemy : MonoBehaviour, IDieable
    { 
        public float maxDetectionRange;
        public GameObject[] patrolPaths;
        public GameObject Target;
        public bool isFlying = false;

        private int pointNumber;
        private bool isChasing;
        private AIDestinationSetter destinationSetter;
        private AIPath aipath;

        public void behave() 
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
                destinationSetter.target=this.Target.transform;
                //Sprawdź czy w zasięgu
                    //zaatakuj

            }
            else
            {
                if (aipath.reachedEndOfPath)
                {
                    if (pointNumber<(patrolPaths.Length-1))
                    {
                        pointNumber++;
                    }
                    else
                    {
                        pointNumber=0;
                    }
                }
                destinationSetter.target=patrolPaths[ pointNumber ].transform;
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

        public void die()
        {
            Destroy( this.gameObject );
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            aipath=GetComponent<AIPath>();
            destinationSetter=GetComponent<AIDestinationSetter>();
            Target=GameObject.FindGameObjectWithTag( "Player" );
            pointNumber=0;
            isChasing=false;
            
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            behave();
        }

    }
}
