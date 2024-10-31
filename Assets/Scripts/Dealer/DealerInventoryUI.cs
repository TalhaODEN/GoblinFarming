using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DealerInventoryUI : MonoBehaviour
{
    public List<Slot_UI> slots = new List<Slot_UI>();
    public List<Button> slotButtons = new List<Button>();
    public Player player;
    public static DealerInventoryUI dealerInventoryUI;
    public GameObject commonZone;
    public GameObject buyPanel;
    public GameObject sellPanel;
    public GameObject buyButton;
    public TextMeshProUGUI amountText;
    public TextMeshProUGUI countText;
    private int count;
    private int amount;
    private int selectedPlayerIndex;
    private int selectedDealerIndex;
    private bool isShaking = false;
    private void Awake()
    {
        if (dealerInventoryUI == null)
        {
            dealerInventoryUI = this;
        }
        selectedDealerIndex = selectedPlayerIndex = -1;
    }
    public void RefreshInventory()
    {
        var playerSlots = player.inventory.slots;
        for (int i = 0; i < slots.Count; i++)
        {
            if (i < playerSlots.Count && playerSlots[i].type != CollectibleType.None)
            {
                slots[i].SetItem(playerSlots[i]);
                slots[i].GetComponent<Slot_UI>().itemIcon.raycastTarget = true;
            }
            else
            {
                slots[i].SetEmpty();
                slots[i].GetComponent<Slot_UI>().itemIcon.raycastTarget = false;
            }
        }
    }

    public void ChooseItemPlayer(int index)
    {
        if (index != selectedPlayerIndex)
        {
            CloseClickedItems();
            selectedPlayerIndex = index;
            if (selectedDealerIndex != -1)
            {
                selectedDealerIndex = -1;
            }
            ChoosedItem.choosedItem.PlayerItemImages[index].type = Image.Type.Simple;
            OpenSellPanel();
            GetPrice(1);
            count = 1;
            UpdateCountandAmount();
        }
    }

    public void ChooseItemDealer(int index2)
    {
        if (index2 != selectedDealerIndex)
        {
            CloseClickedItems();
            selectedDealerIndex = index2;
            if (selectedPlayerIndex != -1)
            {
                selectedPlayerIndex = -1;
            }
            ChoosedItem.choosedItem.ShopItemImages[index2].type = Image.Type.Simple;
            OpenBuyPanel();
            GetPrice(0);
            count = 1;
            UpdateCountandAmount();
        }
    }

    public void CloseClickedItems()
    {
        int maxCount = Mathf.Max(slots.Count, DealerInventory.dealerInventory.availableItems.Count);

        for (int i = 0; i < maxCount; i++)
        {
            if (i < slots.Count && slots[i].itemIcon.sprite != null)
            {
                ChoosedItem.choosedItem.PlayerItemImages[i].type = Image.Type.Tiled;
            }

            if (i < DealerInventory.dealerInventory.availableItems.Count &&
                DealerInventory.dealerInventory.availableItems[i].GetComponentInChildren<ShopItem>().itemType != CollectibleType.None)
            {
                ChoosedItem.choosedItem.ShopItemImages[i].type = Image.Type.Tiled; 
            }
        }
    }
    public void CancelButton()
    {
        selectedDealerIndex = selectedPlayerIndex = -1;
        CloseClickedItems();
        UpdateCountandAmount(0);
        CloseAllPanels();
    }
    public void OpenBuyPanel()
    {
        buyPanel.SetActive(true);
        commonZone.SetActive(true);
        sellPanel.SetActive(false);
    }
    public void OpenSellPanel()
    {
        sellPanel.SetActive(true);
        commonZone.SetActive(true);
        buyPanel.SetActive(false);
    }
    public void CloseAllPanels()
    {
        buyPanel.SetActive(false);
        sellPanel.SetActive(false);
        commonZone.SetActive(false);
    }
    public void BuyButton()
    {
        bool result = GoldPanel.goldPanel.RemoveGold(amount * count);
        if (!result)
        {
            UIManager.uiManager.ShakeButton(buyButton);
        }
        else
        {
            var playerSlots = player.inventory.slots;
            bool itemAdded = false;

            for (int i = 0; i < playerSlots.Count; i++)
            {
                if (playerSlots[i].type == DealerInventory.dealerInventory.availableItems[selectedDealerIndex].GetComponent<ShopItem>().itemType)
                {
                    playerSlots[i].itemCount += count;
                    itemAdded = true;
                    break;
                }
            }

            if (!itemAdded)
            {
                for (int i = 0; i < playerSlots.Count; i++)
                {
                    if (playerSlots[i].type == CollectibleType.None)
                    {
                        playerSlots[i].type = DealerInventory.dealerInventory.availableItems[selectedDealerIndex].GetComponent<ShopItem>().itemType;
                        playerSlots[i].icon = DealerInventory.dealerInventory.availableItems[selectedDealerIndex].GetComponent<ShopItem>().itemIcon;
                        playerSlots[i].itemCount = count;
                        break;
                    }
                }
            }
            CloseClickedItems();
            RefreshInventory();
            CancelButton();
        }
    }

    public void SellButton()
    {
        var playerSlots = player.inventory.slots;
        var selectedSlot = playerSlots[selectedPlayerIndex];

        if (selectedSlot.itemCount > count)
        {
            selectedSlot.itemCount -= count;
        }
        else 
        {
            selectedSlot.type = CollectibleType.None;
            selectedSlot.icon = null;
            selectedSlot.itemCount = 0;
            ShiftSlots();
        }
        GoldPanel.goldPanel.AddGold(amount * count);
        CloseClickedItems();
        RefreshInventory();
        CancelButton();
    }

    private void ShiftSlots()
    {
        var playerSlots = player.inventory.slots;

        for (int i = 0; i < playerSlots.Count; i++)
        {
            if (playerSlots[i].type == CollectibleType.None)
            {
                for (int j = i + 1; j < playerSlots.Count; j++)
                {
                    if (playerSlots[j].type != CollectibleType.None)
                    {
                        playerSlots[i].type = playerSlots[j].type;
                        playerSlots[i].icon = playerSlots[j].icon;
                        playerSlots[i].itemCount = playerSlots[j].itemCount;

                        playerSlots[j].type = CollectibleType.None;
                        playerSlots[j].icon = null;
                        playerSlots[j].itemCount = 0;

                        break;
                    }
                }
            }
        }
    }



    public void IncreaseButton()
    {
        var playerSlots = player.inventory.slots;
        if((selectedPlayerIndex >= 0 && count < playerSlots[selectedPlayerIndex].itemCount) 
            || (selectedDealerIndex >= 0 && selectedPlayerIndex < 0))
        {
            count++;
            UpdateCountandAmount();
        }
    }
    public void DecreaseButton()
    {
        if(count > 1)
        {
            count--;
            UpdateCountandAmount();
        }
    }
    public void UpdateCountandAmount(int check=1)
    {
        switch (check)
        {
            case 1:
                countText.text = count.ToString();
                amountText.text = (amount * count).ToString();
                break;
            case 0:
                countText.text = (count = 1).ToString();
                amountText.text = (amount = 0).ToString();
                break;
            default:
                break;
        }
    }
    public void GetPrice(int check)
    {
        switch (check)
        {
            case 0:
                List<ShopItem> DealerList = DealerInventory.dealerInventory.availableItems;
                foreach (ShopItem shopItem in DealerList)
                {
                    if(shopItem.itemIcon.name == DealerList[selectedDealerIndex].itemIcon.name)
                    {
                        amount = shopItem.price;
                        return;
                    }
                }
                break;
            case 1:
                var playerSlots = player.inventory.slots;
                var collectableItems = ItemManager.itemManager.collectableItems;
                foreach (Inventory.Slot slot in playerSlots)
                {
                    if (slot.type == playerSlots[selectedPlayerIndex].type)
                    {
                        foreach (Collectables item in collectableItems)
                        {
                            if(slot.type == item.type)
                            {
                                amount = item.priceForSell;
                                return;
                            }
                        }  
                    }
                }
                break;
            default:
                break;
        }
    }
    


}
