using platformer.attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace platformer.interactive
{
    public class Acid : MonoBehaviour
    {
        public float FlowXForce = 0.01f;
        public float FlowYForce = 0.00f;
        private float resetXForce = 0.01f;
        private float resetYForce = 0.00f;


        Material material;
        
        // Start is called before the first frame update
        void Start()
        {
            resetXForce=FlowXForce;
            resetYForce=FlowYForce;
            material=GetComponent<SpriteRenderer>().material;
            material.SetVector("_FlowForce",new Vector4(FlowXForce,FlowYForce));
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            //Do Damage to health component
            Health temphp = collision.gameObject.GetComponent<Health>();
            if(temphp!=null)
            {
                print("take dmg");
                temphp.TakeDamage(1);
            }
            //Apply force from object to mat
            Rigidbody2D temprb = collision.attachedRigidbody;
            if(temprb!=null)
            {
                SetFlowForce(temprb.velocity);
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            SetFlowForce(new Vector2(resetXForce, resetYForce));
        }

        private void SetFlowForce(Vector2 f)
        {
            material.SetVector("_FlowForce", f);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}