using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlantHole : MonoBehaviour
{
    public Tilemap topTilemap;
    public Tilemap bottomTilemap;
    public Tile plantableTile;
    public Tile plantHoleTile;
    private BoundsInt interactableBounds;
    private bool isPlanting;

    void Update()
    {
        UpdateInteractableBounds();
        if (Input.GetMouseButton(0) && !PlayerMovement.playerMovement.animator.GetBool("isMoving"))
        {
            Vector3Int cellPosition = GetCurrentGridCell();
            if (CanPlant(cellPosition))
            {
                if (!isPlanting)
                {
                    PlayerMovement.playerMovement.animator.SetBool("isDigging", true);
                    isPlanting = true;
                }

                AnimatorStateInfo stateInfo = PlayerMovement.playerMovement.animator.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.IsName("digging") && stateInfo.normalizedTime >= 1.0f)
                {
                    SetTile(cellPosition, plantHoleTile);
                    PlayerMovement.playerMovement.animator.SetBool("isDigging", false);
                    isPlanting = false;
                }
            }
            else
            {
                if (isPlanting)
                {
                    PlayerMovement.playerMovement.animator.SetBool("isDigging", false);
                    isPlanting = false;
                }
            }
        }
        else
        {
            if (isPlanting)
            {
                PlayerMovement.playerMovement.animator.SetBool("isDigging", false);
                isPlanting = false;
            }
        }
    }
    private void UpdateInteractableBounds()
    {
        Vector3 playerPosition = PlayerMovement.playerMovement.transform.position;
        Vector3Int playerCell = topTilemap.WorldToCell(playerPosition);
        interactableBounds = new BoundsInt(playerCell.x - 1, playerCell.y - 1, 0, 3, 3, 1);
    }
    private Vector3Int GetCurrentGridCell()
    {
        Vector3 playerPosition = PlayerMovement.playerMovement.transform.position;
        return topTilemap.WorldToCell(playerPosition);
    }
    private bool CanPlant(Vector3Int cellPosition)
    {
        TileBase check_tile = topTilemap.GetTile(cellPosition);

        if (check_tile == null)
        {
            check_tile = bottomTilemap.GetTile(cellPosition);
        }
        Collider2D[] colliders = Physics2D.OverlapBoxAll(topTilemap.GetCellCenterWorld(cellPosition), new Vector2(1, 1), 0f);
        bool hasInvalidCollider = false;

        foreach (var collider in colliders)
        {
            if (!collider.CompareTag("Player") && !collider.CompareTag("Collectables") && !collider.CompareTag("Untagged"))
            {
                hasInvalidCollider = true;
                break;
            }
        }

        return (check_tile == plantableTile) && !Plowing.plowing.showGrid 
            && !Plowing.plowing.showUGrid && !hasInvalidCollider && !Inventory_UI.inventory_UI.show;
    }
    private void SetTile(Vector3Int cellPosition, Tile tile)
    {
        topTilemap.SetTile(cellPosition, tile);
    }
}
