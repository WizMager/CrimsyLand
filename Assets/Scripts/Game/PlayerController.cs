using ComponentScripts;
using Game.Interfaces;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PhotonView photonView;
        [SerializeField] private Transform playerBody;
        [SerializeField] private WeaponContainerComponent weaponContainer;
        [SerializeField] private float moveSpeed;
        private WeaponSwitcher _weaponSwitcher;
        private IWeapon _weapon;
        private Camera _mainCamera;
        private InputActions _inputActions;
        private Vector3 _mouseWorldPosition;

        #region MonoBeh

        private void Awake()
        {
            _inputActions = new InputActions();
            _mainCamera = Camera.main;
            _weaponSwitcher = new WeaponSwitcher(weaponContainer.GetWeaponsGameObjects);
            _weapon = _weaponSwitcher.SwitchWeapon(WeaponIndex.Pistol);
        }

        private void OnEnable()
        {
            _inputActions.MouseAndKeyboard.Enable();
        }

        private void FixedUpdate()
        {
            //For switch weapon test
            if (_inputActions.MouseAndKeyboard.SpaceBar.phase == InputActionPhase.Performed)
            {
                _weapon = _weaponSwitcher.SwitchWeapon(WeaponIndex.Rifle);
            }
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
            playerBody.position = new Vector3(moveDirection.x + currentPosition.x, moveDirection.y + currentPosition.y);
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
    }
}