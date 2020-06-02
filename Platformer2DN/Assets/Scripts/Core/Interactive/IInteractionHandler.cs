using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace platformer.interactive
{

    public interface IInteractionHandler
    {
        void HandleInteraction(GameObject who);
    }
}