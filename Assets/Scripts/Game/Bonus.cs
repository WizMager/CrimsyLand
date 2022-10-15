using System.Collections;
using ComponentScripts;
using Game.Interfaces;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public class Bonus : MonoBehaviour
    {
        [SerializeField] private BonusesContainerComponent bonuses;
        [SerializeField] private float selfDestroyTime;
        private BonusType _currentBonus;

        #region MonoBeh

        private void Awake()
        {
            BonusSelect(bonuses.GetBonusesScripts);
        }
        
        private void Start()
        {
            StartCoroutine(SelfDestroy());
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            var player = col.GetComponent<IPlayer>();
            if (player != null)
            {
                player.ReceiveBonus(_currentBonus);
                PhotonNetwork.Destroy(gameObject);
            }
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
            for (int i = 0; i < bonusTypeScripts.Length; i++)
            {
                if (i != randomIndex) continue;
                bonusTypeScripts[i].gameObject.SetActive(true);
                _currentBonus = bonusTypeScripts[i].GetBonusType;
            }
        }
    }
}