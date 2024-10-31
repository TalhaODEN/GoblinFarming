using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StumpBehaviour : MonoBehaviour
{
    public int stumpHealth; 
    private bool isDamaged = false; 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (PlayerMovement.playerMovement.animator.GetBool("isAxeHitting") &&
            other.CompareTag("AxeCollider") &&
            !isDamaged)
        {
            isDamaged = true; 
            TakeDamage();
            StartCoroutine(ResetDamage());
        }
    }

    private void TakeDamage()
    {
        stumpHealth--;

        if (stumpHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
    private IEnumerator ResetDamage()
    {
        yield return new WaitForSeconds(0.5f); 
        isDamaged = false; 
    }
}