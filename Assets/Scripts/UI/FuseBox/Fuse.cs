using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Fuse : MonoBehaviour, IPointerEnterHandler
{

    bool on;

    [SerializeField] Sprite onSprite;
    [SerializeField] Sprite offSprite;

    FuseBoxUIScript fuseBox;
    [SerializeField] Image button;
    public string segmentName;

    UiController uiController;

    //private void Awake()
    //{
    //    button = transform.GetComponent<Image>();
    //}


    private void Start()
    {
        uiController = GetComponentInParent<UiController>();
    }

    public void SetFuse(string segmentName, FuseBoxUIScript fuseBox)
    {
        this.segmentName = segmentName;
        this.fuseBox = fuseBox;
    }

    public void TurnFuse(bool state)
    {
        if (ElectricityController.Overloaded) return;
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
        if (ElectricityController.Overloaded) return;
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(uiController.myEventSystem.currentSelectedGameObject != gameObject)
        {
            uiController.myEventSystem.SetSelectedGameObject(null);
        }
    }
}
