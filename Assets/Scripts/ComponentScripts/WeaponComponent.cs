using UnityEngine;

namespace ComponentScripts
{
    public class WeaponComponent : MonoBehaviour
    {
        [SerializeField] private Transform shootPosition;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private float shootCooldown;
        
        public Transform GetShootPosition => shootPosition;
        public GameObject GetBulletPrefab => bulletPrefab;
        public float GetShootCooldown => shootCooldown;
    }
}