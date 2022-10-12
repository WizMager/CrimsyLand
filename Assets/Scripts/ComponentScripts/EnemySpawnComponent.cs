using UnityEngine;

namespace ComponentScripts
{
    public class EnemySpawnComponent : MonoBehaviour
    {
        [SerializeField] private Transform[] spawnPositions;

        public Transform[] GetSpawnPositions => spawnPositions;
    }
}