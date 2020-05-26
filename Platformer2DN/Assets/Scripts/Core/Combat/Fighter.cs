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

        public void Attack(Ray ray)
        {
            var colliders = GetAllCollidersInAttackRange(ray);
            if (colliders == null) return; // Didn`t hit anything

            foreach (var collider in colliders)
            {
                if (collider.transform.gameObject == this.gameObject)
                {
                    print("Hit player, continue");
                    continue;
                }

                float distanceToCollider = Vector2.Distance(ToVector2(this.transform.position),
                    ToVector2(collider.transform.position));

                if (distanceToCollider > currentWeaponConfig.GetWeaponRange())
                {
                    continue;
                }

                var healthComponent = collider.transform.GetComponent<Health>();

                if (healthComponent != null)
                {
                    healthComponent.TakeDamage(currentWeaponConfig.GetWeaponDamage());
                }


            }
        }

        private RaycastHit2D[] GetAllCollidersInAttackRange(Ray ray)
        {
            Vector2 currentPostionVector2 = new Vector2(transform.position.x, transform.position.y);
            Vector2 direction = new Vector2(ray.origin.x, ray.origin.y);
            float weaponRadius = currentWeaponConfig.GetWeaponRadius();
            return Physics2D.CircleCastAll(currentPostionVector2, weaponRadius, direction);
        }


        //Helper function
        private Vector2 ToVector2(Vector3 vector3)
        {
            return new Vector2(vector3.x, vector3.y);
        }
    }
}
