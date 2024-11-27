using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MillAction : MonoBehaviour
{
    [SerializeField]private Player player;
    [SerializeField]private GameObject flourUI;
    private bool isPlayerInTrigger = false;
    [SerializeField]private int wheatAmountRequired; 
    [SerializeField]private float timeToProcessWheat; 
    private bool isProcessing = false;
    private bool isActionFinished = false;
    private float processingTimer = 0f;
    [SerializeField]private Collectables flourPrefab;
    [SerializeField]private ParticleSystem flourParticleSystem;
    private void Awake()
    {
        timeToProcessWheat *= 60f;
    }
    private void Update()
    {
        if (CanInteract())
        {
            if (!isProcessing && !isActionFinished)
            {
                TryToProcessWheat();
            }
            if (!isProcessing && isActionFinished)
            {
                AddFlourToInventory();
            } 
        }
        if (isProcessing && !isActionFinished)
        {
            processingTimer += (TimeManager.timeManager.gameSpeed * TimeManager.timeManager.timeScale * Time.deltaTime);

            if (processingTimer >= timeToProcessWheat)
            {
                CompleteWheatToFlour();
            }
        }
    }
    private bool CanInteract()
    {
        return Input.GetKeyDown(KeyCode.E) && isPlayerInTrigger;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
        }
    }
    private void TryToProcessWheat()
    {
        if (HasSufficientWheat())
        {
            RemoveWheatFromInventory(wheatAmountRequired);
            isProcessing = true;
            processingTimer = 0f;

            if (flourParticleSystem != null)
            {
                flourParticleSystem.Play();
            }
        }
    }

    private bool HasSufficientWheat()
    {
        int wheatCount = 0;
        foreach (var slot in player.inventory.slots)
        {
            if (slot.type == CollectibleType.Wheat)
            {
                wheatCount += slot.itemCount;
                break;
            }
        }
        return wheatCount >= wheatAmountRequired;
    }

    private void RemoveWheatFromInventory(int amount)
    {
        for (int i = 0; i < player.inventory.slots.Count; i++)
        {
            if (player.inventory.slots[i].type == CollectibleType.Wheat)
            {
                player.inventory.slots[i].RemoveItem(amount);

                if (player.inventory.slots[i].itemCount == 0)
                {
                    player.inventory.slots[i].type = CollectibleType.None;
                    player.inventory.slots[i].icon = null;  
                    for (int j = i + 1; j < player.inventory.slots.Count; j++)
                    {
                        player.inventory.slots[j - 1].type = player.inventory.slots[j].type;
                        player.inventory.slots[j - 1].icon = player.inventory.slots[j].icon;
                        player.inventory.slots[j - 1].itemCount = player.inventory.slots[j].itemCount;

                        player.inventory.Remove(j, player.inventory.slots[j].itemCount);
                    }
                }
                break;
            }
        }
    }

    private void CompleteWheatToFlour()
    {
        if (flourParticleSystem != null)
        {
            flourParticleSystem.Stop();
        }
        flourUI.GetComponent<SpriteRenderer>().enabled = true;
        isActionFinished = true;
        isProcessing = false;     
        processingTimer = 0f;
    }

    private void AddFlourToInventory()
    {
        player.inventory.Add(flourPrefab);
        flourUI.GetComponent<SpriteRenderer>().enabled = false;
        isActionFinished = false;
    }
}
