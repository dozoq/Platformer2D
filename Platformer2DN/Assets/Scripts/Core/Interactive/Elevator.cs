using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace platformer.interactive
{
    public class Elevator : MonoBehaviour, IInteractionHandler
    {
        public void HandleInteraction(GameObject who)
        {
            Debug.LogWarning("Elevator activated by " + who);
        }
    }

}