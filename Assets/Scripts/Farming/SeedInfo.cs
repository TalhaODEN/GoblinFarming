using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class SeedData
{
    public Sprite seedType;
    public List<Tile> growTiles;
    public float timeToGrow;
    public SeedData(Sprite seedType, List<Tile> growTiles,float timeToGrow)
    {
        this.seedType = seedType;
        this.growTiles = growTiles;
        this.timeToGrow = timeToGrow;
    }
}

public class SeedInfo : MonoBehaviour
{
    public List<SeedData> seedDataList;
    public static SeedInfo seedInfo;

    private void Awake()
    {
        if(seedInfo == null)
        {
            seedInfo = this;
        }
    }
    public void Init(List<SeedData> seedDataList)
    {
        this.seedDataList = seedDataList;
    }
}
