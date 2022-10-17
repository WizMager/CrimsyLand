using Game.Interfaces;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Game.PlayerControllers
{
    public class PlayerHealthController : MonoBehaviour, IPlayerHealth
    {
        [SerializeField] private PhotonView photonView;
        [SerializeField] private Slider healthSlider;
        [SerializeField] private float startHealth;
        
        public PhotonView PhotonView { get; private set; }
        
        public float Health { get; private set; }

        private void Awake()
        {
            Health = startHealth;
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

        public void ChangeHealth(float value)
        {
            Health += value;
            healthSlider.value = Health;
        }
    }
}