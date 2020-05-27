using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace platformer.combat
{
    [CreateAssetMenu(fileName = "WeaponConfig", menuName = "Weapons/Create New Weapon")]
    public class WeaponConfig : ScriptableObject
    {
        // PRIVATE
        [Tooltip("Animator override for this weapon")]
        [SerializeField] private AnimatorOverrideController animatorOverride = null; // Will be used later on;

        [Tooltip("Damage applied to enemy")]
        [SerializeField] private int weaponDamage = 1;

        [Tooltip("Damage applied to enemy")]
        [SerializeField] private bool weaponCanDamagePlayer = false;

        [Tooltip("How far will raycast be / how far bullet will fly")]
        [SerializeField] private float weaponRange = 1f; //Raycast range

        [Tooltip("(Height) of the radius that will be passed to raycast")]
        [SerializeField] private float weaponRadius = 0.5f;

        [Tooltip("(Time) which bullet will live")]
        [SerializeField] private float bulletLifespan = 6f;

        [Tooltip("How much delay between attacks")]
        [SerializeField] private float delayBetweenAttacks = 0.5f;

        [SerializeField] private float bulletSpeed = 5f;

        [Tooltip("Prefab to spawn at hand transform")]
        [SerializeField] private Weapon equippedPrefab;

        [Tooltip("If set, will use bullet instead of melee attacks")]
        [SerializeField] private Bullet bullet = null;

        /// <summary>
        /// Rename all instansiated weapons to Weapon so it`s easier to delete them
        /// </summary>
        private const string weaponName = "Weapon"; 

        // PUBLIC

        public Weapon Spawn(Transform handTransform, Animator animator)
        {
            DestroyOldWeapon(handTransform);
            Weapon weapon = null;

            if (equippedPrefab != null)
            {
                weapon = Instantiate(equippedPrefab, handTransform);
                weapon.gameObject.name = weaponName;
            }

            //TODO: add override for runtime animator controller

            return weapon;

        }

        public void LaunchBullet(Vector3 spawnTransform, Vector2 direction)
        {
            Bullet bulletInstance = Instantiate(bullet, spawnTransform, Quaternion.identity);
            bulletInstance.ConfigBullet(weaponDamage,bulletLifespan,bulletSpeed,weaponCanDamagePlayer);
            Vector2 spawnTransfromAsVector2 = new Vector2(spawnTransform.x, spawnTransform.y);
            bulletInstance.SetTarget(spawnTransfromAsVector2, direction);
        }

        private void DestroyOldWeapon(Transform handTransform)
        {
            Transform weaponToDestroy = handTransform.Find(weaponName);

            if (weaponToDestroy == null) return;

            weaponToDestroy.name = "DESTROY";
            Destroy(weaponToDestroy.gameObject);
        }

        public bool HasProjectile()
        {
            return bullet != null;
        }

        public int GetWeaponDamage()
        {
            return weaponDamage;
        }

        public float GetWeaponRange()
        {
            return weaponRange;
        }

        public float GetDelayBetweenAttacks()
        {
            return delayBetweenAttacks;
        }

        public float GetWeaponRadius()
        {
            return weaponRadius;
        }
    }
}
