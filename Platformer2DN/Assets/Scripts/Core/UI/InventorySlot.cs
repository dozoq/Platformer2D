using platformer.combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace platformer.ui
{
    public class InventorySlot : MonoBehaviour
    {
        private Fighter fighter = null;

        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
           // fighter.inventoryUpdated += Redraw;
        }

        private void OnDisable()
        {
           // fighter.inventoryUpdated -= Redraw;
        }

        private void Redraw()
        {
            print("Updating UI");
        }
    }

}