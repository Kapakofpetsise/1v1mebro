using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour
{
    public Animator playerAnimator; // Reference to the player's Animator
    public float damageAmount = 25f;
    public float cooldownTime = 1.0f; // Cooldown duration in seconds

    private HashSet<GameObject> cooldownSet = new HashSet<GameObject>();
    private bool isAttacking = false; // Flag to check if the sword is actively attacking

    private void OnTriggerEnter(Collider other)
    {
          // Check if the sword hits a shield
        if (other.CompareTag("Shield"))
        {
            // Cancel the player's attack animation
            if (playerAnimator != null && isAttacking)
            {
                playerAnimator.SetTrigger("CancelAnimation");
                Debug.Log("Attack animation canceled due to hitting a shield!");
            }
        }
        
        // Only process hits when the sword is actively swinging
        if (isAttacking && other.CompareTag("Player") && other.gameObject != transform.root.gameObject)
        {
            // Check if the player is already on cooldown
            if (!cooldownSet.Contains(other.gameObject))
            {
                PlayerController playerController = other.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.TakeDamage(damageAmount);
                    StartCoroutine(ApplyCooldown(other.gameObject)); // Start cooldown
                }
            }
        }
    }

    public void StartAttack()
    {
        isAttacking = true; // Enable attack state
    }

    public void StopAttack()
    {
        isAttacking = false; // Disable attack state
    }

    private IEnumerator ApplyCooldown(GameObject player)
    {
        // Add the player to the cooldown set
        cooldownSet.Add(player);

        // Wait for the cooldown duration
        yield return new WaitForSeconds(cooldownTime);

        // Remove the player from the cooldown set
        cooldownSet.Remove(player);
    }
}
