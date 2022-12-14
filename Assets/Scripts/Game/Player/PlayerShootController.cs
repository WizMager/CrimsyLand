using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.PlayerControllers
{
    public class PlayerShootController : MonoBehaviour
    {
        public Action OnShootPress;
        [SerializeField] private PhotonView photonView;
        [SerializeField] private PlayerInput input;
        [SerializeField] private string useActionMap;

        private void OnEnable()
        {
            input.actions[useActionMap].Enable();
        }

        private void FixedUpdate()
        {
            if (!photonView.IsMine) return;
            Shoot();
        }

        private void OnDisable()
        {
            input.actions[useActionMap].Disable();
        }
        
        private void Shoot()
        {
            var shootAction = input.actions[useActionMap];
            if (shootAction.phase != InputActionPhase.Performed) return;
            OnShootPress?.Invoke();
        }
    }
}