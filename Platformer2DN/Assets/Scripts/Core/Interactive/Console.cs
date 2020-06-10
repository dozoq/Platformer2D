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
        [Tooltip("Delay between another use in seconds")]
        [SerializeField] private float delayBetweenUsages = 1f;

        IInteractionHandler interactiveObject = null;
        private bool canActivateObject = false; // True if is in the box collider of console
        private float delayTimer = Mathf.Infinity;


        private void Start()
        {
            interactiveObject = triggerObject.GetComponent<IInteractionHandler>();
        }

        private void Update()
        {
            if (!isConsoleActive) return; // If console is disabled, don`t do anything

            delayTimer += Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.E) && canActivateObject && delayBetweenUsages <= delayTimer)
            {
                if(interactiveObject != null)
                {
                    interactiveObject.HandleInteraction(this.gameObject);
                    delayTimer = 0f;
                    print("used and reseted timer");
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