using Photon.Pun;
using UnityEngine;

namespace Game
{
    public class PlayerSpawn : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab;
        private void Start()
        {
            if (!PhotonNetwork.LocalPlayer.IsLocal) return;
            PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
        }
    }
}