using System.Collections;
using UnityEngine;

public class StoneBehaviour : MonoBehaviour
{
    public int stoneHealth = 3; // Taş 3 vuruşta kırılacak
    public AudioClip pickaxeHitSound; // Vuruş sesi (isteğe bağlı)
    public AudioClip stoneBreakSound; // Kırılma sesi (isteğe bağlı)
    public int stoneCount; // Taş tipi başına verilecek taş sayısı
    public Collectables stonePrefab; // Toplanabilir taş prefab'ı
    private bool isBreaking = false; // Taşın kırılma durumu
    private bool isDamaged = false; // Taş hasar aldı mı?

    // Vuruş animasyonunun bitiş süresi
    private float hitAnimationDuration = 0.5f; // Örnek süre, animasyonun bitişine yakın bir süre

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (PlayerMovement.playerMovement.animator.GetBool("isMining") &&
            other.CompareTag("PickaxeCollider") &&
            !isBreaking &&
            !isDamaged)
        {
            isDamaged = true; // Taş hasar aldı
            TakeDamage();
            StartCoroutine(ResetDamage());
        }
    }

    private void TakeDamage()
    {
        stoneHealth--;

        // Vuruş sesi
        if (pickaxeHitSound != null)
        {
            AudioSource.PlayClipAtPoint(pickaxeHitSound, transform.position);
        }

        // Her vuruşta taşı küçült
        float scaleFactor = 0.8f; // Her vuruşta %20 küçülme
        transform.localScale = new Vector3(transform.localScale.x * scaleFactor, transform.localScale.y * scaleFactor, transform.localScale.z);

        if (stoneHealth <= 0)
        {
            StartCoroutine(BreakStone());
        }
    }

    private IEnumerator BreakStone()
    {
        isBreaking = true;

        // Kırılma sesi
        if (stoneBreakSound != null)
        {
            AudioSource.PlayClipAtPoint(stoneBreakSound, transform.position);
        }

        // Vurma animasyonunun bitmesine 0.2 saniye kalana kadar bekle
        float animationTime = PlayerMovement.playerMovement.animator.GetCurrentAnimatorStateInfo(0).length;
        float remainingTime = animationTime - PlayerMovement.playerMovement.animator.GetCurrentAnimatorStateInfo(0).normalizedTime * animationTime;

        yield return new WaitForSeconds(Mathf.Max(0, remainingTime - hitAnimationDuration));

        // Taş yok ediliyor
        Destroy(gameObject);
        DropStones();
    }

    private IEnumerator ResetDamage()
    {
        yield return new WaitForSeconds(0.5f); // Vuruşlar arasında beklemek için süre
        isDamaged = false; // Taş artık hasar alabilir
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
