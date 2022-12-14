using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Game.Interfaces;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Game
{
    public class Enemy : MonoBehaviour, IEnemy, IOnEventCallback
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private int health;
        [SerializeField] private float timeRecheckNearestPlayer;
        [SerializeField] private float damage;
        [SerializeField] private int bonusDropChance;
        [SerializeField] private PhotonView photonView;
        [SerializeField] private GameObject bonusPrefab;
        private List<Transform> _playersTransform = new List<Transform>();
        private Transform _currentTarget;
        
        public PhotonView PhotonView { get; private set; }
        public float Health { get; private set; }
        public float MoveSpeed => moveSpeed;
        public List<Transform> SetPlayersTransform
        {
            set
            {
                _playersTransform = value;
                _currentTarget = _playersTransform[CheckNearestPlayerIndex()];
            }
        }

        #region Monobeh

        private void Start()
        {
            PhotonView = photonView;
            if (!PhotonNetwork.IsMasterClient) return;
            Health = health;
            StartCoroutine(RecheckNearestEnemyCooldown());
        }

        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        private void FixedUpdate()
        { 
            if (!PhotonNetwork.IsMasterClient) return; 
            Move(Time.fixedDeltaTime); 
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            var player = collision.gameObject.GetComponent<IPlayerHealth>();
            player?.ChangeHealth(-damage * Time.fixedDeltaTime);
        }

        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
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

        public void ChangeHealth(float value, int id)
        {
            var dictionary = new Dictionary<int, float> {{id, value}};
            PhotonNetwork.RaiseEvent(EventCodePhoton.EnemyReceiveDamage, dictionary, new RaiseEventOptions {Receivers = ReceiverGroup.MasterClient},
                SendOptions.SendReliable);
        }

        private void ChangeHealthValue(Dictionary<int, float> idDamage)
        {
            var id = 0;
            var receivedDamage = 0f;
            foreach (var value in idDamage)
            {
                id = value.Key;
                receivedDamage = value.Value;
                break;
            }
            if (id != PhotonView.ViewID) return;
            Health += receivedDamage;
            if (Health > 0) return;
            BonusDropCheck();
            PhotonNetwork.Destroy(gameObject);
        }

        private void BonusDropCheck()
        {
            if (Random.Range(0, 101) > bonusDropChance) return;
            PhotonNetwork.Instantiate(bonusPrefab.name, transform.position, Quaternion.identity);
        }
        
        public void Move(float deltaTime)
        {
            transform.position =
                Vector3.MoveTowards(transform.position, _currentTarget.position, moveSpeed * deltaTime);
        }

        public void OnEvent(EventData photonEvent)
        {
            switch (photonEvent.Code)
            {
                case EventCodePhoton.EnemyReceiveDamage:
                    ChangeHealthValue((Dictionary<int, float>)photonEvent.CustomData);
                    break;
            }
        }
    }
}