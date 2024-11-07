using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Inventory_UI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public Player player;
    public List<Slot_UI> slots = new List<Slot_UI>();
    public List<Button> iconButtons = new List<Button>();
    public static Inventory_UI inventory_UI;
    public bool areListenersAdded = false;
    public bool show = false;
    public int clickedSaveIndex;
    public int seedIndex = 0;
    private UnityAction<int> buttonClickedAction;
    private BuildManager buildManager;
    private UIManager uiManager;
    private DealerUI dealerUI;
    private void Awake()
    {
        if (inventory_UI == null)
        {
            inventory_UI = this;
        }
        buildManager = FindObjectOfType<BuildManager>();
        if (buildManager == null)
        {
            buildManager = BuildManager.buildManager; 
        }
        uiManager = FindObjectOfType<UIManager>();
        if(uiManager == null)
        {
            uiManager = UIManager.uiManager;
        }
        dealerUI = FindObjectOfType<DealerUI>();
        if(dealerUI == null)
        {
            dealerUI = DealerUI.dealerUI;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !uiManager.IsSpecificPanelsOpen
            (buildManager.buildingPanel, dealerUI.dealerUIPanel))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        if (!inventoryPanel.activeSelf && !areListenersAdded)
        {
            inventoryPanel.SetActive(true);
            Refresh();
            RemoveButtonListeners();
            AddButtonListeners();
            TimeManager.timeManager.PauseTime();
        }
        else if (inventoryPanel.activeSelf && areListenersAdded)
        {
            RemoveButtonListeners();
            inventoryPanel.SetActive(false);
            CountPanel.countPanel.HidePanel();
            TimeManager.timeManager.ResumeTime();
        }
    }

    public void Refresh()
    {
        if (slots.Count == player.inventory.slots.Count)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (player.inventory.slots[i].type != CollectibleType.None)
                {
                    slots[i].SetItem(player.inventory.slots[i]);
                }
                else
                {
                    slots[i].SetEmpty();
                    for (int j = i + 1; j < player.inventory.slots.Count; j++)
                    {
                        if (player.inventory.slots[j].type != CollectibleType.None)
                        {
                            player.inventory.slots[i].type = player.inventory.slots[j].type;
                            player.inventory.slots[i].icon = player.inventory.slots[j].icon;
                            player.inventory.slots[i].itemCount = player.inventory.slots[j].itemCount;

                            player.inventory.slots[j].type = CollectibleType.None;
                            player.inventory.slots[j].icon = null;
                            player.inventory.slots[j].itemCount = 0;

                            break;
                        }
                    }
                }
            }

            for (int i = 0; i < slots.Count; i++)
            {
                if (player.inventory.slots[i].type != CollectibleType.None)
                {
                    slots[i].SetItem(player.inventory.slots[i]);
                }
                else
                {
                    slots[i].SetEmpty();
                }
            }
        }
    }

    public void Remove(int slotID)
    {
        Collectables itemToDrop = GameManager.instance.itemManager.GetItemByType(player.inventory.slots[slotID].type);
        if (itemToDrop != null)
        {
            CountPanel.countPanel.ShowPanelAtSlot(slots[slotID].slotRectTransform, player.inventory.slots[slotID].itemCount);
            CountPanel.countPanel.acceptAction = () =>
            {
                int dropCount = CountPanel.countPanel.count;
                for (int i = 0; i < dropCount; i++)
                {
                    player.DropItem(itemToDrop);
                }
                player.inventory.Remove(slotID, dropCount);
                Refresh();
            };
        }
    }

    private void AddButtonListeners()
    {
        if (!areListenersAdded)
        {
            for (int i = 0; i < iconButtons.Count; i++)
            {
                int index = i;
                Button button = iconButtons[index];
                buttonClickedAction = OnButtonClicked;
                button.onClick.AddListener(() => buttonClickedAction(index));
            }
            areListenersAdded = true;
        }
    }

    private void RemoveButtonListeners()
    {
        if (areListenersAdded)
        {
            for (int i = 0; i < iconButtons.Count; i++)
            {
                int index = i;
                Button button = iconButtons[index];
                button.onClick.RemoveListener(() => buttonClickedAction(index));
            }
            areListenersAdded = false;
        }
    }

    private void OnButtonClicked(int index)
    {
        seedIndex = 0;
        clickedSaveIndex = index;
        Image iconImage = iconButtons[index].GetComponentInChildren<Image>();
        if (iconImage.sprite != null)
        {
            foreach (SeedData si in SeedInfo.seedInfo.seedDataList)
            {
                if (iconImage.sprite.name == si.seedType.name)
                {
                    if (!show)
                    {
                        show = true;
                        ToggleInventory();
                    }
                    break;
                }
                else
                {
                    seedIndex++;
                }
            }
        }
    }
}
