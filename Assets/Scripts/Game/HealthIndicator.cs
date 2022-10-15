using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class HealthIndicator : MonoBehaviour
    {
        [SerializeField] private Slider healthSlider;
        [SerializeField] private Player player;

        private void Start()
        {
            player.OnHealthChange += OnHealthChangeHandler;
        }

        private void OnHealthChangeHandler(float healthValue)
        {
            healthSlider.value = healthValue;
        }
    }
}