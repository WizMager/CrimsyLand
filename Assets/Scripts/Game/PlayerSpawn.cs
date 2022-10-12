using Photon.Pun;
using UnityEngine;

namespace Game
{
    public class PlayerSpawn : MonoBehaviour
    {
        private void Start()
        {
            PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
        }
    }
}