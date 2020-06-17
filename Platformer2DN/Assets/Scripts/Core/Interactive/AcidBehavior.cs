using platformer.attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace platformer.interactive.liquid
{
    public class AcidBehavior : MonoBehaviour
    {
        private List<GameObject> objectsInAcid;
        private void Awake()
        {
            objectsInAcid=new List<GameObject>();
            InvokeRepeating("dealDamageOverTime", 0f, 1f);
        }
        public void AcidEnter(GameObject collision)
        {
            objectsInAcid.Add(collision);
        }
        public void AcidExit(GameObject collision)
        {
            objectsInAcid.Remove(collision);
        }
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