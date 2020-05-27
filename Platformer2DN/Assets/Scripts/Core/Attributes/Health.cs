using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace platformer.attributes
{
    public class Health : MonoBehaviour
    {
        [Tooltip("How many health object have")]
        public int maxHealth;
        //How many helth left in object
        private int health;
        [HideInInspector]
        public bool isDead;

        private void Awake()
        {
            //set health to full on start
            health = maxHealth;
        }
        //Handles dealing damage by bullets etc.
        public void TakeDamage(int damage)
        {
            if (isDead) return;

            //Debug info
            print("Health before attack: " + health);
            //Remove health from object based on attack
            health -=damage;
            //If there is no health left
            if (health<=0)
            {
                //Take dieable interface from object
                IDieable dieObject = this.GetComponent<IDieable>();
                //if it's not null
                if (dieObject!=null)
                {
                    //call die funtion from object
                    dieObject.Die();
                    //set dead
                    isDead=true;
                }
            }
            //Debug infor
            print("Health afteer attack: " + health);
        }

    }
}
