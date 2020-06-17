using platformer.attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace platformer.interactive.liquid
{
    public class AcidBehavior : MonoBehaviour
    {
        //List of object lying in liquid
        private List<GameObject> objectsInAcid;
        private void Awake()
        {
            objectsInAcid=new List<GameObject>();
            //Repeat function every second
            InvokeRepeating("dealDamageOverTime", 0f, 1f);
        }
        //Liquid call this function on Trigger enter
        public void AcidEnter(GameObject collision)
        {
            objectsInAcid.Add(collision);
        }
        //Liquid call this function on Trigger exit
        public void AcidExit(GameObject collision)
        {
            objectsInAcid.Remove(collision);
        }
        //Implementation of damage over time
        private void dealDamageOverTime()
        {
            foreach(GameObject collision in objectsInAcid)
            {
                Health temphp = collision.GetComponent<Health>();
                if(temphp!=null)
                {
                    temphp.TakeDamage(1);
                }
            }
        }
    }
}