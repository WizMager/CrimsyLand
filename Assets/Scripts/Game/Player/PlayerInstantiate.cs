using Photon.Pun;
using UnityEngine;

namespace Game
{
    public class PlayerInstantiate : MonoBehaviour, IPunInstantiateMagicCallback
    {
        // private void Awake()
        // {
        //     DontDestroyOnLoad(gameObject);
        // }

        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            info.Sender.TagObject = gameObject;
        }
    }
}