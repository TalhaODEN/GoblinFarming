using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPreview : MonoBehaviour
{
    private bool isPreviewing = true;
    [SerializeField]private ParentAlphaController alphaController;
    [SerializeField]private Collider2D prefabCollider;

    public void StartPreview()
    {
        isPreviewing = true;

        if (alphaController != null)
        {
            alphaController = GetComponent<ParentAlphaController>();
            alphaController.alphaValue = 0.5f; 
            alphaController.SetAlpha(alphaController.alphaValue);
        }

        if (prefabCollider == null) 
        {
            prefabCollider = GetComponent<Collider2D>();
        }

        if (prefabCollider != null)
        {
            prefabCollider.enabled = false; 
        }
        else
        {
            Debug.LogError("PrefabCollider is missing!");
        }
    }


    private void Update()
    {
        if (isPreviewing)
        {
            UpdatePreviewPosition(); 

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CancelPreview(); 
            }
            else if (Input.GetMouseButtonDown(0))
            {
                TryPlaceBuilding(); 
            }
        }
    }

    private void UpdatePreviewPosition()
    {
        Vector3Int tilePosition = BuildManager.buildManager.GetTilePositionFromMouse();
        transform.position = BuildManager.buildManager.tilemap.GetCellCenterWorld(tilePosition);
    }

    private void TryPlaceBuilding()
    {
        Vector3Int tilePosition = BuildManager.buildManager.GetTilePositionFromMouse();
        if (prefabCollider != null)
        {
            prefabCollider.enabled = true; 
        }
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(transform.position, prefabCollider.bounds.size, 0f); 

        bool canPlace = true;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider != prefabCollider) 
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);

                if (distance <= 5.0f)
                {
                    canPlace = false;
                    break;
                }
            }
        }

        if (canPlace)
        {
            if (alphaController != null)
            {
                alphaController.alphaValue = 1.0f;
                alphaController.SetAlpha(alphaController.alphaValue);
            }

            if (prefabCollider != null)
            {
                prefabCollider.enabled = true; 
            }

            isPreviewing = false; 
        }
        else
        {
            if (alphaController != null)
            {
                alphaController.alphaValue = 0.5f;
                alphaController.SetAlpha(alphaController.alphaValue);
            }
            if (prefabCollider != null)
            {
                prefabCollider.enabled = false; 
            }
        }
    }


    private void CancelPreview()
    {
        isPreviewing = false;
        Destroy(gameObject); 
    }
}

