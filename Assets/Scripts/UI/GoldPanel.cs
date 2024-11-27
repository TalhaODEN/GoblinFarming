using UnityEngine;
using TMPro;

public class GoldPanel : MonoBehaviour
{
    public TextMeshProUGUI goldText;
    public static int goldAmount;
    public static GoldPanel goldPanel;

    private void Awake()
    {
        if (goldPanel == null)
        {
            goldPanel = this;
        }
        SetGoldAmount(0);
    }

    public void SetGoldAmount(int amount)
    {
        goldAmount = amount;
        UpdateGoldText();
    }

    public void AddGold(int amount)
    {
        goldAmount += amount;
        UpdateGoldText();
    }

    public bool RemoveGold(int amount) 
    {
        if (goldAmount >= amount) 
        {
            goldAmount -= amount;
            UpdateGoldText();
            return true; 
        }
        return false; 
    }

    private void UpdateGoldText()
    {
        goldText.text = goldAmount.ToString();
    }
}
