using platformer.attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace platformer.interactive.liquid
{
    public class Liquid : MonoBehaviour
    {
        public float FlowXForce = 0.01f;
        public float FlowYForce = 0.00f;
        private float resetXForce = 0.01f;
        private float resetYForce = 0.00f;

        public LiquidEvent EnterFunctionToCall;
        public LiquidEvent ExitFunctionToCall;


        Material material;

        // Start is called before the first frame update
        void Start()
        {
            resetXForce=FlowXForce;
            resetYForce=FlowYForce;
            material=GetComponent<SpriteRenderer>().material;
            material.SetVector("_FlowForce", new Vector4(FlowXForce, FlowYForce));
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            EnterFunctionToCall.Invoke(collision.gameObject);
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            ExitFunctionToCall.Invoke(collision.gameObject);
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