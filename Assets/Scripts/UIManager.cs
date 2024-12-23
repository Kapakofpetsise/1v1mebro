using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public GameObject uiCanvas; // Reference to the UI canvas
    public InputActionAsset inputActions; // Reference to the Input Action Asset

    private InputActionMap playerActionMap;
    private InputActionMap playerLocomotionMap;
    private InputActionMap uiActionMap;

    public static bool IsUIActive { get; private set; } = false;

    private void Start()
    {
        if (uiCanvas == null || inputActions == null)
        {
            Debug.LogError("UI Canvas or Input Action Asset is not assigned in UIManager.");
            return;
        }

        // Get the action maps
        playerLocomotionMap = inputActions.FindActionMap("PlayerLocomotionMap");
        playerActionMap = inputActions.FindActionMap("PlayerActionMap");
        uiActionMap = inputActions.FindActionMap("UI");

        if (playerLocomotionMap == null || playerActionMap == null || uiActionMap == null)
        {
            Debug.LogError("Action maps not found in the Input Action Asset.");
            return;
        }

        // Enable the gameplay action maps by default
        playerLocomotionMap.Enable();
        playerActionMap.Enable();
        uiActionMap.Disable();

        // Ensure the canvas is initially hidden
        uiCanvas.SetActive(false);
        IsUIActive = false;
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            ToggleUI();
        }
    }

    private void ToggleUI()
    {
        IsUIActive = !IsUIActive;
        uiCanvas.SetActive(IsUIActive); // Toggle the canvas visibility

        if (IsUIActive)
        {
            // Switch to UI action map
            Debug.Log("Enabling UI action map and disabling player action maps.");
            playerLocomotionMap.Disable();
            playerActionMap.Disable();
            uiActionMap.Enable();
            // Show the cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            // Switch to gameplay action maps
            Debug.Log("Disabling UI action map and enabling player action maps.");
            uiActionMap.Disable();
            playerLocomotionMap.Enable();
            playerActionMap.Enable();
            // Hide the cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}