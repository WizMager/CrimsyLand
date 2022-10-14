using System.Collections;
using System.Collections.Generic;
using ComponentScripts;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public class EnemySpawnController : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject[] enemyPrefabs;
        [SerializeField] private EnemySpawnComponent enemySpawnComponent;
        [SerializeField] private float spawnCooldown;
        private Transform[] _enemySpawnPositions;
        private readonly List<Transform> _playerTransforms = new List<Transform>();
        private readonly List<EnemyController> _enemies = new List<EnemyController>();

        private void Start()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            var masterPlayer = FindObjectOfType<PlayerController>();
            _playerTransforms.Add(masterPlayer.transform);
            _enemySpawnPositions = enemySpawnComponent.GetSpawnPositions;
            StartCoroutine(SpawnEnemy());
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (!PhotonNetwork.IsMasterClient) return;
            base.OnPlayerEnteredRoom(newPlayer);
            var players = FindObjectsOfType<PlayerController>();
            _playerTransforms.Clear();
            foreach (var player in players)
            {
                _playerTransforms.Add(player.transform);
            }

            foreach (var enemy in _enemies)
            {
                enemy.SetPlayersTransform = _playerTransforms;
            }
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
            var randomEnemyType = Random.Range(0, enemyPrefabs.Length);
            var enemy = PhotonNetwork.Instantiate($"Enemies/{enemyPrefabs[randomEnemyType].name}", _enemySpawnPositions[randomSpawnPosition].position, Quaternion.identity).GetComponent<EnemyController>();
            enemy.SetPlayersTransform = _playerTransforms;
            _enemies.Add(enemy);
            StartCoroutine(SpawnEnemy());
        }
    }
}