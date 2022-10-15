using System;
using ComponentScripts;
using Game.Interfaces;
using Game.Weapons;
using UnityEngine;

namespace Game
{
    public class WeaponSwitcher
    {
        private readonly GameObject[] _weapons;

        public WeaponSwitcher(GameObject[] weapons)
        {
            _weapons = weapons;
        }

        public IWeapon SwitchWeapon(WeaponIndex weaponIndex)
        {
            foreach (var weapon in _weapons)
            {
                weapon.SetActive(false);
            }
            var weaponComponent = _weapons[(int) weaponIndex].GetComponent<WeaponComponent>();
            _weapons[(int)weaponIndex].SetActive(true);
            
            switch (weaponIndex)
            {
                case WeaponIndex.Pistol:
                    return new Pistol(weaponComponent);
                case WeaponIndex.Rifle:
                    return new Rifle(weaponComponent);
                default:
                    throw new ArgumentOutOfRangeException(nameof(weaponIndex), weaponIndex, null);
            }
        }
    }
}