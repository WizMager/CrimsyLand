using Photon.Pun;
using UnityEngine;

namespace Game
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float startSpeed;
        [SerializeField] private float maxMoveDistance;

        private void Update()
        {
            if (maxMoveDistance > 0)
            {
                var frameMove = Time.deltaTime * startSpeed;
                maxMoveDistance -= frameMove;
                transform.Translate(transform.up * frameMove, Space.World);
            }
            else
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Enemy"))
            {
                PhotonNetwork.Destroy(col.gameObject);
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}