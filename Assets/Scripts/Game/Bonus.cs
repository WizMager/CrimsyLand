using System.Collections;
using ComponentScripts;
using ExitGames.Client.Photon;
using Game.Interfaces;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public class Bonus : MonoBehaviour, IOnEventCallback
    {
        [SerializeField] private BonusesContainerComponent bonuses;
        [SerializeField] private float selfDestroyTime;
        private BonusType _currentBonus;

        #region MonoBeh

        private void Awake()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            BonusSelect(bonuses.GetBonusesScripts);
        }
        
        private void Start()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            StartCoroutine(SelfDestroy());
        }

        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            var player = col.GetComponent<IBonus>();
            if (player != null)
            {
                player.ReceiveBonus(_currentBonus, player.PhotonView.ViewID);
                PhotonNetwork.Destroy(gameObject);
            }
        }

        private void OnDisable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        #endregion
        
        private IEnumerator SelfDestroy()
        {
            for (float i = 0; i < selfDestroyTime; i += Time.deltaTime)
            {
                yield return null;
            }
            PhotonNetwork.Destroy(gameObject);
        }
        
        private void BonusSelect(BonusComponent[] bonusTypeScripts)
        {
            var randomIndex = Random.Range(0, bonusTypeScripts.Length);
            PhotonNetwork.RaiseEvent(EventCodePhoton.ChosenBonusSend, randomIndex, new RaiseEventOptions{ Receivers = ReceiverGroup.All},
                SendOptions.SendReliable);
        }

        private void BonusSelectedReceive(int index)
        {
            var bonusesComponents = bonuses.GetBonusesScripts;
            for (int i = 0; i < bonusesComponents.Length; i++)
            {
                if (i != index) continue;
                bonusesComponents[i].gameObject.SetActive(true);
                _currentBonus = bonusesComponents[i].GetBonusType;
            }
        }
        
        public void OnEvent(EventData photonEvent)
        {
            if (photonEvent.Code == EventCodePhoton.ChosenBonusSend)
            {
                BonusSelectedReceive((int)photonEvent.CustomData);
            }
        }
    }
}