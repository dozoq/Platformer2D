using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace platformer.interactive
{
    public class Console : MonoBehaviour
    {
        [Tooltip("GameObject(with IInteractionHandler) that this console activates")]
        [SerializeField] private GameObject triggerObject = null;

        IInteractionHandler interactiveObject = null;

        private void Start()
        {
            interactiveObject = triggerObject.GetComponent<IInteractionHandler>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if(interactiveObject != null)
                {
                    interactiveObject.HandleInteraction(this.gameObject);
                }
            }
        }
    }

}