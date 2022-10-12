using System.Collections;
using ComponentScripts;
using Photon.Pun;
using UnityEngine;

namespace Game
{
    public class EnemySpawnController : MonoBehaviour
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private EnemySpawnComponent enemySpawnComponent;
        [SerializeField] private float spawnCooldown;
        private Transform[] _enemySpawnPositions;

        private void Start()
        {
            _enemySpawnPositions = enemySpawnComponent.GetSpawnPositions;
            StartCoroutine(SpawnEnemy());
        }

        private IEnumerator SpawnEnemy()
        {
            for (float i = 0; i < spawnCooldown; i += Time.deltaTime)
            {
                yield return null;
            }
            InstantiateEnemy();
        }

        private void InstantiateEnemy()
        {
            var randomSpawnPosition = Random.Range(0, _enemySpawnPositions.Length);
            PhotonNetwork.Instantiate(enemyPrefab.name, _enemySpawnPositions[randomSpawnPosition].position, Quaternion.identity);
            StartCoroutine(SpawnEnemy());
        }
    }
}