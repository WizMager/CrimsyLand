using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class HealthIndicator : MonoBehaviour
    {
        [SerializeField] private Slider healthSlider;
        [SerializeField] private PlayerController playerController;
        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Start()
        {
            playerController.OnHealthChange += OnHealthChangeHandler;
        }

        private void FixedUpdate()
        {
            //healthSlider.gameObject.transform.rotation = _mainCamera.transform.rotation;
        }

        private void OnHealthChangeHandler(float healthValue)
        {
            healthSlider.value = healthValue;
        }
    }
}