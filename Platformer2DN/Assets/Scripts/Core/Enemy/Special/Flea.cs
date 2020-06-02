using platformer.enemy;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace platformer.enemy.special
{
    public class Flea : BaseEnemy
    {
        protected override void specialAttack()
        {
            base.specialAttack();
            print("Own attack");
        }
        protected override void Start()
        {
            base.Start();
            this.haveOwnAttack=true;
        }
    }
}