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
}
