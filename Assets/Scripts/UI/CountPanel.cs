using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CountPanel : MonoBehaviour
{
    public int count;
    public TextMeshProUGUI countText;
    public GameObject CountPanelScreen;
    public static CountPanel countPanel;
    public Button increaseButton; 
    public Button decreaseButton;
    private bool isActive = false;
    public System.Action acceptAction;
    public int maxCount;

    private void Awake()
    {
        if (countPanel == null)
        {
            countPanel = this;
        }
        count = 0;
        maxCount = 0;
    }

    public void ShowPanelAtSlot(RectTransform slotTransform, int itemCount)
    {
        maxCount = itemCount;
        if (!isActive)
        {
            CountPanelScreen.SetActive(true);
            isActive = true;

            RectTransform panelRectTransform = CountPanelScreen.GetComponent<RectTransform>();
            panelRectTransform.position = slotTransform.position + new Vector3(40f, 40f, 0);


        }
    }

    public void HidePanel()
    {
        if (isActive)
        {

            CountPanelScreen.SetActive(false);
            isActive = false;
            count = 0;
            maxCount = 0;
            UpdateCountText();
        }
    }

    public void Increase()
    {
        if (count < maxCount) 
        {
            count++;
            UpdateCountText();
        }
    }

    public void Decrease()
    {
        if (count > 0)
        {
            count--;
            UpdateCountText();
        }
    }

    private void UpdateCountText()
    {
        countText.text = count.ToString();
    }

    public void Confirm()
    {
        acceptAction?.Invoke();
        HidePanel();
    }

    public void Cancel()
    {
        HidePanel();
    }
}
