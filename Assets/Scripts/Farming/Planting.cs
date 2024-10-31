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

    private BoundsInt interactableBounds;
    public Vector3Int gridPosition;
    private List<PlantingInfo> plantingInfos = new List<PlantingInfo>();

    private void Start()
    {
        if (planting == null)
        {
            planting = this;
        }
    }

    private void Update()
    {
        if (Inventory_UI.inventory_UI != null && Inventory_UI.inventory_UI.show) 
        {
            UpdateGridPosition();
            if (Input.GetMouseButtonDown(0) && Tool_Inventory.tool_inventory.currentToolIndex == 3
                && !Plowing.plowing.showGrid && !Plowing.plowing.showUGrid 
                && !CountPanel.countPanel.CountPanelScreen.activeSelf)
            {
                PlantSeedAtTile();
            }
        }
        else if (Input.GetMouseButton(0) && Tool_Inventory.tool_inventory.currentToolIndex == 3
            && !Plowing.plowing.showGrid && !Plowing.plowing.showUGrid && !Inventory_UI.inventory_UI.inventoryPanel.activeSelf)
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
    }

    public void UpdateGridPosition()
    {
        if (!Input.GetKeyDown(KeyCode.Escape) && !Input.GetKeyDown(KeyCode.Tab)
            && !Input.GetKeyDown(KeyCode.Space) && !Input.GetKeyDown(KeyCode.U)
            && Inventory_UI.inventory_UI != null 
            && Inventory_UI.inventory_UI.show && !Inventory_UI.inventory_UI.inventoryPanel.activeSelf) 
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            gridPosition = topTilemap.WorldToCell(mouseWorldPosition);
        }
        else
        {
            Inventory_UI.inventory_UI.show = false;
        }
    }

    public void OnDrawGizmos()
    {
        if (Inventory_UI.inventory_UI != null && Inventory_UI.inventory_UI.show) // null kontrolü eklendi
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(topTilemap.GetCellCenterWorld(gridPosition), new Vector3(1, 1, 0));
        }
        else
        {
            Gizmos.color = Color.clear;
        }
    }

    public void PlantSeedAtTile()
    {
        Vector3Int clickedTilePosition = topTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        Tile clickedTile = topTilemap.GetTile<Tile>(clickedTilePosition);
        Vector3Int playerPosition = topTilemap.WorldToCell(player.position);
        if (Inventory_UI.inventory_UI.clickedSaveIndex >= 0 &&
            Inventory_UI.inventory_UI.clickedSaveIndex < PlayerObject.inventory.slots.Count 
            && PlayerObject.inventory.slots[Inventory_UI.inventory_UI.clickedSaveIndex].itemCount > 0
            && Vector3Int.Distance(playerPosition, clickedTilePosition) <= 2)
        {
            if (clickedTile == null)
            {
                clickedTile = bottomTilemap.GetTile<Tile>(clickedTilePosition);

                if (clickedTile != null && clickedTile == holeTile)
                {
                    topTilemap.SetTile(clickedTilePosition, afterHoleTile);
                    PlayerObject.inventory.slots[Inventory_UI.inventory_UI.clickedSaveIndex].RemoveItem(1);
                    Inventory_UI.inventory_UI.Refresh();
                    PlantingInfo plantingInfo = new PlantingInfo(clickedTilePosition, PlayerObject.inventory.slots[Inventory_UI.inventory_UI.clickedSaveIndex].icon);
                    plantingInfos.Add(plantingInfo);
                    if (Inventory_UI.inventory_UI.seedIndex >= 0 && Inventory_UI.inventory_UI.seedIndex < SeedInfo.seedInfo.seedDataList.Count)
                    {
                        StartCoroutine(GrowPlant(plantingInfo, SeedInfo.seedInfo.seedDataList[Inventory_UI.inventory_UI.seedIndex]));
                    }
                }
            }
            else if (clickedTile == holeTile)
            {
                topTilemap.SetTile(clickedTilePosition, afterHoleTile);
                PlayerObject.inventory.slots[Inventory_UI.inventory_UI.clickedSaveIndex].RemoveItem(1);
                Inventory_UI.inventory_UI.Refresh();
                PlantingInfo plantingInfo = new PlantingInfo(clickedTilePosition, PlayerObject.inventory.slots[Inventory_UI.inventory_UI.clickedSaveIndex].icon);
                plantingInfos.Add(plantingInfo);
                if (Inventory_UI.inventory_UI.seedIndex >= 0 && Inventory_UI.inventory_UI.seedIndex < SeedInfo.seedInfo.seedDataList.Count)
                {
                    StartCoroutine(GrowPlant(plantingInfo, SeedInfo.seedInfo.seedDataList[Inventory_UI.inventory_UI.seedIndex]));
                }
            }
        }
    }

    private IEnumerator GrowPlant(PlantingInfo plantingInfo, SeedData seedData)
    {
        float totalGrowTime = seedData.timeToGrow;
        float elapsed = 0f;

        while (elapsed < totalGrowTime)
        {
            elapsed += TimeManager.timeManager.gameSpeed * Time.deltaTime;
            float growthPercentage = elapsed / totalGrowTime;

            if (growthPercentage >= 0.25f && growthPercentage < 0.5f)
            {
                topTilemap.SetTile(plantingInfo.tilePosition, seedData.growTiles[0]);
            }
            else if (growthPercentage >= 0.5f && growthPercentage < 0.75f)
            {
                topTilemap.SetTile(plantingInfo.tilePosition, seedData.growTiles[1]);
            }
            else if (growthPercentage >= 0.75f)
            {
                topTilemap.SetTile(plantingInfo.tilePosition, seedData.growTiles[2]);
            }

            yield return null;
        }
        topTilemap.SetTile(plantingInfo.tilePosition, seedData.growTiles[3]);
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
        Vector3 spawnLocation = topTilemap.GetCellCenterWorld(cellPosition); // Tıklanan karenin merkezinde spawnla

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
