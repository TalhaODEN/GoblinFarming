using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[System.Serializable]
public class SaveData
{
    public float gameTime;  
    public int currentToolIndex;  
    public List<PlantingInfo> plantedPlants;  
    public int goldAmount;  
    public List<Inventory.Slot> inventoryItems;  

    public SaveData(float gameTime, int currentToolIndex, List<PlantingInfo> plantedPlants, int goldAmount, List<Inventory.Slot> inventoryItems)
    {
        this.gameTime = gameTime;
        this.currentToolIndex = currentToolIndex;
        this.plantedPlants = plantedPlants;
        this.goldAmount = goldAmount;
        this.inventoryItems = inventoryItems;
    }
}

