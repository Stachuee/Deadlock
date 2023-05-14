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
        if(!infoContainer.show)
        {
            HideInfo();
            return;
        }

        itemName.text = infoContainer.name;
        itemSprite.sprite = infoContainer.sprite;
        gameObject.SetActive(true);
    }

    public void HideInfo()
    {
        gameObject.SetActive(false);
    }
}
