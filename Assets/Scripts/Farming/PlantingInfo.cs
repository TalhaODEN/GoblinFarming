using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable] 
public class PlantingInfo
{
    public Vector3Int tilePosition; 
    public Sprite seedType;

    public PlantingInfo(Vector3Int tilePosition, Sprite seedType)
    {
        this.tilePosition = tilePosition;
        this.seedType = seedType;
    }
}
