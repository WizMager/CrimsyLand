using Photon.Pun;
using UnityEngine;

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
}