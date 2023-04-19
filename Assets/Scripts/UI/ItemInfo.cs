using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI itemName;
    [SerializeField]
    Image itemSprite;
    [SerializeField]
    Image itemType;
    [SerializeField]
    Image itemSubtype;

    public void SetupInfo(ItemSO item)
    {
        itemName.text = item.name;
        itemSprite.sprite = item.GetIconSprite();
        itemType.sprite = item.GetTypeIcon();
        itemSubtype.sprite = item.GetSubtypeIcon();
    }
}
