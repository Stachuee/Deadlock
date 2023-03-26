using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fuse : MonoBehaviour
{

    bool on;

    [SerializeField] Sprite onSprite;
    [SerializeField] Sprite offSprite;

    FuseBoxUIScript fuseBox;
    [SerializeField] Image button;

    public string segmentName;

    //private void Awake()
    //{
    //    button = transform.GetComponent<Image>();
    //}

    public void SetFuse(string segmentName, FuseBoxUIScript fuseBox)
    {
        this.segmentName = segmentName;
        this.fuseBox = fuseBox;
    }

    public void TurnFuse(bool state)
    {
        on = state;
        if (on)
        {
            button.sprite = onSprite;
        }
        else
        {
            button.sprite = offSprite;
        }
    }

    public void Use()
    {
        on = !on;
        fuseBox.SwitchFuse(segmentName, on);
        if (on)
        {
            button.sprite = onSprite;
        }
        else
        {
            button.sprite = offSprite;
        }

    }
}
