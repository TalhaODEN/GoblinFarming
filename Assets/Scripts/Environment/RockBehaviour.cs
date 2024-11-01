using System.Collections;
using UnityEngine;

public class StoneBehaviour : MonoBehaviour
{
    public int stoneHealth = 3; 
    public AudioClip pickaxeHitSound; 
    public AudioClip stoneBreakSound; 
    public int stoneCount; 
    public Collectables stonePrefab; 
    private bool isBreaking = false; 
    private bool isDamaged = false; 
    private float hitAnimationDuration = 0.5f; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (PlayerMovement.playerMovement.animator.GetBool("isMining") &&
            other.CompareTag("PickaxeCollider") &&
            !isBreaking &&
            !isDamaged)
        {
            isDamaged = true; 
            TakeDamage();
            StartCoroutine(ResetDamage());
        }
    }

    private void TakeDamage()
    {
        stoneHealth--;

        if (pickaxeHitSound != null)
        {
            AudioSource.PlayClipAtPoint(pickaxeHitSound, transform.position);
        }

        float scaleFactor = 0.8f; 
        transform.localScale = new Vector3(transform.localScale.x * scaleFactor, transform.localScale.y * scaleFactor, transform.localScale.z);

        if (stoneHealth <= 0)
        {
            StartCoroutine(BreakStone());
        }
    }

    private IEnumerator BreakStone()
    {
        isBreaking = true;

        if (stoneBreakSound != null)
        {
            AudioSource.PlayClipAtPoint(stoneBreakSound, transform.position);
        }

        float animationTime = PlayerMovement.playerMovement.animator.GetCurrentAnimatorStateInfo(0).length;
        float remainingTime = animationTime - PlayerMovement.playerMovement.animator.GetCurrentAnimatorStateInfo(0).normalizedTime * animationTime;

        yield return new WaitForSeconds(Mathf.Max(0, remainingTime - hitAnimationDuration));

        Destroy(gameObject);
        DropStones();
    }

    private IEnumerator ResetDamage()
    {
        yield return new WaitForSeconds(0.5f); 
        isDamaged = false; 
    }

    public void DropStones()
    {
        Vector3 spawnLocation = transform.position;

        float dropRadius = 2f;
        Vector3 spawnOffset;

        for (int i = 0; i < stoneCount; i++)
        {
            do
            {
                spawnOffset = UnityEngine.Random.insideUnitCircle * dropRadius;
            } while (spawnOffset.magnitude < 0.5f);

            Collectables droppedStone = Instantiate(stonePrefab, spawnLocation + spawnOffset, Quaternion.identity);
            droppedStone.rb2d.AddForce(spawnOffset * 0.2f, ForceMode2D.Impulse);
        }
    }
}
