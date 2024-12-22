using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public string promptMessage;
    public void BaseInteract()
    {
        Debug.Log("Interacting with " + gameObject.name);
        Interact();
    }

    public abstract void Interact();



}
