using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DealerManager : MonoBehaviour
{
    public GameObject player;
    private bool isPlayerInRange = false;
    private UIManager uiManager;
    private DealerUI dealerUI;
    private Inventory_UI inventory_UI;
    private BuildManager buildManager;
    private void Start()
    {
        if(uiManager == null)
        {
            uiManager = FindObjectOfType<UIManager>();
        }
        if(dealerUI == null)
        {
            dealerUI = FindObjectOfType<DealerUI>();
        }
        if(inventory_UI == null)
        {
            inventory_UI = FindObjectOfType<Inventory_UI>();
        }
        if(buildManager == null)
        {
            buildManager = FindObjectOfType<BuildManager>();
        }
    }
    void Update()
    {
        if (CanOpenDealerUI())
        {
            dealerUI.ToggleDealerUI();
        }
    }

    private bool CanOpenDealerUI()
    {
        return isPlayerInRange && 
               !uiManager.IsSpecificPanelsOpen(inventory_UI.inventoryPanel,buildManager.buildingPanel) &&
               Input.GetKeyDown(KeyCode.E);
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
            if (dealerUI.IsDealerUIActive())
            {
                dealerUI.CloseDealerUI();
            }
        }
    }
}
