
using ComponentScripts;
using UnityEngine;

namespace Game.Interfaces
{
    public interface IWeapon
    {
        public WeaponComponent WeaponComponent { get;}
        public GameObject BulletPrefab { get;}
        public Transform ShootPosition { get;}
        public bool ShootAvailable { get;}
        public float ShootCooldown { get;}

        public void Shoot();
    }
}