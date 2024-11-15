using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable] 
public class PlantingInfo
{
    public Vector3Int tilePosition; 
    public Sprite seedType;
    public float lastWateredTime;
    public float health = 100f;
    private float lastHealthLossTime = 0f;
    private float timeSinceStart = 0f;
    public float growthModifier = 1f;
    public bool isMature = false;
    public PlantingInfo(Vector3Int tilePosition, Sprite seedType)
    {
        this.tilePosition = tilePosition;
        this.seedType = seedType;
        this.lastWateredTime = Mathf.Floor(TimeManager.timeManager.gameTime);
    }

    public void WaterPlant()
    {
        if(health <= 0 || isMature)
        {
            Debug.Log("Bu bitki kurumuş veya tamamen olgunlaşmış.");
            return;
        }

        float currentTime = Mathf.Floor(TimeManager.timeManager.gameTime);
        float timeSinceLastWatered = currentTime - lastWateredTime;

        if (timeSinceLastWatered < 0)
        {
            timeSinceLastWatered += 24f;
        }

        if (timeSinceLastWatered < 24f)
        {
            Debug.Log("24 saat dolmadı, sulama gereksiz.");
            return;
        }
        WateringEffect.wateringEffect.StartWateringEffect(tilePosition);
        health = Mathf.Min(health + 10f, 100f);
        lastWateredTime = currentTime;
        lastHealthLossTime = timeSinceStart;
        Debug.Log("Bitki sulandı, health arttı: " + health);
    }


    public void DailyHealthCheck()
    {
        if(health <= 0 || isMature)
        {
            return;
        }
        float currentTime = Mathf.Floor(TimeManager.timeManager.gameTime); 
        float timeSinceLastWatered = currentTime - lastWateredTime;

        timeSinceStart += 1f;

        if (timeSinceLastWatered < 0) 
        {
            timeSinceLastWatered += 24f; 
        }
        if (timeSinceLastWatered >= 36f && timeSinceStart - lastHealthLossTime >= 36f)
        {
            health -= 20f;
            lastHealthLossTime = timeSinceStart; 
        }
        if (health <= 0)
        {
            growthModifier = 0f; 
        }
        else if (health <= 20)
        {
            growthModifier = 0.25f; 
        }
        else if (health <= 50)
        {
            growthModifier = 0.5f; 
        }
        else if (health <= 80)
        {
            growthModifier = 0.75f; 
        }
        else
        {
            growthModifier = 1f; 
        }
    }
}
