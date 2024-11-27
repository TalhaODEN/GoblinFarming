using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player player;
    public Inventory inventory;

    private void Awake()
    {
        if (player == null)
        {
            player = this;
        }
        else
        {
            Destroy(player);
        }
        inventory = new Inventory(27);
    }
    public void DropItem(Collectables item)
    {
        Vector3 spawnLocation = transform.position;
        float dropRadius = 2f;
        float playerRadius = GetComponent<Collider2D>().bounds.extents.magnitude;

        Vector3 spawnOffset;
        do
        {
            spawnOffset = Random.insideUnitCircle * dropRadius;
        } while (spawnOffset.magnitude < playerRadius * 1.5f);

        Collectables droppedItem = Instantiate(item, spawnLocation + spawnOffset, Quaternion.identity);
        droppedItem.rb2d.AddForce(spawnOffset * 0.2f, ForceMode2D.Impulse);
    }



}
