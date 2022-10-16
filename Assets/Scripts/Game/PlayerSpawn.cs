using Photon.Pun;
using UnityEngine;

namespace Game
{
    public class PlayerSpawn : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab;
        private void Start()
        {
            PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
        }
    }
}