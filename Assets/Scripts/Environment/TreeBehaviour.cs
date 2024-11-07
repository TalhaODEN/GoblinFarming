using System.Collections;
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
        if (IsAxeHit(other))
        {
            isDamaged = true;
            TakeDamage();
            StartCoroutine(ResetDamage());
        }
    }

    private bool IsAxeHit(Collider2D other)
    {
        return PlayerMovement.playerMovement.animator.GetBool("isAxeHitting") &&
               other.CompareTag("AxeCollider") &&
               !isFalling &&
               !isDamaged;
    }

    private void TakeDamage()
    {
        treeHealth--;

        PlayChopSound();

        if (treeHealth <= 0)
        {
            StartCoroutine(FallTree());
        }
    }

    private void PlayChopSound()
    {
        if (chopSound != null)
        {
            AudioSource.PlayClipAtPoint(chopSound, transform.position);
        }
    }

    private IEnumerator FallTree()
    {
        isFalling = true;
        PlayFallSound();

        float remainingTime = GetRemainingAnimationTime();
        yield return new WaitForSeconds(Mathf.Max(0, remainingTime - hitAnimationDuration));

        yield return RotateTree();
        DropLogs();
        Instantiate(treeStumpPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void PlayFallSound()
    {
        if (fallSound != null)
        {
            AudioSource.PlayClipAtPoint(fallSound, transform.position);
        }
    }

    private float GetRemainingAnimationTime()
    {
        float animationTime = PlayerMovement.playerMovement.animator.GetCurrentAnimatorStateInfo(0).length;
        return animationTime - PlayerMovement.playerMovement.animator.GetCurrentAnimatorStateInfo(0).normalizedTime * animationTime;
    }

    private IEnumerator RotateTree()
    {
        for (float t = 0; t < 1f; t += Time.deltaTime)
        {
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(0, -90, t));
            yield return null;
        }
    }

    private IEnumerator ResetDamage()
    {
        yield return new WaitForSeconds(0.5f);
        isDamaged = false;
    }

    public void DropLogs()
    {
        Vector3 spawnLocation = transform.position;

        for (int i = 0; i < logCount; i++)
        {
            Vector3 spawnOffset;
            do
            {
                spawnOffset = UnityEngine.Random.insideUnitCircle * 2f;
            } while (spawnOffset.magnitude < 0.5f);

            Collectables droppedLog = Instantiate(logPrefab, spawnLocation + spawnOffset, Quaternion.identity);
            droppedLog.rb2d.AddForce(spawnOffset * 0.2f, ForceMode2D.Impulse);
        }
    }
}
