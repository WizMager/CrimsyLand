using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Game
{
    public class PlayerSpawn : MonoBehaviour, IOnEventCallback
    {
        public Action OnPlayersInstantiated;
        [SerializeField] private GameObject playerPrefab;
        private int _spawnCounter;

        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        private void Start()
        {
            PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
            PhotonNetwork.RaiseEvent(EventCodePhoton.InstantiateNewPlayerComplete, null, new RaiseEventOptions {Receivers = ReceiverGroup.MasterClient},
                SendOptions.SendReliable);
        }

        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this); 
        }

        public void OnEvent(EventData photonEvent)
        {
            switch (photonEvent.Code)
            {
                case EventCodePhoton.InstantiateNewPlayerComplete:
                    _spawnCounter++;
                    if (_spawnCounter != PhotonNetwork.CurrentRoom.MaxPlayers) return;
                    OnPlayersInstantiated?.Invoke();
                    break;
            }
        }
    }
}