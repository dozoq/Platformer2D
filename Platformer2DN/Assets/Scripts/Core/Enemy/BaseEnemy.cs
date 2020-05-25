using Pathfinding;
using platformer.attributes;
using System.Collections;
using System.Collections.Generic;
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
        private int pointnumber;
        public GameObject target;
        private bool isChasing;
        private AIDestinationSetter destinationSetter;
        private AIPath aipath;

        public void behave() 
        {
            if (target==null)
            {
                throw new System.Exception("Instantiate player first");
            }
            float tempX;
            float tempY;
            if (target.transform.position.x>this.gameObject.transform.position.x)
            {
                tempX=target.transform.position.x-this.gameObject.transform.position.x;
            }
            else
            {
                tempX=this.gameObject.transform.position.x-target.transform.position.x;
            }
            if (target.transform.position.x>this.gameObject.transform.position.x)
            {
                tempY=target.transform.position.y-this.gameObject.transform.position.y;
            }
            else
            {
                tempY=this.gameObject.transform.position.y-target.transform.position.y;
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
                destinationSetter.target=this.target.transform;
                //Sprawdź czy w zasięgu
                    //zaatakuj

            }
            else
            {
                if (aipath.reachedEndOfPath)
                {
                    if (pointnumber<(patrolPaths.Length-1))
                    {
                        pointnumber++;
                    }
                    else
                    {
                        pointnumber=0;
                    }
                }
                destinationSetter.target=patrolPaths[ pointnumber ].transform;
            }

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
            target=GameObject.FindGameObjectWithTag( "Player" );
            pointnumber=0;
            isChasing=false;
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            this.behave();
        }

    }
}
