using Photon.Pun;
using UnityEngine;

namespace Game
{
    public class PlayerInstantiate : MonoBehaviour, IPunInstantiateMagicCallback
    {
        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            info.Sender.TagObject = gameObject;
        }
    }
}