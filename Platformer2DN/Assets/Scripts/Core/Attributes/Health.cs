using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace platformer.attributes
{
    public class Health : MonoBehaviour
    {
        public int maxHealth;
        private int health;
        [HideInInspector]
        public bool isDead;

        private void Awake()
        {
            health = maxHealth;
        }

        public void TakeDamage(int damage)
        {
            if (isDead) return;

            print("Health before attack: " + health);
            health -=damage;
            if (health<=0)
            {
                IDieable dieObject = this.GetComponent<IDieable>();
                if (dieObject!=null)
                {
                    dieObject.die();
                    isDead=true;
                }
            }

            print("Health afteer attack: " + health);
        }

    }
}
