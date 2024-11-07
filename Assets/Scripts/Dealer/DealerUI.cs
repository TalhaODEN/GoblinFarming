using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DealerUI : MonoBehaviour
{
    public GameObject dealerUIPanel;
    public Button closeButton;
    public static DealerUI dealerUI;
    private bool isDealerUIActive = false;

    private void Awake()
    {
        if (dealerUI == null)
        {
            dealerUI = this;
        }
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseDealerUI);
        }
    }

    public void ToggleDealerUI()
    {
        if (IsDealerUIActive())
        {
            CloseDealerUI();
        }
        else
        {
            OpenDealerUI();
        }
    }

    public void OpenDealerUI()
    {
        TimeManager.timeManager.PauseTime();
        dealerUIPanel.SetActive(true);
        isDealerUIActive = true;
        DealerInventoryUI.dealerInventoryUI.RefreshInventory();
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseDealerUI);
        }
    }

    public void CloseDealerUI()
    {
        DealerInventoryUI.dealerInventoryUI.CancelButton();
        dealerUIPanel.SetActive(false);
        isDealerUIActive = false;
        TimeManager.timeManager.ResumeTime();
        if (closeButton != null)
        {
            closeButton.onClick.RemoveListener(CloseDealerUI);
        }
    }

    public bool IsDealerUIActive()
    {
        return isDealerUIActive;
    }
}
