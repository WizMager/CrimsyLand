using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Game.Interfaces;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Game.Player
{
    public class PlayerBonusController : MonoBehaviour, IBonus, IOnEventCallback
    {
        public Action<int> OnWeaponChange;
        [SerializeField] private PhotonView photonView;

        public PhotonView PhotonView { get; private set; }

        private void Start()
        {
            PhotonView = photonView;
        }

        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        public void ReceiveBonus(BonusType bonusType, int id)
        {
            var idBonus = new Dictionary<int, int> {{id, (int) bonusType}};
            PhotonNetwork.RaiseEvent(EventCodePhoton.PlayerPickedUpBonus, idBonus,
                new RaiseEventOptions {Receivers = ReceiverGroup.All}, SendOptions.SendReliable);
        }

        private void BonusTake(Dictionary<int, int> idBonus)
        {
            var id = 0;
            var bonusIndex = 0;
            foreach (var value in idBonus)
            {
                id = value.Key;
                bonusIndex = value.Value;
            }

            if (id != PhotonView.ViewID) return;
            switch (bonusIndex)
            {
                case (int)BonusType.None:
                    Debug.LogError("Something wrong with take bonus type.");
                    break;
                case (int)BonusType.Pistol:
                    OnWeaponChange?.Invoke((int)WeaponIndex.Pistol);
                    break;
                case (int)BonusType.Rifle:
                    OnWeaponChange?.Invoke((int)WeaponIndex.Rifle);
                    break;
            }
        }
        
        public void OnEvent(EventData photonEvent)
        {
            switch (photonEvent.Code)
            {
                case EventCodePhoton.PlayerPickedUpBonus:
                    var idBonus = (Dictionary<int, int>) photonEvent.CustomData;
                    BonusTake(idBonus);
                    break;
            }
        }
    }
}