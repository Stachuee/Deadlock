using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyComputerIndicators : MonoBehaviour
{

    Image myImage;

    [SerializeField] Sprite powered;
    [SerializeField] Sprite notPowered;

    private void Awake()
    {
        myImage = GetComponent<Image>();
    }

    public void UpdateIndicator(bool powered)
    {
        if(powered)
        {
            myImage.sprite = this.powered;
        }
        else
        {
            myImage.sprite = this.notPowered;
        }
    }
}
