using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class TileCollectibleMapping
{
    public CollectibleType collectibleType; 
    public Tile tile; 
    public Collectables collectiblePrefab;
}

public class ItemManager : MonoBehaviour
{
    public Collectables[] collectableItems;
    public List<TileCollectibleMapping> tileCollectibleMappings; 

    private Dictionary<CollectibleType, Collectables> collectableItemsDict
        = new Dictionary<CollectibleType, Collectables>();
    public static ItemManager itemManager;
    private void Awake()
    {
        if(itemManager == null)
        {
            itemManager = this;
        }
        
    }
    private void Start()
    {
        foreach (Collectables item in collectableItems)
        {
            AddItem(item);
        }
    }
    private void AddItem(Collectables item)
    {
        if (!collectableItemsDict.ContainsKey(item.type))
        {
            collectableItemsDict.Add(item.type, item);
        }
    }

    public Collectables GetItemByType(CollectibleType type)
    {
        if (collectableItemsDict.ContainsKey(type))
        {
            return collectableItemsDict[type];
        }
        return null;
    }

    public CollectibleType GetCollectibleTypeForTile(Tile tile)
    {
        foreach (var mapping in tileCollectibleMappings)
        {
            if (mapping.tile == tile)
            {
                return mapping.collectibleType;
            }
        }
        return CollectibleType.None; 
    }
}
