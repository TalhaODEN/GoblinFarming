using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    public float gameTime;  
    public int currentToolIndex;
    public List<PlantingInfo> plantedPlants = new List<PlantingInfo>();
    public int goldAmount;
    public List<Inventory.Slot> inventoryItems = new List<Inventory.Slot>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void SaveGame()
    {
        // Save Data oluşturuluyor
        SaveData saveData = new SaveData(TimeManager.timeManager.gameTime, Tool_Inventory.tool_inventory.currentToolIndex, Planting.planting.plantingInfos, GoldPanel.goldAmount, Player.player.inventory.slots);

        // Save Data JSON formatında kaydediliyor
        string saveJson = JsonUtility.ToJson(saveData);
        System.IO.File.WriteAllText("savefile.json", saveJson);
        Debug.Log("Game saved.");
    }

    public void LoadGame()
    {
        if (System.IO.File.Exists("savefile.json"))
        {
            string saveJson = System.IO.File.ReadAllText("savefile.json");
            SaveData saveData = JsonUtility.FromJson<SaveData>(saveJson);
            TimeManager.timeManager.gameTime = saveData.gameTime;  // Yüklenen zamanı TimeManager'a set et
            currentToolIndex = saveData.currentToolIndex;
            plantedPlants = saveData.plantedPlants;
            goldAmount = saveData.goldAmount;
            inventoryItems = saveData.inventoryItems;
            Debug.Log("Game loaded.");
        }
        else
        {
            Debug.LogError("Save file not found.");
        }
    }
}

