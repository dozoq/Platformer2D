using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace platformer.interactive
{
    public class Doors : MonoBehaviour, IInteractionHandler
    {
        public void HandleInteraction(GameObject who)
        {
            gameObject.transform.Translate(new Vector3(0, -70, 0));
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}