using platformer.enemy;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace platformer.enemy.special
{
    public class Flea : BaseEnemy
    {
        public bool isCharging;
        protected override void specialAttack()
        {
            base.specialAttack();
            print("Own attack");
            isCharging=true;
            this.fireTimer+=FireRate;
        }
        protected override void Start()
        {
            base.Start();
            this.haveOwnAttack=true;
        }

        protected override void Update()
        {
            if(isCharging)
            {
                stop=true;
            }
            else
            {
                stop=false;
            }
            base.Update();
            
        }
    }
}