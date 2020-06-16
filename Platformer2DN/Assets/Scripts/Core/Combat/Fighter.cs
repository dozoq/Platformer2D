using System;
using System.Collections;
using System.Collections.Generic;
using platformer.attributes;
using platformer.core;
using platformer.ui;
using UnityEngine;

namespace platformer.combat
{
    public class Fighter : MonoBehaviour
    {
        public Animator animator = null;
        public Transform handTransform = null;

        private Weapon currentWeapon; // Current weapon prefab
        private WeaponConfig currentWeaponConfig;

        private Inventory inventory = null;

        public event Action<int> inventoryActiveWeaponChanged;

        private int currentlyUsedSlot = 1; //Default 


        private void Awake()
        {
            inventory = GetComponent<Inventory>();
            inventory.inventoryUpdated += UpdateCurrentWeapon;
        }

        private void Start()
        {
            currentWeaponConfig = inventory.GetWeaponConfig(0);
            currentWeapon = SetupDefaultWeapon(currentWeaponConfig);
        }

        /// <summary>
        /// Initialize weapon from defaultWeaponConfig attached to a player
        /// </summary>
        /// <param name="defaultWeaponConfig"></param>
        /// <returns>Weapon prefab set scriptableObject</returns>
        private Weapon SetupDefaultWeapon(WeaponConfig defaultWeaponConfig)
        {
            inventoryActiveWeaponChanged?.Invoke(1);
            return defaultWeaponConfig.Spawn(handTransform, animator);
        }

        /// <summary>
        /// Spawn new prefab. Change weapon config in inventory at given slot
        /// </summary>
        /// <param name="newWeapon">New WeaponConfig to replace previous one</param>
        /// <param name="slotNumber">Inventory slot number (1-3) </param>
        public void ChangeWeapon(WeaponConfig newWeapon, int slotNumber)
        {
            //weaponConfig = newWeapon;
            //weaponConfig.Spawn(handTransform, animator);

            currentWeapon = newWeapon.Spawn(handTransform, animator);

            currentlyUsedSlot = slotNumber;
            inventoryActiveWeaponChanged?.Invoke(slotNumber);

            //inventoryUpdated?.Invoke(); // Update UI
        }

        /// <summary>
        /// Called from inventory when "swaping" with new weapon on same slot.
        /// If currently used slot is different than changed slot, no need to update
        /// current weapon as it will change when switching weapon.
        /// </summary>
        /// <param name="slotNumber">Slot that was changed(1-3, 1=1st, 2=2nd, 3=3rd)</param>
        /// <param name="newWeaponConfig">New WeaponConfig at given slot</param>
        private void UpdateCurrentWeapon(int slotNumber, WeaponConfig newWeaponConfig)
        {
            if (currentlyUsedSlot == slotNumber)
            {
                ChangeWeapon(newWeaponConfig, slotNumber);
            }
        }

        /// <summary>
        /// Instatiating a bullet from a 
        /// </summary>
        /// <param name="worldPoint">Coordinates of mouse position in the world space</param>
        public void Shoot(Vector3 worldPoint)
        {
            var vectorPostion = new Vector2(worldPoint.x, worldPoint.y);
            if(currentWeaponConfig.HasProjectile())
            {
                //TODO: Change handtransform into barrel transform of weapon
                Vector3 weaponBarrelTransform = new Vector3(handTransform.position.x, handTransform.position.y, 0);
                currentWeaponConfig.LaunchBullet(weaponBarrelTransform, vectorPostion);

            }
        }

    }
}
