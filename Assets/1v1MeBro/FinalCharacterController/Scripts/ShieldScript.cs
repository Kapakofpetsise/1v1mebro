using System.Collections;
using UnityEngine;

public class ShieldScript : MonoBehaviour
{
    public Collider shieldCollider; // Reference to the shield's collider
    public PlayerActionsInput playerActionsInput; // Reference to the PlayerActionsInput script
    public Animator playerAnimator; // Reference to the player's Animator
    public float blockCooldown = 2.0f; // Cooldown duration in seconds
    public float stopBlockDelay = 0.5f; // Delay before stopping blocking in seconds
    public float colliderExtendedTime = 0.5f; // Additional time to keep the collider active after stopping blocking
    private bool isBlocking = false; // Whether the player is currently blocking
    private bool isCooldown = false; // Whether blocking is on cooldown
    private Coroutine stopBlockingCoroutine; // Reference to the StopBlocking coroutine

    private void Start()
    {
        // Ensure the shield's collider is initially disabled
        if (shieldCollider != null)
        {
            shieldCollider.enabled = false;
        }
        else
        {
            Debug.LogError("Shield collider is not assigned!");
        }

        if (playerActionsInput == null)
        {
            playerActionsInput = GetComponent<PlayerActionsInput>();
        }

        if (playerAnimator == null)
        {
            Debug.LogError("Player Animator is not assigned!");
        }
    }

    private void Update()
    {
        // Use the BlockPressed value from PlayerActionsInput
        if (playerActionsInput.BlockPressed && !isCooldown && !isBlocking)
        {
            StartBlocking();
        }
        else if (!playerActionsInput.BlockPressed && isBlocking)
        {
            if (stopBlockingCoroutine == null)
            {
                // Start a coroutine to delay stopping the block
                stopBlockingCoroutine = StartCoroutine(DelayedStopBlocking());
            }
        }
    }

    public void StartBlocking()
    {
        isBlocking = true;

        // Enable the shield's collider to allow blocking
        if (shieldCollider != null)
        {
            shieldCollider.enabled = true;
        }

        // Play the blocking animation
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("isBlocking", true);
        }

        Debug.Log("Blocking started: Shield collider enabled, animation playing.");
    }

    private IEnumerator DelayedStopBlocking()
    {
        // Wait for the specified delay before stopping the block
        yield return new WaitForSeconds(stopBlockDelay);

        // Stop the blocking animation
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("isBlocking", false);
            Debug.Log("Blocking animation stopped.");
        }

        // Wait for additional time before disabling the collider
        yield return new WaitForSeconds(colliderExtendedTime);

        // Disable the shield's collider
        if (shieldCollider != null)
        {
            shieldCollider.enabled = false;
        }

        Debug.Log("Shield collider disabled.");

        // Clear the coroutine reference and start cooldown
        stopBlockingCoroutine = null;
        StartCoroutine(BlockCooldownRoutine());
    }

    private IEnumerator BlockCooldownRoutine()
    {
        isBlocking = false; // Reset the blocking state
        isCooldown = true; // Activate the cooldown
        Debug.Log("Blocking is on cooldown.");

        yield return new WaitForSeconds(blockCooldown); // Wait for the cooldown duration

        isCooldown = false; // Cooldown ends
        Debug.Log("Blocking cooldown ended.");
    }
}
