using System;
using System.Collections;
using System.Collections.Generic;
using ComponentScripts;
using Game.PlayerControllers;
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
        [SerializeField] private PlayerSpawn playerSpawn;
        private Transform[] _enemySpawnPositions;
        private readonly List<Transform> _playerTransforms = new List<Transform>();
        private readonly List<Enemy> _enemies = new List<Enemy>();

        private void Awake()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            playerSpawn.OnPlayerInstantiated += OnAddPlayerHandler;
        }

        private void Start()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            _enemySpawnPositions = enemySpawnComponent.GetSpawnPositions;
            StartCoroutine(SpawnEnemy());
        }

        private void OnAddPlayerHandler()
        {
            _playerTransforms.Clear();
            var playersList = PhotonNetwork.PlayerList;
            foreach (var player in playersList)
            {
                var playerObject = (GameObject)player.TagObject;
                _playerTransforms.Add(playerObject.GetComponent<Transform>());
            }
            
            // foreach (var enemy in _enemies)
            // {
            //     enemy.SetPlayersTransform = _playerTransforms;
            // }
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
            var enemy = PhotonNetwork.Instantiate(enemyPrefabs[randomEnemyType].name, _enemySpawnPositions[randomSpawnPosition].position, Quaternion.identity).GetComponent<Enemy>();
            enemy.SetPlayersTransform = _playerTransforms;
            //_enemies.Add(enemy);
            StartCoroutine(SpawnEnemy());
        }
    }
}