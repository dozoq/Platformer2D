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

        public void TakeDamage(int damage)
        {
            health-=damage;
            if (health<=0)
            {
                IDieable dieObject = this.GetComponentInParent<IDieable>();
                if (dieObject!=null)
                {
                    dieObject.die();
                }
            }
        }

    }
}
