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
        if (!TryRemoveGold())
        {
            UIManager.uiManager.ShakeButton(buyButton);
            return;
        }

        bool itemAdded = TryAddItemToInventory();

        if (!itemAdded)
        {
            TryAddItemToEmptySlot();
        }

        CloseClickedItems();
        RefreshInventory();
        CancelButton();
    }

    private bool TryRemoveGold()
    {
        return GoldPanel.goldPanel.RemoveGold(amount * count);
    }

    private bool TryAddItemToInventory()
    {
        var playerSlots = player.inventory.slots;
        var itemType = GetSelectedItemType();

        foreach (var slot in playerSlots)
        {
            if (slot.type == itemType)
            {
                slot.itemCount += count;
                return true;
            }
        }
        return false;
    }

    private void TryAddItemToEmptySlot()
    {
        var playerSlots = player.inventory.slots;
        var itemType = GetSelectedItemType();
        var itemIcon = GetSelectedItemIcon();

        foreach (var slot in playerSlots)
        {
            if (slot.type == CollectibleType.None)
            {
                slot.type = itemType;
                slot.icon = itemIcon;
                slot.itemCount = count;
                break;
            }
        }
    }

    private CollectibleType GetSelectedItemType()
    {
        return DealerInventory.dealerInventory.availableItems[selectedDealerIndex]
            .GetComponent<ShopItem>()
            .itemType;
    }
    private Sprite GetSelectedItemIcon()
    {
        return DealerInventory.dealerInventory.availableItems[selectedDealerIndex]
            .GetComponent<ShopItem>()
            .itemIcon;
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
            if (IsSlotEmpty(playerSlots[i]))
            {
                ShiftItemToEmptySlot(i, playerSlots);
            }
        }
    }

    private bool IsSlotEmpty(Inventory.Slot slot)
    {
        return slot.type == CollectibleType.None;
    }

    private void ShiftItemToEmptySlot(int emptySlotIndex, List<Inventory.Slot> playerSlots)
    {
        for (int j = emptySlotIndex + 1; j < playerSlots.Count; j++)
        {
            if (!IsSlotEmpty(playerSlots[j]))
            {
                MoveItemToSlot(emptySlotIndex, j, playerSlots);
                ClearSlot(j, playerSlots);
                break;
            }
        }
    }

    private void MoveItemToSlot(int fromIndex, int toIndex, List<Inventory.Slot> playerSlots)
    {
        playerSlots[fromIndex].type = playerSlots[toIndex].type;
        playerSlots[fromIndex].icon = playerSlots[toIndex].icon;
        playerSlots[fromIndex].itemCount = playerSlots[toIndex].itemCount;
    }

    private void ClearSlot(int index, List<Inventory.Slot> playerSlots)
    {
        playerSlots[index].type = CollectibleType.None;
        playerSlots[index].icon = null;
        playerSlots[index].itemCount = 0;
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
                SetDealerItemPrice();
                break;

            case 1: 
                SetPlayerItemPrice();
                break;

            default:
                break;
        }
    }

    private void SetDealerItemPrice()
    {
        List<ShopItem> dealerItems = DealerInventory.dealerInventory.availableItems;
        ShopItem selectedItem = dealerItems[selectedDealerIndex];

        foreach (ShopItem shopItem in dealerItems)
        {
            if (shopItem.itemIcon.name == selectedItem.itemIcon.name)
            {
                amount = shopItem.price;
                return;
            }
        }
    }

    private void SetPlayerItemPrice()
    {
        var playerSlots = player.inventory.slots;
        var collectableItems = ItemManager.itemManager.collectableItems;

        Inventory.Slot selectedSlot = playerSlots[selectedPlayerIndex];

        foreach (Collectables item in collectableItems)
        {
            if (selectedSlot.type == item.type)
            {
                amount = item.priceForSell;
                return;
            }
        }
    }
}
