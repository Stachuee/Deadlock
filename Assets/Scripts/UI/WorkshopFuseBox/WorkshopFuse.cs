using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkshopFuse : MonoBehaviour
{
    bool isOn;

    bool IsOn
    {
        get { return isOn; }
        set { 
            if(isOn != value)
            {
                isOn = value;
                if(isOn)
                {
                    fuse.image.sprite = on;
                }
                else
                {
                    fuse.image.sprite = off;
                }
            }
        }
    }


    [SerializeField] Button fuse;
    [SerializeField] Sprite on;
    [SerializeField] Sprite off;
    [SerializeField] PlayerController player;
    
    [SerializeField]
    Vector2 gaugeAngle;
    [SerializeField] Transform gaugeArrow;
    [SerializeField] Transform overchargeGaugeArrow;

    [SerializeField] GameObject fuseBoxUI;

    bool open;

    private void Update()
    {
        if(open)
        {
            float angle = (gaugeAngle.y - gaugeAngle.x) * Mathf.Min(ElectricityController.fusesActive / (float)ElectricityController.maxFusesActive, 1.2f); // max overloaded arrow 
            gaugeArrow.rotation = Quaternion.Euler(0, 0, gaugeAngle.x + angle);
            angle = (gaugeAngle.y - gaugeAngle.x) * ElectricityController.overcharge / ElectricityController.maxOvercharge;
            overchargeGaugeArrow.rotation = Quaternion.Euler(0, 0, gaugeAngle.x + angle);
        }
    }

    public void ChangeFuseState()
    {
        if (ElectricityController.Overloaded) return;
        IsOn = !IsOn;
        PowerPlug.Instance.TurnOn(IsOn);
    }

    public void RefreshFuse()
    {
        IsOn = ElectricityController.workshopPowered;
    }

    public void Open()
    {
        IsOn = ElectricityController.workshopPowered;
        player.uiController.myEventSystem.SetSelectedGameObject(fuse.gameObject);
        open = true;
        fuseBoxUI.gameObject.SetActive(true);
    }
    public void Close()
    {
        open = false;
        fuseBoxUI.gameObject.SetActive(false);
    }
}
