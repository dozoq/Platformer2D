using System.Collections;
using System.Collections.Generic;
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
    }
}
