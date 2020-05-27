using System.Collections;
using System.Collections.Generic;
using platformer.attributes;
using UnityEngine;

namespace platformer.combat
{
    public class Fighter : MonoBehaviour
    {
        public Animator animator = null;
        public WeaponConfig weaponConfig = null;
        public Transform handTransform = null;

        private Weapon currentWeapon;
        private WeaponConfig currentWeaponConfig;

        private void Awake()
        {
            currentWeaponConfig = weaponConfig;
            currentWeapon = SetupDefaultWeapon(currentWeaponConfig);
        }

        private Weapon SetupDefaultWeapon(WeaponConfig defaultWeaponConfig)
        {
            return defaultWeaponConfig.Spawn(handTransform, animator);
        }

        public void Shoot(Vector3 worldPoint)
        {
            var vectorPostion = new Vector2(worldPoint.x, worldPoint.y);
            if(currentWeaponConfig.HasProjectile())
            {
                Vector3 weaponBarrelTransform = new Vector3(handTransform.position.x + 2, handTransform.position.y, 0);
                currentWeaponConfig.LaunchBullet(weaponBarrelTransform, vectorPostion, currentWeaponConfig.GetWeaponDamage());

            }
        }


        //Helper function
        private Vector2 ToVector2(Vector3 vector3)
        {
            return new Vector2(vector3.x, vector3.y);
        }
    }
}
