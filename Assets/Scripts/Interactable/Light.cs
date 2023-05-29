using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Light : PoweredInteractable
{
    Light2D myLight;

    bool turnOn;

    [SerializeField] bool inverted;

    protected override void Awake()
    {
        base.Awake();
        myLight = transform.GetComponent<Light2D>();
        AddAction(TurnOnOffByComputer);
    }

    public void TurnOnOffByComputer(PlayerController player)
    {
        turnOn = !turnOn;
    }

    public override void Highlight()
    {
        
    }

    public override void UnHighlight()
    {
        
    }

    override public void PowerOn(bool on)
    {
        //if (!turnOn) return;
        if(on)
        {
            myLight.enabled = true ^ inverted;
        }
        else
        {
            myLight.enabled = false ^ inverted;
        }
    }
}
