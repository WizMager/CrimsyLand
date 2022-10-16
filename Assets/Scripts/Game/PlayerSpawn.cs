using System;
using System.Collections;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using UnityEngine;

namespace Game
{
    public class PlayerSpawn : MonoBehaviour, IOnEventCallback
    {
        public Action OnPlayerInstantiated;
        [SerializeField] private GameObject playerPrefab;

        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        private void Start()
        {
            if (PlayerManager.LocalPlayerInstance == null)
            {
                PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
                PhotonNetwork.RaiseEvent(EventCodePhoton.InstantiateAddPlayerComplete, null, new RaiseEventOptions {Receivers = ReceiverGroup.MasterClient},
                    SendOptions.SendReliable);
            }
            
            
        }

        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this); 
        }

        public void OnEvent(EventData photonEvent)
        {
            switch (photonEvent.Code)
            {
                case EventCodePhoton.InstantiateAddPlayerComplete:
                    OnPlayerInstantiated?.Invoke();
                    break;
            }
        }
    }
}