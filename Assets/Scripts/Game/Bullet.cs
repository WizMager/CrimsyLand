using System;
using Game.Interfaces;
using Photon.Pun;
using UnityEngine;

namespace Game
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float maxFlyDistance;
        [SerializeField] private float damage;
        [SerializeField] private float flySpeed;
        [SerializeField] private PhotonView photonView;
        
        private void FixedUpdate()
        {
            if (!photonView.IsMine) return;
            Fly(Time.fixedDeltaTime);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!photonView.IsMine) return;
            var iEnemy = col.GetComponent<IEnemy>();
            if (iEnemy != null)
            {
                iEnemy.ChangeHealth(-damage, iEnemy.PhotonView.ViewID);
                PhotonNetwork.Destroy(gameObject);
            }
        }

        private void Fly(float deltaTime)
        {
            if (maxFlyDistance > 0)
            {
                var frameMove = deltaTime * flySpeed;
                maxFlyDistance -= frameMove;
                transform.Translate(transform.up * frameMove, Space.World);
            }
            else
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}