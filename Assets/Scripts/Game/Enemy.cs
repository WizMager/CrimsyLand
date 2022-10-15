using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Game.Interfaces;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public class Enemy : MonoBehaviour, IEnemy
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private int health;
        [SerializeField] private float timeRecheckNearestPlayer;
        [SerializeField] private float damage;
        [SerializeField] private int bonusDropChance;
        private List<Transform> _playersTransform = new List<Transform>();
        private Transform _currentTarget;
        
        public float Health { get; private set; }
        public float MoveSpeed => moveSpeed;
        public List<Transform> SetPlayersTransform
        {
            set
            {
                _playersTransform.Clear();
                _playersTransform = value;
                _currentTarget = _playersTransform[CheckNearestPlayerIndex()];
            }
        }

        #region Monobeh

        private void Start()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            Health = health;
            StartCoroutine(RecheckNearestEnemyCooldown());
        }

        private void OnEnable()
        {
            PhotonNetwork.NetworkingClient.EventReceived += OnMessageReceive;
        }

        private void FixedUpdate()
        {
            if (!PhotonNetwork.IsMasterClient) return;
           Move(Time.fixedDeltaTime); 
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            var player = collision.gameObject.GetComponent<IPlayer>();
            player?.ChangeHealthSend(-damage * Time.deltaTime);
        }

        private void OnDisable()
        {
            PhotonNetwork.NetworkingClient.EventReceived -= OnMessageReceive;
        }

        private void OnDestroy()
        {
            StopCoroutine(RecheckNearestEnemyCooldown());
        }

        #endregion

        #region CheckNearestPlayer

        private IEnumerator RecheckNearestEnemyCooldown()
        {
            for (float i = 0; i < timeRecheckNearestPlayer; i += Time.deltaTime)
            {
                yield return null;
            }
            
            RecheckNearestPlayer();
        }
        
        private int CheckNearestPlayerIndex()
        {
            var distance = 100f;
            var index = 0;
            for (int i = 0; i < _playersTransform.Count; i++)
            {
                var currentDistance = Vector3.Distance(transform.position, _playersTransform[i].position);
                if (currentDistance >= distance) continue;
                distance = currentDistance;
                index = i;
            }
            return index;
        }

        private void RecheckNearestPlayer()
        {
            _currentTarget = _playersTransform[CheckNearestPlayerIndex()];
            StartCoroutine(RecheckNearestEnemyCooldown());
        }

        #endregion

        private void OnMessageReceive(EventData data)
        {
            switch (data.Code)
            {
                case EventCodePhoton.ChangeEnemyHealthEvent:
                    ChangeHealthReceive((float)data.CustomData);
                    break;
            }
        }
        
        public void ChangeHealthSend(float value)
        {
            PhotonNetwork.RaiseEvent(EventCodePhoton.ChangeEnemyHealthEvent, value,
                new RaiseEventOptions {Receivers = ReceiverGroup.MasterClient}, SendOptions.SendReliable);
        }

        private void ChangeHealthReceive(float value)
        {
            Health += value;
            if (Health > 0) return;
            if (Random.Range(0, 101) <= bonusDropChance)
            {
                PhotonNetwork.Instantiate("Bonus", transform.position, Quaternion.identity); 
            } 
            PhotonNetwork.Destroy(gameObject);
        }
        
        public void Move(float deltaTime)
        {
            transform.position =
                Vector3.MoveTowards(transform.position, _currentTarget.position, moveSpeed * deltaTime);
        }
    }
}