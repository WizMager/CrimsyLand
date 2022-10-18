using System;
using Game.Interfaces;
using Photon.Pun;
using UnityEngine;

namespace Game.Player
{
    public class PlayerBonusReceiverController : MonoBehaviour, IBonusReceiver
    {
        public Action<int> OnWeaponChange;
        [SerializeField] private PhotonView photonView;

        public PhotonView PhotonView { get; private set; }

        private void Start()
        {
            PhotonView = photonView;
        }

        public void ReceiveBonus(int id, int bonusType)
        {
            Photon.Realtime.Player player = null;
            foreach (var playerOne in PhotonNetwork.PlayerList)
            {
                if (id != playerOne.ActorNumber) continue;
                player = playerOne;
            }
            photonView.RPC("BonusTake", player, bonusType);
        }

        [PunRPC]
        private void BonusTake(int bonusIndex)
        {
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
    }
}