using platformer.combat;
using platformer.core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace platformer.ui
{
    public class InventorySlot : MonoBehaviour
    {
        [Range(1,3)]
        [SerializeField] private int inventorySlotNumber;
        [Tooltip("Icon to change attached to child of this - Gun")]
        [SerializeField] private Image gunIcon = null;
        [SerializeField] private CanvasGroup backgroundCanvasGroup = null;

        private Fighter fighter = null;
        private Inventory inventory = null;

        
        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
            inventory = GameObject.FindWithTag("Player").GetComponent<Inventory>();

            inventory.inventoryUpdated += Redraw;
            fighter.inventoryActiveWeaponChanged += SetActiveWeapon;
        }

        private void OnDisable()
        {
           // fighter.inventoryUpdated -= Redraw;
        }

        /// <summary>
        /// Make current used weapon in UI background more visible
        /// </summary>
        /// <param name="slot">To highlight</param>
        public void SetActiveWeapon(int slot)
        {
            if(slot == inventorySlotNumber)
            {
                backgroundCanvasGroup.alpha = 0.2f;
            }
            else
            {
                backgroundCanvasGroup.alpha = 0.1f;
            }
        }

        /// <summary>
        /// Change image based on weapon config. Called from inventory after assigning new weapon.
        /// </summary>
        /// <param name="slotNumber">Slot to redraw</param>
        /// <param name="newWeaponConfig">Used to get sprite icon</param>
        private void Redraw(int slotNumber, WeaponConfig newWeaponConfig)
        {
            if(slotNumber == inventorySlotNumber)
            {
                gunIcon.sprite = newWeaponConfig.GetWeaponSprite();
            }
        }
    }

}