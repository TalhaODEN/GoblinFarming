using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slot_UI : MonoBehaviour
{
    public Image itemIcon;
    public TextMeshProUGUI quantityText;
    public RectTransform slotRectTransform;

    [Header("Tooltip")]
    public GameObject tooltipObject;
    public TextMeshProUGUI tooltipText;

    private Inventory.Slot currentSlot;
    private bool tooltipActive = false; // Tooltip'in aktif olup olmadığını kontrol eder
    private float hoverTime = 0f;
    private const float hoverDelay = 1.1f;
    private Vector3 lastMousePosition;


    private void Update()
    {

        // Fare ekran koordinatlarını al
        Vector2 mousePosition = Input.mousePosition;

        // Eğer fare slotun içinde ise ve slot boş değilse
        if (IsMouseOverSlot(mousePosition) && currentSlot != null && currentSlot.type != CollectibleType.None)
        {
            if (!tooltipActive)
            {
                hoverTime += Time.unscaledDeltaTime;

                if (hoverTime >= hoverDelay)
                {
                    ShowTooltip(mousePosition); // Tooltip'i göster
                    tooltipActive = true; // Tooltip aktif edildi
                }
            }
        }
        else
        {
            if (tooltipActive)
            {
                HideTooltip(); // Fare slot dışında ise tooltip'i gizle
                tooltipActive = false; // Tooltip artık aktif değil
            }
            hoverTime = 0f; // Fare slot dışında olduğu için hover süresi sıfırlanır
        }
    }

    // Fare, slot recttransform'ının içinde mi?
    private bool IsMouseOverSlot(Vector2 mousePosition)
    {
        // Fare pozisyonunu slotun ekran koordinatları ile karşılaştırır
        return RectTransformUtility.RectangleContainsScreenPoint(slotRectTransform, mousePosition);
    }

    public void SetItem(Inventory.Slot slot)
    {
        if (slot != null)
        {
            currentSlot = slot;
            itemIcon.sprite = slot.icon;
            itemIcon.color = new Color(1, 1, 1, 1);
            quantityText.text = slot.itemCount.ToString();
            itemIcon.raycastTarget = true;
        }
    }

    public void SetEmpty()
    {
        currentSlot = null;
        itemIcon.sprite = null;
        itemIcon.color = new Color(1, 1, 1, 0);
        quantityText.text = "";
        itemIcon.raycastTarget = false;
        tooltipActive = false; // Tooltip boş slot olduğunda gizlenir
    }

    private void ShowTooltip(Vector2 mousePosition)
    {
        tooltipObject.SetActive(true); // Tooltip'i aktif et
        tooltipText.text = currentSlot.type.ToString(); // Tooltip metnini ayarla
        tooltipObject.transform.position = mousePosition + new Vector2(15, -15); // Tooltip'in pozisyonunu ayarla
    }

    private void HideTooltip()
    {
        tooltipObject.SetActive(false); // Tooltip'i gizle
    }
}
