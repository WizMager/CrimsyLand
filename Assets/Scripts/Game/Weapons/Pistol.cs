using System.Collections;
using ComponentScripts;
using Game.Interfaces;
using Photon.Pun;
using UnityEngine;

namespace Game.Weapons
{
    public class Pistol : IWeapon
    {
        public WeaponComponent WeaponComponent { get; }
        public GameObject BulletPrefab { get;}
        public Transform ShootPosition { get;}
        public bool ShootAvailable { get; private set; }
        public float ShootCooldown { get; }

        public Pistol(WeaponComponent weaponComponent)
        {
            WeaponComponent = weaponComponent;
            BulletPrefab = weaponComponent.GetBulletPrefab;
            ShootPosition = weaponComponent.GetShootPosition;
            ShootCooldown = weaponComponent.GetShootCooldown;
            ShootAvailable = true;
        }
        
        public void Shoot()
        {
            if (!ShootAvailable) return;
            PhotonNetwork.Instantiate(BulletPrefab.name, ShootPosition.position, ShootPosition.rotation);
            ShootAvailable = false;
            WeaponComponent.StartCoroutine(ShootCooldownTimer());
        }
        
        private IEnumerator ShootCooldownTimer()
        {
            for (float i = 0; i < ShootCooldown; i += Time.deltaTime)
            {
                yield return null;
            }
            ShootAvailable = true;
        }
    }
}