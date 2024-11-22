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
    [SerializeField] private Sprite selectedSprite;
    [SerializeField] private Sprite normalSprite;

    private void Awake()
    {
        if (tool_inventory == null)
        {
            tool_inventory = this;
        }
        currentToolIndex = toolButtons.Count - 1;
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
            if (toolIndex >= tools.Count)
            {
                currentToolIndex = toolIndex;
                DeactivateAllTools();
            }
            else if (toolIndex >= 0)
            {
                currentToolIndex = toolIndex;
                UpdateTool();
            }
            UpdateToolUI();
        }
    }

    private void HandleToolSwitching()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (!IsAnyAnimationPlaying())
        {
            if (scrollInput > 0f)
            {
                currentToolIndex = (currentToolIndex + 1) % (toolButtons.Count);
            }
            else if (scrollInput < 0f)
            {
                currentToolIndex = (currentToolIndex - 1 + toolButtons.Count) % (toolButtons.Count);
            }
            KeyCode[] keyCodes = { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5 };

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
               PlayerMovement.playerMovement.animator.GetBool("isDigging") ||
               PlayerMovement.playerMovement.animator.GetBool("isWatering");
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
            toolButtons[i].GetComponent<Button>().interactable = false;
            toolButtons[i].GetComponent<Button>().interactable = true;
        }

        for (int i = 0; i < toolButtons.Count; i++)
        {
            if (i == currentToolIndex) 
            {
                toolButtons[i].sprite = selectedSprite;
            }
            else
            {
                toolButtons[i].sprite = normalSprite;
            }
        }
    }

    public bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
