using platformer.attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace platformer.interactive.liquid
{
    public class Liquid : MonoBehaviour
    {
        //Function called on collider enter
        public LiquidEvent EnterFunctionToCall;
        //function called on collider exit
        public LiquidEvent ExitFunctionToCall;


        private void OnTriggerEnter2D(Collider2D collision)
        {
            EnterFunctionToCall.Invoke(collision.gameObject);
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            ExitFunctionToCall.Invoke(collision.gameObject);
        }
    }
}