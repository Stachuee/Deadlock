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
    [SerializeField] Slider progress;
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

    public void UpdateHighlight(ComputerInfoContainer info)
    {
        nameText.text = info.name;
        if (info.showHp)
        {
            hpSlider.value = info.hp / info.maxHp;
        }
        if (info.showCharge)
        {
            powered.color = info.charged ? Color.white : Color.yellow;
        }
        if(info.showProgress)
        {
            progress.value = info.progress;   
        }
        hpSlider.gameObject.SetActive(info.showHp);
        powered.gameObject.SetActive(info.showCharge);
        progress.gameObject.SetActive(info.showProgress);
    }
}
