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

    public void SetupInfo(HandInfoContainer infoContainer)
    {
        itemName.text = infoContainer.name;
        itemSprite.sprite = infoContainer.sprite;
        itemType.sprite = infoContainer.type;
        itemSubtype.sprite = infoContainer.subtype;
    }
}
