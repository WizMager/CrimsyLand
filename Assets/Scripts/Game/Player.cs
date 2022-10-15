using System;
using ComponentScripts;
using Game.Interfaces;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public class Player : MonoBehaviour, IPlayer
    {
        public Action<float> OnHealthChange;
        [SerializeField] private PhotonView photonView;
        [SerializeField] private Transform playerBody;
        [SerializeField] private WeaponContainerComponent weaponContainer;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float startHealth;
        private WeaponSwitcher _weaponSwitcher;
        private IWeapon _weapon;
        private Camera _mainCamera;
        private InputActions _inputActions;
        private Vector3 _mouseWorldPosition;
        
        public float Health { get; private set; }
        
        #region MonoBeh

        private void Awake()
        {
            _inputActions = new InputActions();
            _mainCamera = Camera.main;
            Health = startHealth;
            _weaponSwitcher = new WeaponSwitcher(weaponContainer.GetWeaponsGameObjects);
            _weapon = _weaponSwitcher.SwitchWeapon(WeaponIndex.Pistol);
        }

        private void OnEnable()
        {
            _inputActions.MouseAndKeyboard.Enable();
        }

        private void FixedUpdate()
        {
            if (!photonView.IsMine) return;
            var fixedDeltaTime = Time.fixedDeltaTime;
            Move(fixedDeltaTime);
            Aim();
            Shoot();
        }

        private void OnDisable()
        {
            _inputActions.MouseAndKeyboard.Disable();
        }

        #endregion

        private void Move(float deltaTime)
        {
            var moveAction = _inputActions.MouseAndKeyboard.Move;
            if (moveAction.phase != InputActionPhase.Started) return;
            var moveDirection = moveAction.ReadValue<Vector2>() * moveSpeed * deltaTime;
            var currentPosition = playerBody.position;
            transform.position = new Vector3(moveDirection.x + currentPosition.x, moveDirection.y + currentPosition.y);
        }

        private void Aim()
        {
            var aimAction = _inputActions.MouseAndKeyboard.Aim;
            if (aimAction.phase != InputActionPhase.Started) return;
            var mousePosition = aimAction.ReadValue<Vector2>();
            _mouseWorldPosition = _mainCamera.ScreenToWorldPoint(mousePosition);
            var lookMouseDirection = (playerBody.position - _mouseWorldPosition).normalized;
            var angleAxisZ = Mathf.Atan2(lookMouseDirection.y, lookMouseDirection.x) * Mathf.Rad2Deg + 90;
            playerBody.rotation = Quaternion.Euler(0, 0, angleAxisZ);
        }

        private void Shoot()
        {
            var shootAction = _inputActions.MouseAndKeyboard.Shoot;
            if (shootAction.phase != InputActionPhase.Performed) return;
            _weapon.Shoot();
        }
        public void ChangeHealth(float value)
        {
            Health += value;
            OnHealthChange?.Invoke(Health);
            Debug.Log(Health);
        }

        public void ReceiveBonus(BonusType bonusType)
        {
            switch (bonusType)
            {
                case BonusType.None:
                    break;
                case BonusType.Pistol:
                    _weapon = _weaponSwitcher.SwitchWeapon(WeaponIndex.Pistol);
                    break;
                case BonusType.Rifle:
                    _weapon = _weaponSwitcher.SwitchWeapon(WeaponIndex.Rifle);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(bonusType), bonusType, null);
            }
        }
    }
}