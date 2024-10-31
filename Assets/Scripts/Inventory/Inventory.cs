using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Collectables;

[System.Serializable]
public class Inventory
{
    [System.Serializable]
    public class Slot
    {
        public CollectibleType type;
        public int maxAllowed;
        public int itemCount;
        public Sprite icon;
        public Slot()
        {
            type = CollectibleType.None;
            itemCount = 0;
            maxAllowed = 99;
        } 
        public bool CanAddItem()
        {
            if (itemCount < maxAllowed)
            {
                return true;
            }
            return false;
        }

        public void AddItem(Collectables item)
        {
            this.type = item.type;
            this.icon = item.icon;
            itemCount++;
        }
        public void RemoveItem(int amount)
        {
            if (itemCount > 0)
            {
                itemCount -= amount; 
                if (itemCount <= 0)
                {
                    icon = null;
                    type = CollectibleType.None;
                }
            }
        }



    }

    public List<Slot> slots = new List<Slot>();
    public static Inventory inventory;
    public Inventory(int numSlots)
    {
        if (inventory == null) 
        {
            inventory = this;
        }

        for (int i = 0; i < numSlots; i++)
        {
            Slot slot = new Slot();
            slots.Add(slot);


        }

    }

    public void Add(Collectables item)
    {
        foreach(Slot slot in slots)
        {
            if(slot.type == item.type && slot.CanAddItem())
            {
                slot.AddItem(item);
                return;
            }
        }

        foreach(Slot slot in slots)
        {
            if(slot.type == CollectibleType.None)
            {
                slot.AddItem(item);
                return;
            }
        }


    }
    public void Remove(int index,int dropCount)
    {
        slots[index].RemoveItem(dropCount);
    }
}
