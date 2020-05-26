using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace platformer.combat
{
    [CreateAssetMenu(fileName = "WeaponConfig", menuName = "Weapons/Create New Weapon")]
    public class WeaponConfig : ScriptableObject
    {
        // PRIVATE
        [SerializeField] private float weaponDamage = 1f;
        [SerializeField] private float weaponRange = 1f; //Raycast range
        [SerializeField] private Bullet bullet = null;

        /// <summary>
        /// Rename all instansiated weapons to Weapon so it`s easier to delete them
        /// </summary>
        private const string weaponName = "Weapon"; 
    }
}
