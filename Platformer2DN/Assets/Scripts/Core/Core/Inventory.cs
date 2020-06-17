using platformer.combat;
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
            // DEBUG. LATER INVENTORY WILL BE UPDATED FROM HUB
            if (Input.GetKeyDown(KeyCode.M))
            {
                WeaponConfig[] weapons = Resources.LoadAll<WeaponConfig>("");
                SwapWeaponAtSlot(1, weapons[1]);
                /*weaponSlotOne = weapons[1];

                weaponSlots[0] = weaponSlotOne;
                inventoryUpdated?.Invoke(1, weaponSlotOne); //Change at weaponSlot1*/
            }
        }


        /// <summary>
        /// Assign new weapon config(weapon) at given slot
        /// </summary>
        /// <param name="slot">1-3 weapon slot</param>
        /// <param name="newWeapon">new weapon config to be assigned at given slot</param>
        public void SwapWeaponAtSlot(int slot, WeaponConfig newWeapon)
        {
            if (weaponSlots[slot - 1] == newWeapon)
            {
                print("assigning same weapon to given slot. Not possible");
                return;
            }


            switch(slot)
            {
                case 1:
                    weaponSlotOne = newWeapon; // Assign new weaponconfig
                    weaponSlots[0] = weaponSlotOne; // Update array
                    inventoryUpdated?.Invoke(1, weaponSlotOne); //Change at weaponSlot1
                    break;
                case 2:
                    weaponSlotTwo = newWeapon;
                    weaponSlots[1] = weaponSlotTwo;
                    inventoryUpdated?.Invoke(2, weaponSlotTwo);
                    break;
                case 3:
                    weaponSlotThree = newWeapon;
                    weaponSlots[2] = weaponSlotThree;
                    inventoryUpdated?.Invoke(3, weaponSlotThree);
                    break;
            }
        }

        /// <summary>
        /// Get WeaponConfig from slot from Inventory
        /// </summary>
        /// <param name="slot">0-2(0 = 1st slot, 1 = 2nd slot, 2 = 3rd slot)</param>
        /// <returns>WeaponConfig at given slot</returns>
        public WeaponConfig GetWeaponConfig(int slot)
        {
            if(weaponSlots[slot] == null)
            {
                return null;
            }
            return weaponSlots[slot];
        }


    }

}