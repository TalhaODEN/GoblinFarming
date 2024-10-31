using System.Collections.Generic;
using UnityEngine;

public class DealerInventory : MonoBehaviour
{
    public List<ShopItem> availableItems;
    public static DealerInventory dealerInventory;

    private void Awake()
    {
        if(dealerInventory == null)
        {
            dealerInventory = this;
        }
    }
    public int GetPrice(Collectables item)
    {
        foreach (ShopItem shopItem in availableItems)
        {
            if (shopItem.itemType == item.type)
            {
                return shopItem.price;
            }
        }
        return 0; 
    }
}
