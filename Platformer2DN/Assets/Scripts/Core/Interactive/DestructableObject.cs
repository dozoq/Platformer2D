using platformer.attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace platformer.interactive
{

    public class DestructableObject : MonoBehaviour, IDieable
    {
        public void Die()
        {
            Destroy(gameObject);
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