using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoosedItem : MonoBehaviour
{
    public List<Image> ShopItemImages = new List<Image>();
    public List<Image> PlayerItemImages = new List<Image>();
    public static ChoosedItem choosedItem;

    private void Awake()
    {
        if(choosedItem == null)
        {
            choosedItem = this;
        }
    }
}
