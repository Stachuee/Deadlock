using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI itemName;

    public void SetupInfo(HandInfoContainer infoContainer)
    {
        if(!infoContainer.show)
        {
            HideInfo();
            return;
        }

        itemName.text = infoContainer.name;
        gameObject.SetActive(true);
    }

    public void HideInfo()
    {
        gameObject.SetActive(false);
    }
}
