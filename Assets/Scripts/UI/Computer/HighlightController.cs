using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HighlightController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] Image hpSlider;
    [SerializeField] Image powered;
    [SerializeField] Image progress;
    IInteractable toMonitor;

    [SerializeField] Sprite powerOn;
    [SerializeField] Sprite powerOff;

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
            hpSlider.fillAmount = info.hp / info.maxHp;
        }
        if (info.showCharge)
        {
            powered.sprite = info.charged ? powerOn : powerOff;
        }
        if(info.showProgress)
        {
            progress.fillAmount = info.progress;   
        }
        hpSlider.gameObject.SetActive(info.showHp);
        powered.gameObject.SetActive(info.showCharge);
        progress.gameObject.SetActive(info.showProgress);
    }
}
