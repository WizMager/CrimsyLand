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
            photonView.RPC("BonusTake", RpcTarget.All, id, bonusType);
        }

        [PunRPC]
        private void BonusTake(int id, int bonusIndex)
        {
            if (id != photonView.ViewID) return;
            Debug.Log(bonusIndex);
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