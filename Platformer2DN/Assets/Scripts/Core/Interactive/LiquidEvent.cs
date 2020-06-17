using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace platformer.interactive.liquid
{
    //Class used to handle dynamic methods in inspector
    [System.Serializable]
    public class LiquidEvent : UnityEvent<GameObject>
    {
    }
}

