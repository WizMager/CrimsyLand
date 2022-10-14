using UnityEngine;

namespace ComponentScripts
{
    public class BulletComponent : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float flyDistance;
        [SerializeField] private float damage;

        public float GetSpeed => speed;
        public float GetFlyDistance => flyDistance;
        public float GetDamage => damage;
    }
}