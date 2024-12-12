using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public GameObject uiCanvas; // Reference to the UI canvas
    public InputActionAsset inputActions; // Reference to the Input Action Asset

    private InputActionMap gameplayActionMap;
    private InputActionMap uiActionMap;

    private void Start()
    {
        if (uiCanvas == null || inputActions == null)
        {
            Debug.LogError("UI Canvas or Input Action Asset is not assigned in UIManager.");
            return;
        }

        // Get the action maps
        gameplayActionMap = inputActions.FindActionMap("Player");
        uiActionMap = inputActions.FindActionMap("UI");

        if (gameplayActionMap == null || uiActionMap == null)
        {
            Debug.LogError("Action maps not found in the Input Action Asset.");
            return;
        }

        // Enable the gameplay action map by default
        gameplayActionMap.Enable();
        uiActionMap.Disable();

        // Ensure the canvas is initially hidden
        uiCanvas.SetActive(false);
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
        bool isActive = !uiCanvas.activeSelf;
        uiCanvas.SetActive(isActive); // Toggle the canvas visibility

        if (isActive)
        {
            // Switch to UI action map
            gameplayActionMap.Disable();
            uiActionMap.Enable();
            // Show the cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            // Switch to gameplay action map
            uiActionMap.Disable();
            gameplayActionMap.Enable();
            // Hide the cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}