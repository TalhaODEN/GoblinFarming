using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Inventory;

public class Planting : MonoBehaviour
{
    public static Planting planting;
    public Transform player;
    public Player PlayerObject;
    public Tilemap topTilemap;
    public Tilemap bottomTilemap;
    public Tile holeTile;
    public Tile afterHoleTile;
    private Inventory_UI inventory_UI;
    private Plowing plowing;
    private Tool_Inventory tool_inventory;
    public Vector3Int gridPosition;
    public List<PlantingInfo> plantingInfos = new List<PlantingInfo>();
    private float lastDailyCheckTime = -1f;
    public GameObject cursorObjectPrefab;
    private GameObject cursorObject; 

    private void Awake()
    {
        if (planting == null)
        {
            planting = this;
        }
        if (inventory_UI == null)
        {
            inventory_UI = Inventory_UI.inventory_UI;
        }
        if (plowing == null)
        {
            plowing = Plowing.plowing;
        }
        if (tool_inventory == null)
        {
            tool_inventory = Tool_Inventory.tool_inventory;
        }

        if (cursorObjectPrefab != null)
        {
            cursorObject = Instantiate(cursorObjectPrefab);
        }
    }

    private void Update()
    {
        int currentHour = Mathf.FloorToInt(TimeManager.timeManager.gameTime);

        if (currentHour != lastDailyCheckTime)
        {
            Debug.Log("DailyHealthCheck Çağrıldı.");
            DailyHealthCheckForAllPlants();
            lastDailyCheckTime = currentHour;
        }

        if (IsInputAllowed())
        {
            inventory_UI.show = false;
            return;
        }

        if (inventory_UI.show && cursorObject != null)
        {
            cursorObject.SetActive(true); 
            UpdateGridPosition(); 
            UpdateCursorPosition(); 

            if (CanPlantSeed())
            {
                TryPlantSeed();
            }
        }
        else if (cursorObject != null)
        {
            cursorObject.SetActive(false); 
        }

        if (!inventory_UI.show && CanPlantSeed())
        {
            HandleHarvest();
        }
    }

    private void UpdateCursorPosition()
    {
        if (cursorObject != null)
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = topTilemap.WorldToCell(mouseWorldPosition);

            Vector3 gridWorldPosition = topTilemap.GetCellCenterWorld(gridPosition);
            gridWorldPosition.x -= 0.5f;  

            cursorObject.transform.position = new Vector3(gridWorldPosition.x, gridWorldPosition.y, cursorObject.transform.position.z); // Z değeri sabit tutulur
        }
    }



    private void DailyHealthCheckForAllPlants()
    {
        foreach (var plantingInfo in plantingInfos)
        {
            if (plantingInfo.health > 0)
            {
                plantingInfo.DailyHealthCheck();
            }
        }
    }

    private bool IsInputAllowed()
    {
        return Input.GetKeyDown(KeyCode.Escape)
               || Input.GetKeyDown(KeyCode.Tab)
               || Input.GetKeyDown(KeyCode.Space)
               || Input.GetKeyDown(KeyCode.U)
               || inventory_UI == null
               || inventory_UI.inventoryPanel.activeSelf;
    }

    private bool CanPlantSeed()
    {
        return Input.GetMouseButtonDown(0)
               && tool_inventory.currentToolIndex == tool_inventory.toolButtons.Count - 1
               && !plowing.showGrid
               && !plowing.showUGrid
               && !CountPanel.countPanel.CountPanelScreen.activeSelf;
    }

    public void UpdateGridPosition()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        gridPosition = topTilemap.WorldToCell(mouseWorldPosition);
    }

    public void TryPlantSeed()
    {
        Vector3Int clickedTilePosition = topTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        Tile clickedTile = topTilemap.GetTile<Tile>(clickedTilePosition);
        Vector3Int playerPosition = topTilemap.WorldToCell(player.position);
        var slots = PlayerObject.inventory.slots;
        var clickedSaveIndex = inventory_UI.clickedSaveIndex;
        var seedIndex = inventory_UI.seedIndex;
        var seedDataList = SeedInfo.seedInfo.seedDataList;

        if (CheckPlantCondition(slots, clickedSaveIndex, slots.Count, playerPosition, clickedTilePosition))
        {
            if (clickedTile == null)
            {
                clickedTile = bottomTilemap.GetTile<Tile>(clickedTilePosition);
            }

            if (clickedTile == holeTile)
            {
                ExecutePlanting(clickedTilePosition, clickedSaveIndex, seedIndex, slots, seedDataList);
            }
        }
    }

    private void ExecutePlanting(Vector3Int position, int saveIndex, int seedIndex,
        List<Slot> slots, List<SeedData> seedDataList)
    {
        topTilemap.SetTile(position, afterHoleTile);
        slots[saveIndex].RemoveItem(1);
        inventory_UI.Refresh();

        PlantingInfo plantingInfo = new PlantingInfo(position, slots[saveIndex].icon);
        plantingInfos.Add(plantingInfo);

        if (seedIndex >= 0 && seedIndex < seedDataList.Count)
        {
            StartCoroutine(GrowPlant(plantingInfo, seedDataList[seedIndex]));
        }
    }

    private IEnumerator GrowPlant(PlantingInfo plantingInfo, SeedData seedData)
    {
        float totalGrowTime = seedData.timeToGrow * 60f;
        float elapsed = 0f;

        while (elapsed < totalGrowTime)
        {
            elapsed += (TimeManager.timeManager.gameSpeed * TimeManager.timeManager.timeScale * Time.deltaTime) * plantingInfo.growthModifier;

            float growthPercentage = elapsed / totalGrowTime;

            if (growthPercentage >= 0.25f && growthPercentage < 0.5f)
            {
                if (plantingInfo.health <= 0)
                {
                    topTilemap.SetTile(plantingInfo.tilePosition, seedData.fadedTiles[0]);
                    yield break;
                }

                topTilemap.SetTile(plantingInfo.tilePosition, seedData.growTiles[0]);
            }
            else if (growthPercentage >= 0.5f && growthPercentage < 0.75f)
            {
                if (plantingInfo.health <= 0)
                {
                    topTilemap.SetTile(plantingInfo.tilePosition, seedData.fadedTiles[1]);
                    yield break;
                }

                topTilemap.SetTile(plantingInfo.tilePosition, seedData.growTiles[1]);
            }
            else if (growthPercentage >= 0.75f)
            {
                if (plantingInfo.health <= 0)
                {
                    topTilemap.SetTile(plantingInfo.tilePosition, seedData.fadedTiles[2]);
                    yield break;
                }

                topTilemap.SetTile(plantingInfo.tilePosition, seedData.growTiles[2]);
            }

            yield return null;
        }

        if (plantingInfo.health > 0)
        {
            plantingInfo.isMature = true;
            topTilemap.SetTile(plantingInfo.tilePosition, seedData.growTiles[3]);
        }
    }

    private bool CheckPlantCondition(List<Slot> slots, int index,
        int slotsCount, Vector3Int player_position, Vector3Int tile_position)
    {
        return index >= 0 &&
               index < slotsCount
               && slots[index].itemCount > 0
               && Vector3Int.Distance(player_position, tile_position) <= 2;
    }

    private void HandleHarvest()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = topTilemap.WorldToCell(mouseWorldPosition);

        Tile clickedTile = null;

        if (topTilemap != null)
        {
            clickedTile = topTilemap.GetTile<Tile>(cellPosition);
        }

        if (clickedTile == null && bottomTilemap != null)
        {
            clickedTile = bottomTilemap.GetTile<Tile>(cellPosition);
        }

        if (clickedTile != null)
        {
            foreach (var mapping in ItemManager.itemManager.tileCollectibleMappings)
            {
                if (mapping.tile == clickedTile)
                {
                    harvestCrop(clickedTile, cellPosition);
                    break;
                }
            }
        }
    }

    private void harvestCrop(Tile clickedTile, Vector3Int cellPosition)
    {
        topTilemap.SetTile(cellPosition, holeTile);

        CollectibleType collectibleType = ItemManager.itemManager.GetCollectibleTypeForTile(clickedTile);

        Collectables collectiblePrefab = ItemManager.itemManager.GetItemByType(collectibleType);
        if (collectiblePrefab != null)
        {
            DropItem(collectiblePrefab, cellPosition);
        }
    }

    public void DropItem(Collectables item, Vector3Int cellPosition)
    {
        Vector3 spawnLocation = topTilemap.GetCellCenterWorld(cellPosition);

        float dropRadius = 1.5f;
        Vector3 spawnOffset;

        do
        {
            spawnOffset = UnityEngine.Random.insideUnitCircle * dropRadius;
        } while (spawnOffset.magnitude < 0.5f);

        Collectables droppedItem = Instantiate(item, spawnLocation + spawnOffset, Quaternion.identity);
        droppedItem.rb2d.AddForce(spawnOffset * 0.2f, ForceMode2D.Impulse);
    }
}
