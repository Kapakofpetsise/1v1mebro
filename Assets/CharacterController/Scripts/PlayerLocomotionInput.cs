using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

[DefaultExecutionOrder(-2)]
public class PlayerLocomotionInput : MonoBehaviourPunCallbacks, PlayerControls.IPlayerLocomotionMapActions, IPunObservable
{

    public bool SprintToggledOn { get; private set; }
    public PlayerControls PlayerControls { get; private set; }
    public Vector2 MovementInput { get; private set; }
    public Vector2 LookInput { get; private set; }


    public override void OnEnable()
    {
        if (photonView.IsMine)
        {
            PlayerControls = new PlayerControls();
            PlayerControls.Enable();

            PlayerControls.PlayerLocomotionMap.Enable();
            PlayerControls.PlayerLocomotionMap.SetCallbacks(this);
        }
    }

    public override void OnDisable()
    {
        if (photonView.IsMine)
        {
            PlayerControls.PlayerLocomotionMap.Disable();
            PlayerControls.PlayerLocomotionMap.RemoveCallbacks(this);
        }
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (photonView.IsMine)
        {
            MovementInput = context.ReadValue<Vector2>();
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (photonView.IsMine)
        {
            LookInput = context.ReadValue<Vector2>();
        }
    }

    public void OnToggleSprint(InputAction.CallbackContext context)
    {
        if (photonView.IsMine)
        {
            if (context.performed)
            {
                SprintToggledOn = !SprintToggledOn;
            }
            else if (context.canceled)
            {
                SprintToggledOn = false;
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(SprintToggledOn);
            stream.SendNext(MovementInput);
            stream.SendNext(LookInput);
        }
        else
        {
            // Network player, receive data
            SprintToggledOn = (bool)stream.ReceiveNext();
            MovementInput = (Vector2)stream.ReceiveNext();
            LookInput = (Vector2)stream.ReceiveNext();
        }
    }
}
