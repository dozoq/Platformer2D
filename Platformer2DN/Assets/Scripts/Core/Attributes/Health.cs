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
        public bool isDead;

        private void Awake()
        {
            health = maxHealth;
        }

        public void TakeDamage(int damage)
        {
            health-=damage;
            if (health<=0)
            {
                IDieable dieObject = this.GetComponent<IDieable>();
                if (dieObject!=null)
                {
                    dieObject.die();
                    isDead=true;
                }
            }
        }

    }
}
