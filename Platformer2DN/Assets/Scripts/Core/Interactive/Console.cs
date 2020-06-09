using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace platformer.interactive
{
    public class Console : MonoBehaviour
    {
        [Tooltip("GameObject(with IInteractionHandler) that this console activates")]
        [SerializeField] private GameObject triggerObject = null;
        [Tooltip("Can be used since start of the game? (Some consoles might be not enable since the beginning)")] 
        [SerializeField] private bool isConsoleActive = true;

        IInteractionHandler interactiveObject = null;
        private bool canActivateObject = false; // True if is in the box collider of console

        private void Start()
        {
            interactiveObject = triggerObject.GetComponent<IInteractionHandler>();
        }

        private void Update()
        {
            if (!isConsoleActive) return; // If console is disabled, don`t do anything


            if (Input.GetKeyDown(KeyCode.E) && canActivateObject)
            {
                if(interactiveObject != null)
                {
                    interactiveObject.HandleInteraction(this.gameObject);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                canActivateObject = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if(collision.CompareTag("Player"))
            {
                canActivateObject = false;
            }
        }
    }

}