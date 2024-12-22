using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class PlayerActionsInput : MonoBehaviourPunCallbacks, PlayerControls.IPlayerActionMapActions, IPunObservable
{

    public bool AttackPressed { get; private set; }
    public bool BlockPressed { get; private set; }

    public PlayerControls PlayerControls { get; private set; }

    public override void OnEnable()
    {
        if (photonView.IsMine)
        {
            PlayerControls = new PlayerControls();
            PlayerControls.Enable();

            PlayerControls.PlayerActionMap.Enable();
            PlayerControls.PlayerActionMap.SetCallbacks(this);
        }
    }

    private void LateUpdate()
    {
        if (photonView.IsMine)
        {
            AttackPressed = false;
            BlockPressed = false;
        }
    }

    public override void OnDisable()
    {
        if (photonView.IsMine)
        {
            PlayerControls.PlayerActionMap.Disable();
            PlayerControls.PlayerActionMap.RemoveCallbacks(this);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        if (photonView.IsMine && context.performed)
        {
            AttackPressed = true;
        }

    }

    public void OnBlock(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (photonView.IsMine && context.performed)
        {
            BlockPressed = true;
        }

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(AttackPressed);
            stream.SendNext(BlockPressed);
        }
        else
        {
            // Network player, receive data
            AttackPressed = (bool)stream.ReceiveNext();
            BlockPressed = (bool)stream.ReceiveNext();
        }
    }
}