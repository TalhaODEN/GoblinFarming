using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBehaviour : MonoBehaviour
{
    public int treeHealth = 3; // Ağaç 3 vuruşta yıkılacak
    public GameObject treeStumpPrefab; // Kök için prefab referansı
    public AudioClip chopSound; // Vuruş sesi (isteğe bağlı)
    public AudioClip fallSound; // Yıkılma sesi (isteğe bağlı)
    public int logCount; // Editörden ayarlanabilir kütük sayısı
    public Collectables logPrefab;
    private bool isFalling = false; // Ağaç devrilme durumunu kontrol
    private bool isDamaged = false; // Ağaç hasar alındı mı?

    // Vuruş animasyonunun bitiş süresi
    private float hitAnimationDuration = 0.5f; // Örnek süre, animasyonun bitişine yakın bir süre

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (PlayerMovement.playerMovement.animator.GetBool("isAxeHitting") &&
            other.CompareTag("AxeCollider") &&
            !isFalling &&
            !isDamaged)
        {
            isDamaged = true; // Ağaç hasar aldı
            TakeDamage();
            StartCoroutine(ResetDamage());
        }
    }

    private void TakeDamage()
    {
        treeHealth--;

        // Vuruş sesi
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

        // Yıkılma sesi
        if (fallSound != null)
        {
            AudioSource.PlayClipAtPoint(fallSound, transform.position);
        }

        // Vurma animasyonunun bitmesine 0.2 saniye kalana kadar bekle
        float animationTime = PlayerMovement.playerMovement.animator.GetCurrentAnimatorStateInfo(0).length;
        float remainingTime = animationTime - PlayerMovement.playerMovement.animator.GetCurrentAnimatorStateInfo(0).normalizedTime * animationTime;

        yield return new WaitForSeconds(Mathf.Max(0, remainingTime - hitAnimationDuration));

        // Ağaç yavaşça devriliyor (rotation animasyonu)
        for (float t = 0; t < 1f; t += Time.deltaTime)
        {
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(0, -90, t)); // 90 derece devrilme
            yield return null;
        }
        DropLogs();
        // Kök ekleme
        Instantiate(treeStumpPrefab, transform.position, Quaternion.identity);

        // Ağaç yok ediliyor
        Destroy(gameObject);
        
    }

    private IEnumerator ResetDamage()
    {
        yield return new WaitForSeconds(0.5f); // Vuruşların arasında beklemek için süre
        isDamaged = false; // Ağaç artık hasar alabilir
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
