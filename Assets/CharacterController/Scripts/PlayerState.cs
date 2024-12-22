using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [field: SerializeField] public PlayerMovementState MovementState { get; private set; } = PlayerMovementState.Idling;


    public void SetPlayerMovementState(PlayerMovementState newState){MovementState = newState;}

}
    public enum PlayerMovementState
    {
        Idling = 0,
        Walking = 1,
        Running = 2,
        Sprinting = 3,
        Strafing = 4,
        Attacking = 5,
    }
