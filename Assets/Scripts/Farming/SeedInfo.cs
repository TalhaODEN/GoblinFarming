using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.Tilemaps;

[System.Serializable]
public class SeedData
{
    public Sprite seedType;
    public List<Tile> growTiles;
    public List<Tile> fadedTiles;
    public float timeToGrow; 
    public SeedData(Sprite seedType, List<Tile> growTiles,float timeToGrow,List<Tile> fadedTiles)
    {
        this.seedType = seedType;
        this.growTiles = growTiles;
        this.timeToGrow = timeToGrow;
        this.fadedTiles = fadedTiles;
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
