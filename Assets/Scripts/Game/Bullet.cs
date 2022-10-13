using System;
using Game.Interfaces;
using Photon.Pun;
using UnityEngine;

namespace Game
{
    public class Bullet : MonoBehaviour, IBullet
    {
        [SerializeField] private float startSpeed;
        [SerializeField] private float maxMoveDistance;
        [SerializeField] private float damage;

        public float MoveSpeed => startSpeed;
        public float MaxFlyDistance => maxMoveDistance;
        public float Damage => damage;

        private void FixedUpdate()
        {
            Fly(Time.fixedDeltaTime);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            var iEnemy = col.GetComponent<IEnemy>();
            if (iEnemy != null)
            {
                iEnemy.ReceiveDamage(Damage);
                PhotonNetwork.Destroy(gameObject);
            }
        }
        
        public void Fly(float deltaTime)
        {
            if (maxMoveDistance > 0)
            {
                var frameMove = Time.fixedDeltaTime * startSpeed;
                maxMoveDistance -= frameMove;
                transform.Translate(transform.up * frameMove, Space.World);
            }
            else
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}