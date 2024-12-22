using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActionsInput : MonoBehaviour, PlayerControls.IPlayerActionMapActions
{
    
    public bool AttackPressed { get; private set; }
    public bool BlockPressed { get; private set; }
    
    public PlayerControls PlayerControls { get; private set; }

    private void OnEnable() {
        PlayerControls = new PlayerControls();
        PlayerControls.Enable();

        PlayerControls.PlayerActionMap.Enable();
        PlayerControls.PlayerActionMap.SetCallbacks(this);
    }

    private void LateUpdate() {
        AttackPressed = false;
        BlockPressed = false;
    }

    private void OnDisable() {
        PlayerControls.PlayerActionMap.Disable();
        PlayerControls.PlayerActionMap.RemoveCallbacks(this);
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if(!context.performed) return;
        AttackPressed = true;
    }

    public void OnBlock(InputAction.CallbackContext context)
    {
        if(!context.performed) return;
        BlockPressed = true;

    }
}