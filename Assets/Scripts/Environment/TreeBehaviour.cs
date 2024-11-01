using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBehaviour : MonoBehaviour
{
    public int treeHealth = 3; 
    public GameObject treeStumpPrefab; 
    public AudioClip chopSound; 
    public AudioClip fallSound; 
    public int logCount; 
    public Collectables logPrefab;
    private bool isFalling = false; 
    private bool isDamaged = false; 

    private float hitAnimationDuration = 0.5f; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (PlayerMovement.playerMovement.animator.GetBool("isAxeHitting") &&
            other.CompareTag("AxeCollider") &&
            !isFalling &&
            !isDamaged)
        {
            isDamaged = true; 
            TakeDamage();
            StartCoroutine(ResetDamage());
        }
    }

    private void TakeDamage()
    {
        treeHealth--;

        if (chopSound != null)
        {
            AudioSource.PlayClipAtPoint(chopSound, transform.position);
        }

        if (treeHealth <= 0)
        {
            StartCoroutine(FallTree());
        }
    }

    private IEnumerator FallTree()
    {
        isFalling = true;

        if (fallSound != null)
        {
            AudioSource.PlayClipAtPoint(fallSound, transform.position);
        }

        float animationTime = PlayerMovement.playerMovement.animator.GetCurrentAnimatorStateInfo(0).length;
        float remainingTime = animationTime - PlayerMovement.playerMovement.animator.GetCurrentAnimatorStateInfo(0).normalizedTime * animationTime;

        yield return new WaitForSeconds(Mathf.Max(0, remainingTime - hitAnimationDuration));

        for (float t = 0; t < 1f; t += Time.deltaTime)
        {
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(0, -90, t)); 
            yield return null;
        }
        DropLogs();
        Instantiate(treeStumpPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
        
    }

    private IEnumerator ResetDamage()
    {
        yield return new WaitForSeconds(0.5f); 
        isDamaged = false; 
    }
    public void DropLogs()
    {
        Vector3 spawnLocation = transform.position;

        float dropRadius = 2f;
        Vector3 spawnOffset;

        for (int i = 0; i < logCount; i++)
        {
            do
            {
                spawnOffset = UnityEngine.Random.insideUnitCircle * dropRadius;
            } while (spawnOffset.magnitude < 0.5f);

            Collectables droppedLog = Instantiate(logPrefab, spawnLocation + spawnOffset, Quaternion.identity);
            droppedLog.rb2d.AddForce(spawnOffset * 0.2f, ForceMode2D.Impulse);
        }
    }
}
