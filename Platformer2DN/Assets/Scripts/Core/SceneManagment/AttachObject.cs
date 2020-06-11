using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace platformer.scenemanagment
{
    public class AttachObject : MonoBehaviour
    {
        [SerializeField] private bool attachPlayerOnly = true;


        private Vector3 previousTransform;
        private bool isPreviousTransformSet = false;

        private void OnCollisionStay2D(Collision2D collision)
        {
            if(attachPlayerOnly && !collision.gameObject.CompareTag("Player"))
            {
                Debug.LogWarning("Object not a player");
                return;
            }

            if (isPreviousTransformSet && Mathf.Abs(previousTransform.x - this.transform.position.x) > 0)
            {
                float difference = previousTransform.x - this.transform.position.x;
                float x = collision.transform.position.x - difference;
                collision.transform.position = new Vector2(x, collision.transform.position.y);
            }

            previousTransform = this.transform.position;
            isPreviousTransformSet = true;
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            isPreviousTransformSet = false;
            print("Leaved elevator");
        }

    }

}