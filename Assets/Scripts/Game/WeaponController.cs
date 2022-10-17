using System;
using Game.Interfaces;
using Game.Player;
using Game.PlayerControllers;
using Photon.Pun;
using UnityEngine;

namespace Game
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private GameObject[] weaponGameObjects;
        [SerializeField] private PhotonView photonView;
        [SerializeField] private PlayerShootController shootController;
        [SerializeField] private PlayerBonusController bonusController;
        private WeaponSwitcher _weaponSwitcher;
        private IWeapon _currentWeapon;

        private void Start()
        {
            if (!photonView.IsMine) return;
            _weaponSwitcher = new WeaponSwitcher(weaponGameObjects);
            _currentWeapon = _weaponSwitcher.SwitchWeapon(WeaponIndex.Pistol);
            shootController.OnShootPress += OnShootPressHandler;
            bonusController.OnWeaponChange += OnWeaponChangeHandler;
        }

        private void OnWeaponChangeHandler(int weaponIndex)
        {
            switch (weaponIndex)
            {
                case (int)WeaponIndex.Pistol:
                    _currentWeapon = _weaponSwitcher.SwitchWeapon(WeaponIndex.Pistol);
                    break;
                case (int)WeaponIndex.Rifle:
                    _currentWeapon = _weaponSwitcher.SwitchWeapon(WeaponIndex.Rifle);
                    break;
            }
        }

        private void OnShootPressHandler()
        {
            _currentWeapon.Shoot();
        }

        private void OnDestroy()
        {
            if (!photonView.IsMine) return;
            shootController.OnShootPress -= OnShootPressHandler;
        }
    }
}