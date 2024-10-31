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


    private void Awake()
    {
        if(plowing == null)
        {
            plowing = this;
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (showUGrid)
            {
                showUGrid = false;
            }
            showGrid = !showGrid; 

            if (showGrid)
            {
                playerLastPosition = player.position; 
                UpdateInteractableBounds(); 
            }
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            if (showGrid)
            {
                showGrid = false;
            }

            showUGrid = !showUGrid; 

            if (showUGrid)
            {
                playerLastPosition = player.position; 
                UpdateInteractableBounds(); 
            }
        }

        if (showGrid && Input.GetMouseButtonDown(0))
        {
            PlaceTile();
        }

        if (showUGrid && Input.GetMouseButtonDown(0))
        {
            ConvertTile();
        }
    }

    private void PlaceTile()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = topTilemap.WorldToCell(mouseWorldPosition); 

        Collider2D[] colliders = Physics2D.OverlapBoxAll(topTilemap.GetCellCenterWorld(cellPosition), new Vector2(1, 1), 0f);

        foreach (var collider in colliders)
        {
            if (!collider.CompareTag("Player") && !collider.CompareTag("Collectables") && !collider.CompareTag("Untagged"))
            {
                return;
            }
        }

        if (interactableBounds.Contains(cellPosition))
        {
            Tile clickedTile = topTilemap.GetTile<Tile>(cellPosition); 
            if (plowableTiles.Contains(clickedTile)) 
            {
                topTilemap.SetTile(cellPosition, plowingTile); 
                return; 
            }
        }

        cellPosition = bottomTilemap.WorldToCell(mouseWorldPosition); 
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
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = topTilemap.WorldToCell(mouseWorldPosition); 

        Collider2D[] colliders = Physics2D.OverlapBoxAll(topTilemap.GetCellCenterWorld(cellPosition), new Vector2(1, 1), 0f);

        foreach (var collider in colliders)
        {
            if (!collider.CompareTag("Player") && !collider.CompareTag("Collectables") && !collider.CompareTag("Untagged"))
            {
                return;
            }
        }

        if (interactableBounds.Contains(cellPosition))
        {
            Tile clickedTile = topTilemap.GetTile<Tile>(cellPosition); 
            if (unplowableTiles.Contains(clickedTile)) 
            {
                topTilemap.SetTile(cellPosition, unplowingTile); 
                return; 
            }
        }

        cellPosition = bottomTilemap.WorldToCell(mouseWorldPosition); 
        if (interactableBounds.Contains(cellPosition))
        {
            Tile clickedTile = bottomTilemap.GetTile<Tile>(cellPosition); 
            if (unplowableTiles.Contains(clickedTile)) 
            {
                bottomTilemap.SetTile(cellPosition, unplowingTile); 
            }
        }
    }

    private void UpdateInteractableBounds()
    {
        interactableBounds = new BoundsInt(topTilemap.WorldToCell(playerLastPosition) + new Vector3Int(-1, -1, 0), new Vector3Int(3, 3, 1)); 
    }

    private void OnDrawGizmos()
    {
        if (showGrid || showUGrid) 
        {
            if (showGrid)
            {
                Gizmos.color = Color.white;
            }
            else if (showUGrid)
            {
                Gizmos.color = Color.blue;
            }

            BoundsInt bounds = topTilemap.cellBounds;
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
