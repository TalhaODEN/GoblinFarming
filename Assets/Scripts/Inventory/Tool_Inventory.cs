using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tool_Inventory : MonoBehaviour
{
    public List<GameObject> tools;
    public int currentToolIndex;
    public static Tool_Inventory tool_inventory;
    public List<Image> toolButtons;

    private void Awake()
    {
        if (tool_inventory == null)
        {
            tool_inventory = this;
        }
        currentToolIndex = 3;
        UpdateToolUI();
    }

    private void Update()
    {
        if (!UIManager.uiManager.IsAnyPanelOpen())
        {
            HandleToolSwitching();
            UpdateTool();
            UpdateToolUI();
        }
    }

    public void SelectTool(int toolIndex)
    {
        if (!IsAnyAnimationPlaying() && !UIManager.uiManager.IsAnyPanelOpen())
        {
            if (toolIndex == 3)
            {
                currentToolIndex = toolIndex;
                DeactivateAllTools();
            }
            else if (toolIndex >= 0 && toolIndex < tools.Count)
            {
                currentToolIndex = toolIndex;
                UpdateTool();
                UpdateToolUI();
            }
        }
    }

    private void HandleToolSwitching()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (!IsAnyAnimationPlaying())
        {
            if (scrollInput > 0f)
            {
                currentToolIndex = (currentToolIndex + 1) % (tools.Count + 1);
            }
            else if (scrollInput < 0f)
            {
                currentToolIndex = (currentToolIndex - 1 + tools.Count + 1) % (tools.Count + 1);
            }
            KeyCode[] keyCodes = { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4 };

            for (int i = 0; i < keyCodes.Length; i++)
            {
                if (Input.GetKeyDown(keyCodes[i])) 
                {
                    currentToolIndex = i;
                    break;
                }
            }
        }
    }

    private bool IsAnyAnimationPlaying()
    {
        return PlayerMovement.playerMovement.animator.GetBool("isAttacking") ||
               PlayerMovement.playerMovement.animator.GetBool("isAxeHitting") ||
               PlayerMovement.playerMovement.animator.GetBool("isMining") ||
               PlayerMovement.playerMovement.animator.GetBool("isDigging");
    }

    public void DeactivateAllTools()
    {
        foreach (var tool in tools)
        {
            tool.SetActive(false);
        }
    }

    private void UpdateTool()
    {
        if (!IsAnyAnimationPlaying())
        {
            for (int i = 0; i < tools.Count; i++)
            {
                tools[i].SetActive(i == currentToolIndex);
            }
        }
    }

    private void UpdateToolUI()
    {
        for (int i = 0; i < toolButtons.Count; i++)
        {
            toolButtons[i].type = (i == currentToolIndex) ? Image.Type.Filled : Image.Type.Sliced;
        }
    }

    public bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
