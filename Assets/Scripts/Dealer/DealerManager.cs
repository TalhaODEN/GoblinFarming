using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DealerManager : MonoBehaviour
{
    public GameObject player;

    private bool isPlayerInRange = false;

    void Update()
    {
        if (isPlayerInRange && !DealerUI.dealerUI.isDealerUIActive 
            && !Inventory_UI.inventory_UI.inventoryPanel.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            DealerUI.dealerUI.ToggleDealerUI();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            isPlayerInRange = false;
            if (DealerUI.dealerUI.IsDealerUIActive())
            {
                DealerUI.dealerUI.CloseDealerUI();
            }
        }
    }
}
