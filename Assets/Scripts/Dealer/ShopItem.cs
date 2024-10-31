using UnityEngine;
using UnityEngine.UI;


public class ShopItem : MonoBehaviour
{
    public CollectibleType itemType;
    public int price;
    public Sprite itemIcon;
    public Image slotImage;

    private void Start()
    {
        AssignToSlot();
    }
    public void AssignToSlot()
    {
        if (itemType != CollectibleType.None && itemIcon != null)
        {
            slotImage.sprite = itemIcon;
            slotImage.color = new Color(255, 255, 255, 255);
            slotImage.raycastTarget = true;
        }
        else
        {
            slotImage.color = new Color(255, 255, 255, 0);
            slotImage.sprite = null;
            slotImage.raycastTarget = false;
        }
    }





}
