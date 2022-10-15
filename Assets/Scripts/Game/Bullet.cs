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

        private void FixedUpdate()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            Fly(Time.fixedDeltaTime);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!PhotonNetwork.IsMasterClient) return;
            var iEnemy = col.GetComponent<IEnemy>();
            if (iEnemy != null)
            {
                iEnemy.ChangeHealthSend(-damage);
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