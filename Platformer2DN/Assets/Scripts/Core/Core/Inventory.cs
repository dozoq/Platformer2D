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

        private void Awake()
        {
            weaponSlots = new WeaponConfig[3] { weaponSlotOne, weaponSlotTwo, weaponSlotThree };
        }

        public WeaponConfig GetWeaponConfig(int slot)
        {
            return weaponSlots[slot];
        }


    }

}