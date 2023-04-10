using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HighlightController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] Slider hpSlider;
    [SerializeField] Image powered;

    IInteractable toMonitor;

    private void Update()
    {
        if (toMonitor != null)
        {
            UpdateHighlight(toMonitor.GetInfo());
        }
    }

    public void SetupHighlight(IInteractable toMonitor)
    {
        this.toMonitor = toMonitor;
    }

    public void UpdateHighlight(InfoContainer info)
    {
        nameText.text = info.name;
        if (info.showHp)
        {
            hpSlider.gameObject.SetActive(true);
            hpSlider.value = info.hp / info.maxHp;
        }
        if (info.showCharge)
        {
            powered.gameObject.SetActive(true);
            powered.color = info.charged ? Color.white : Color.yellow;
        }
    }
}
