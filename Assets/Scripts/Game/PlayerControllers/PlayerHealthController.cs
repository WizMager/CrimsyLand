using UnityEngine;
using UnityEngine.UI;

namespace Game.PlayerControllers
{
    public class PlayerHealthController : MonoBehaviour
    {
        [SerializeField] private Slider healthSlider;
        [SerializeField] private float startHealth;
        private float _playerHealth;
        
        private void Awake()
        {
            _playerHealth = startHealth;
        }

        public void ChangeHealth(float value)
        {
            _playerHealth += value;
        }
    }
}