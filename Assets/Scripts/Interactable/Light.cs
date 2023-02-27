using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Light : MonoBehaviour, PowerInterface
{
    Light2D myLight;

    private void Awake()
    {
        myLight = transform.GetComponent<Light2D>();    
    }

    public void PowerOn(bool on)
    {
        if(on)
        {
            myLight.intensity = 1;
        }
        else
        {
            myLight.intensity = 0;
        }
    }
}
