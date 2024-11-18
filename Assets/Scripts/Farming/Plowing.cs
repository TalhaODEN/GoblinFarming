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

    public GameObject whiteGridPrefab;  // White grid prefab
    public GameObject blueGridPrefab;   // Blue grid prefab

    private GameObject currentGridObject; // To hold the instantiated grid

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

            // Get the player's current tile position and adjust to the left by one tile
            Vector3Int playerTilePosition = topTilemap.WorldToCell(playerLastPosition);
            Vector3Int leftTilePosition = playerTilePosition + new Vector3Int(-1, 0, 0);

            // Convert this adjusted tile position to world coordinates
            Vector3 adjustedPosition = topTilemap.GetCellCenterWorld(leftTilePosition);

            // Apply a 0.5 offset to the x-coordinate to correct the 0.5 unit shift
            adjustedPosition.x -= 0.5f;

            // Instantiate the correct grid prefab based on the state
            if (currentGridObject != null)
                Destroy(currentGridObject); // Destroy the previous grid

            if (showGrid)
            {
                currentGridObject = Instantiate(whiteGridPrefab, adjustedPosition, Quaternion.identity);
            }
            else if (showUGrid)
            {
                currentGridObject = Instantiate(blueGridPrefab, adjustedPosition, Quaternion.identity);
            }
        }
        else
        {
            if (currentGridObject != null)
            {
                Destroy(currentGridObject); // Destroy the grid when it is no longer visible
            }
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
}
