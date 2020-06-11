using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace platformer.scenemanagment
{
    /// <summary>
    /// Use this script whenever you want to attach "dynamic" object to 
    /// another object that is moving horizontally
    /// </summary>
    public class AttachObject : MonoBehaviour
    {
        [SerializeField] private bool attachPlayerOnly = true;


        private Vector3 previousTransform;
        private bool isPreviousTransformSet = false;

        /// <summary>
        /// Activates whenever object with collider/rigidbody is on object of this component
        /// </summary>
        private void OnCollisionStay2D(Collision2D collision)
        {
            // If can move only player, and the the collider is not a player, return
            if(attachPlayerOnly && !collision.gameObject.CompareTag("Player"))
            {
                Debug.LogWarning("Object not a player");
                return;
            }

            // Check if transform changed horizontally
            if (isPreviousTransformSet && Mathf.Abs(previousTransform.x - this.transform.position.x) > 0)
            {
                float difference = previousTransform.x - this.transform.position.x;
                float x = collision.transform.position.x - difference;
                collision.transform.position = new Vector2(x, collision.transform.position.y);
            }

            previousTransform = this.transform.position;
            isPreviousTransformSet = true;
        }

        /// <summary>
        /// Left the collider
        /// </summary>
        private void OnCollisionExit2D(Collision2D collision)
        {
            // If can move only player, and the the collider is not a player, return
            if (attachPlayerOnly && !collision.gameObject.CompareTag("Player"))
            {
                Debug.LogWarning("Object not a player");
                return;
            }

            isPreviousTransformSet = false;
            print("Leaved elevator");
        }

    }

}