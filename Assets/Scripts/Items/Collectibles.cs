using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CollectibleType
{
    None,
    CarrotSeed,
    BeetSeed,
    SunflowerSeed,
    Carrot,
    WoodLog,
    Stone,
}
public class Collectables : MonoBehaviour
{
    
    public CollectibleType type;
    public Sprite icon;
    public Rigidbody2D rb2d;
    public int priceForSell;
    public float rotationSpeed;
    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player)
        {
            player.inventory.Add(this);
            Destroy(gameObject);
        }
    }
}
