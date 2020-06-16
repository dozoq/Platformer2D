﻿using platformer.combat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace platformer.core
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private WeaponConfig weaponSlotOne = null;
        [SerializeField] private WeaponConfig weaponSlotTwo = null;
        [SerializeField] private WeaponConfig weaponSlotThree = null;

        private WeaponConfig[] weaponSlots;

        public event Action<int, WeaponConfig> inventoryUpdated;

        private void Awake()
        {
            weaponSlots = new WeaponConfig[3] { weaponSlotOne, weaponSlotTwo, weaponSlotThree };
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                WeaponConfig[] weapons = Resources.LoadAll<WeaponConfig>("");
                weaponSlotOne = weapons[0];

                weaponSlots[0] = weaponSlotOne;
                inventoryUpdated?.Invoke(1, weaponSlotOne); //Change at weaponSlot1
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slot">0-2 (0 = 1st slot, 1 = 2nd slot, 2 = 3rd slot)</param>
        /// <returns></returns>
        public WeaponConfig GetWeaponConfig(int slot)
        {
            return weaponSlots[slot];
        }



    }

}