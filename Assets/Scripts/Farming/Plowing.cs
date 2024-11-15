using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Plowing : MonoBehaviour
{
    public static Plowing plowing;
    public bool showGrid = false;
    public bool showUGrid = false;
    private Vector3 playerLastPosition;
    public Transform player;
    public Tilemap topTilemap;
    public Tilemap bottomTilemap;
    public Tile plowingTile;
    private BoundsInt interactableBounds;
    public List<Tile> plowableTiles;
    public List<Tile> unplowableTiles;
    public Tile unplowingTile;
    private UIManager uiManager;
    private Tool_Inventory tool_inventory;
    private void Awake()
    {
        if (plowing == null)
        {
            plowing = this;
        }
        uiManager = FindObjectOfType<UIManager>();
        tool_inventory = FindObjectOfType<Tool_Inventory>();
    }

    private void Update()
    {
        if (uiManager.IsAnyPanelOpen() || 
            tool_inventory.currentToolIndex != tool_inventory.toolButtons.Count - 1)
        {
            showGrid = showUGrid = false;
        }

        HandleInput();
        if (showGrid && Input.GetMouseButtonDown(0))
        {
            PlaceTile();
        }
        if (showUGrid && Input.GetMouseButtonDown(0))
        {
            ConvertTile();
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
           showUGrid = false;
           ToggleGridVisibility(ref showGrid);
           if (showGrid) UpdateInteractableBounds();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
           showGrid = false;
           ToggleGridVisibility(ref showUGrid);
           if (showUGrid) UpdateInteractableBounds();
        } 
    }

    private void ToggleGridVisibility(ref bool gridVisibility)
    {
        gridVisibility = !gridVisibility;

        if (gridVisibility)
        {
            playerLastPosition = player.position;
            UpdateInteractableBounds();
        }
    }

    private void PlaceTile()
    {
        Vector3Int cellPosition = GetCellPosition();

        if (!IsValidPlacement(cellPosition)) return;

        if (interactableBounds.Contains(cellPosition))
        {
            Tile clickedTile = topTilemap.GetTile<Tile>(cellPosition);
            if (plowableTiles.Contains(clickedTile))
            {
                topTilemap.SetTile(cellPosition, plowingTile);
                return;
            }
        }

        cellPosition = bottomTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (interactableBounds.Contains(cellPosition))
        {
            Tile clickedTile = bottomTilemap.GetTile<Tile>(cellPosition);
            if (plowableTiles.Contains(clickedTile))
            {
                bottomTilemap.SetTile(cellPosition, plowingTile);
            }
        }
    }

    private void ConvertTile()
    {
        Vector3Int cellPosition = GetCellPosition();

        if (!IsValidPlacement(cellPosition)) return;

        if (interactableBounds.Contains(cellPosition))
        {
            Tile clickedTile = topTilemap.GetTile<Tile>(cellPosition);
            if (unplowableTiles.Contains(clickedTile))
            {
                topTilemap.SetTile(cellPosition, unplowingTile);
                return;
            }
        }

        cellPosition = bottomTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (interactableBounds.Contains(cellPosition))
        {
            Tile clickedTile = bottomTilemap.GetTile<Tile>(cellPosition);
            if (unplowableTiles.Contains(clickedTile))
            {
                bottomTilemap.SetTile(cellPosition, unplowingTile);
            }
        }
    }

    private Vector3Int GetCellPosition()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return topTilemap.WorldToCell(mouseWorldPosition);
    }

    private bool IsValidPlacement(Vector3Int cellPosition)
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(topTilemap.GetCellCenterWorld(cellPosition), new Vector2(1, 1), 0f);
        foreach (var collider in colliders)
        {
            if (!collider.CompareTag("Player") && !collider.CompareTag("Collectables") && !collider.CompareTag("Untagged"))
            {
                return false;
            }
        }
        return true;
    }

    private void UpdateInteractableBounds()
    {
        interactableBounds = new BoundsInt(topTilemap.WorldToCell(playerLastPosition) + new Vector3Int(-1, -1, 0), new Vector3Int(3, 3, 1));
    }

    private void OnDrawGizmos()
    {
        if ((showGrid || showUGrid) && 
            tool_inventory.currentToolIndex == tool_inventory.toolButtons.Count - 1)
        {
            Gizmos.color = showGrid ? Color.white : Color.blue;

            Vector3 cellSize = topTilemap.cellSize;
            Vector3 playerPosition = new Vector3(
                Mathf.Floor(playerLastPosition.x / cellSize.x) * cellSize.x,
                Mathf.Floor(playerLastPosition.y / cellSize.y) * cellSize.y,
                playerLastPosition.z
            );

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    Vector3 cellPosition = playerPosition + new Vector3(
                        x * cellSize.x + cellSize.x / 2,
                        y * cellSize.y + cellSize.y / 2,
                        0
                    );
                    Gizmos.DrawWireCube(cellPosition, new Vector3(cellSize.x, cellSize.y, 1f));
                }
            }
        }
    }
}
