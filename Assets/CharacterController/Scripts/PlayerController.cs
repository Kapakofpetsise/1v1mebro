using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;

[DefaultExecutionOrder(-1)]
public class PlayerController : MonoBehaviourPunCallbacks
{
    #region Class Variables

    [Header("Components")]
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Camera _playerCamera;
    public SwordScript swordScript;
    public Animator playerAnimator;

    [Header("Movement")]
    public float runAcceleration = 0.25f;
    public float runSpeed = 4f;
    public float sprintAcceleration = 0.5f;
    public float sprintSpeed = 7f;
    public float drag = 0.1f;
    public float movingThreshold = 0.1f;
    public float gravity = -9.81f;

    [Header("Camera Settings")]
    public float lookSensHor = 2f;
    public float lookSensVer = 2f;
    public float lookLimitVer = 89f;

    [Header("Combat Variables")]
    public float attackRange = 2f;
    public float attackRate = 2f;
    public float attackDamage = 10f;
    public float playerHealth = 100f;
    public float currentHealth = 100f;


    private PlayerLocomotionInput _playerLocomotionInput;
    private PlayerState _playerState;
    private Vector2 _cameraRotation = Vector2.zero;
    private Vector2 _playerTargetRotation = Vector2.zero;
    private Vector3 _velocity = Vector3.zero;

    #endregion


    #region Startup
    private void Awake()
    {
        _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
        _playerState = GetComponent<PlayerState>();
        if (swordScript == null)
        {
            swordScript = GetComponentInChildren<SwordScript>();
        }

        // Disable the camera if this is not the local player
        if (!photonView.IsMine && _playerCamera != null)
        {
            _playerCamera.gameObject.SetActive(false);
        }


    }
    #endregion

    private void Update()
    {
        HandleLateralMovement();
        ApplyGravity();
        UpdateMovementState();
        _characterController.Move(_velocity * Time.deltaTime);


    }

    public void UpdateMovementState()
    {
        bool isMovementInput = _playerLocomotionInput.MovementInput != Vector2.zero;
        bool isMovingLaterally = IsMovingLaterally();
        bool isSprinting = _playerLocomotionInput.SprintToggledOn && isMovingLaterally;


        PlayerMovementState lateralState = isSprinting ? PlayerMovementState.Sprinting :
                                           isMovingLaterally || isMovementInput ? PlayerMovementState.Running : PlayerMovementState.Idling;
        _playerState.SetPlayerMovementState(lateralState);
    }

    public void HandleLateralMovement()
    {

        bool isSprinting = _playerState.MovementState == PlayerMovementState.Sprinting;

        float lateralAcceleration = isSprinting ? sprintAcceleration : runAcceleration;
        float clampLateralMagnitude = isSprinting ? sprintSpeed : runSpeed;

        Vector3 cameraForwardXZ = new Vector3(_playerCamera.transform.forward.x, 0f, _playerCamera.transform.forward.z).normalized;
        Vector3 cameraRightXZ = new Vector3(_playerCamera.transform.right.x, 0f, _playerCamera.transform.right.z).normalized;
        Vector3 moveDirection = cameraForwardXZ * _playerLocomotionInput.MovementInput.y + cameraRightXZ * _playerLocomotionInput.MovementInput.x;

        Vector3 movementDelta = moveDirection * lateralAcceleration;
        Vector3 horizontalVelocity = new Vector3(_velocity.x, 0f, _velocity.z) + movementDelta;

        Vector3 currentDrag = horizontalVelocity * drag * Time.deltaTime;
        horizontalVelocity = (horizontalVelocity.magnitude > drag * Time.deltaTime) ? horizontalVelocity - currentDrag : Vector3.zero;
        horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, clampLateralMagnitude);

        _velocity.x = horizontalVelocity.x;
        _velocity.z = horizontalVelocity.z;
    }

    private void ApplyGravity()
    {
        if (!_characterController.isGrounded)
        {
            _velocity.y += gravity * Time.deltaTime;
        }
        else
        {
            _velocity.y = -2f; // Small negative value to ensure the character stays grounded
        }
    }

    private void LateUpdate()
    {
        if (!photonView.IsMine) return;

        _cameraRotation.x += _playerLocomotionInput.LookInput.x * lookSensHor;
        _cameraRotation.y = Mathf.Clamp(_cameraRotation.y - lookSensVer * _playerLocomotionInput.LookInput.y, -lookLimitVer, lookLimitVer);

        _playerTargetRotation.x += transform.eulerAngles.x + lookSensHor * _playerLocomotionInput.LookInput.x;
        transform.rotation = Quaternion.Euler(0f, _playerTargetRotation.x, 0f);

        _playerCamera.transform.rotation = Quaternion.Euler(_cameraRotation.y, _cameraRotation.x, 0f);
    }

    private bool IsMovingLaterally()
    {

        Vector3 lateralVelocity = new Vector3(_characterController.velocity.x, 0f, _characterController.velocity.y);

        return lateralVelocity.magnitude > movingThreshold;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log($"{gameObject.name} took {damageAmount} damage! Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
         Debug.Log($"{gameObject.name} has died!");

        // Play death animation
        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("Die");
        }

    }

    public void StartAttack()
    {
        swordScript?.StartAttack();
    }

    public void StopAttack()
    {
        swordScript?.StopAttack();
    }

    public float GetHealth()
    {
        return currentHealth;
    }
}
