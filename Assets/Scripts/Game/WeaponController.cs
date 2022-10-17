using System;
using Game.Interfaces;
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
        private WeaponSwitcher _weaponSwitcher;
        private IWeapon _currentWeapon;

        private void Start()
        {
            if (!photonView.IsMine) return;
            _weaponSwitcher = new WeaponSwitcher(weaponGameObjects);
            _currentWeapon = _weaponSwitcher.SwitchWeapon(WeaponIndex.Pistol);
            shootController.OnShootPress += OnShootPressHandler;
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