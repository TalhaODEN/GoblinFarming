using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watering : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private Tool_Inventory tool_inventory;
    private Planting planting;
    public static Watering watering;
    private bool isWatering = false;
    private bool isAnimationCompleted = false; 

    private void Awake()
    {
        if (watering == null)
        {
            watering = this;
        }
        if (playerMovement == null)
        {
            playerMovement = FindObjectOfType<PlayerMovement>();
        }
        if (tool_inventory == null)
        {
            tool_inventory = FindObjectOfType<Tool_Inventory>();
        }
        if (planting == null)
        {
            planting = FindObjectOfType<Planting>();
        }
    }

    private void Update()
    {
        ControlWatering();
    }

    public void ControlWatering()
    {
        if (CheckConditions() && !isWatering)
        {
            isWatering = true;
            playerMovement.animator.SetBool("isWatering", true);
            isAnimationCompleted = false;
            AudioManager.audioManager.WateringSound(isWatering);
        }

        if (isWatering && !isAnimationCompleted)
        {
            if (playerMovement.animator.GetCurrentAnimatorStateInfo(0).IsName("watering") &&
                playerMovement.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                isAnimationCompleted = true;
                AudioManager.audioManager.WateringSound(isAnimationCompleted);
                isWatering = false;
                playerMovement.animator.SetBool("isWatering", isWatering);
                WaterCurrentPlant();
                StartCoroutine(DelayedAction());
            }
        }

        if (!Input.GetMouseButton(0))
        {
            isWatering = false;
            playerMovement.animator.SetBool("isWatering", isWatering);
            isAnimationCompleted = false;
            AudioManager.audioManager.WateringSound(isAnimationCompleted);
        }
    }

    public bool CheckConditions()
    {
        return Input.GetMouseButton(0) &&
               tool_inventory.currentToolIndex == tool_inventory.toolButtons.Count - 2 &&
               !playerMovement.animator.GetBool("isMoving") &&
               !isWatering &&
               !tool_inventory.IsMouseOverUI();
    }

    public void WaterCurrentPlant()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = planting.topTilemap.WorldToCell(mouseWorldPosition);
        Vector3Int playerPosition = planting.topTilemap.WorldToCell(playerMovement.transform.position);

        bool isNearbyCell = cellPosition == playerPosition ||
                            (cellPosition == playerPosition + Vector3Int.right &&
                            playerMovement.transform.rotation.y == 0) ||
                            (cellPosition == playerPosition + Vector3Int.left &&
                            playerMovement.transform.rotation.y == -1) ||
                            cellPosition == playerPosition + Vector3Int.up ||
                            cellPosition == playerPosition + Vector3Int.down ||
                            (cellPosition == playerPosition + Vector3Int.right + Vector3Int.down &&
                            playerMovement.transform.rotation.y == 0) ||
                            (cellPosition == playerPosition + Vector3Int.left + Vector3Int.down &&
                            playerMovement.transform.rotation.y == -1) ||
                            (cellPosition == playerPosition + Vector3Int.right + Vector3Int.up &&
                            playerMovement.transform.rotation.y == 0) ||
                            (cellPosition == playerPosition + Vector3Int.left + Vector3Int.up &&
                            playerMovement.transform.rotation.y == -1);

        if (!isNearbyCell)
        {
            return;
        }

        foreach (var plantingInfo in planting.plantingInfos)
        {
            if (plantingInfo.tilePosition == cellPosition)
            {
                plantingInfo.WaterPlant();
                break;
            }
        }
    }
    private IEnumerator DelayedAction()
    {
        yield return new WaitForSeconds(2f);
    }

}
