using UnityEngine;

namespace ComponentScripts
{
    public class WeaponContainerComponent : MonoBehaviour
    {
        [SerializeField] private GameObject[] weapons;

        public GameObject[] GetWeaponsGameObjects => weapons;
    }
}