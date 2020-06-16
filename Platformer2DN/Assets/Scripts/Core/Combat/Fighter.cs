﻿using System;
using System.Collections;
using System.Collections.Generic;
using platformer.attributes;
using platformer.core;
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

        private int currentlyUsedSlot = 1; //Default 


        private void UpdateCurrentWeapon(int slotNumber, WeaponConfig newWeaponConfig)
        {
            if(currentlyUsedSlot == slotNumber)
            {
                ChangeWeapon(newWeaponConfig, slotNumber);
            }
        }



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
            return defaultWeaponConfig.Spawn(handTransform, animator);
        }

        public void ChangeWeapon(WeaponConfig newWeapon, int slotNumber)
        {
            //weaponConfig = newWeapon;
            //weaponConfig.Spawn(handTransform, animator);

            newWeapon.Spawn(handTransform, animator);

            currentlyUsedSlot = slotNumber;

            //inventoryUpdated?.Invoke(); // Update UI
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
