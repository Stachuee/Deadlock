using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySelector : MonoBehaviour
{
    [SerializeField] List<GunMenuButton> slotButtons = new List<GunMenuButton>();
    private Vector2 mousePos;
    private Vector2 fromVector2M = new Vector2(0.5f, 1.0f);
    private Vector2 centerCircle = new Vector2(0.5f, 0.5f);
    private Vector2 toVector2M;

    private int gunMenuItemsAmount;
    private int currentSelectedItem;
    private int previousSelectedItem;

    [SerializeField]
    private GameObject player;

    private PlayerController pC;
    private GunController gC;
    private EquipmentController eC;

    private void Start()
    {
        pC = player.GetComponent<PlayerController>();
        gC = player.GetComponent<GunController>();
        eC = player.GetComponent<EquipmentController>();

        gunMenuItemsAmount = slotButtons.Count;

        foreach(GunMenuButton gB in slotButtons)
        {
            gB.image.color = gB.NormalColor;
        }

        currentSelectedItem = 0;
        previousSelectedItem = 0;
    }

    private void Update()
    {
        GetCurrentSelectedItem();
    }

    public void GetCurrentSelectedItem() 
    {
        mousePos = pC.currentAimDirection;

        toVector2M = mousePos.normalized;


        float angle = (Mathf.Atan2(fromVector2M.y - centerCircle.y, fromVector2M.x - centerCircle.x) - Mathf.Atan2(toVector2M.y - centerCircle.y, toVector2M.x - centerCircle.x)) * Mathf.Rad2Deg;

        if (angle < 0)
            angle += 360;


        currentSelectedItem = (int)(angle / (360 / gunMenuItemsAmount));

        if (currentSelectedItem != previousSelectedItem)
        {
            slotButtons[previousSelectedItem].image.color = slotButtons[previousSelectedItem].NormalColor;
            previousSelectedItem = currentSelectedItem;
            slotButtons[currentSelectedItem].image.color = slotButtons[currentSelectedItem].HoverColor;
        }
    }

    public void ChangePlayerSlot()
    {
        slotButtons[currentSelectedItem].image.color = slotButtons[currentSelectedItem].PressedColor;
        if (!slotButtons[currentSelectedItem].isActive || slotButtons[currentSelectedItem].isEmpty) return;

        if (slotButtons[currentSelectedItem].slotType == SlotType.Weapon) gC.ChangeWeapon(slotButtons[currentSelectedItem].id);
        else if (slotButtons[currentSelectedItem].slotType == SlotType.Equipment) eC.ChangeEquipment(slotButtons[currentSelectedItem].id);
    }


    public void ActivateSlot(SlotType sT, int _id)
    {
        if (sT == SlotType.Weapon)
        {
            for (int i = 0; i < slotButtons.Count; i++)
            {
                if (slotButtons[i].slotType == SlotType.Weapon && slotButtons[i].id == _id)
                {
                    slotButtons[i].isActive = true;
                    break;
                }
            }
        }
        else if (sT == SlotType.Equipment)
        {
            for (int i = 0; i < slotButtons.Count; i++)
            {
                if (slotButtons[i].slotType == SlotType.Equipment && slotButtons[i].id == _id)
                {
                    slotButtons[i].isActive = true;
                    break;
                }
            }
        }
    }

    public EquipmentController GetEController()
    {
        return eC;
    }
    /*
    public void AddSlot(SlotType sT, int _id)
    {
        if(sT == SlotType.Weapon)
        {
            for(int i = 0; i < slotButtons.Count; i++)
            {
                if (slotButtons[i].slotType == SlotType.Equipment) Debug.LogError("No free slot for weapon");
                if (slotButtons[i].isEmpty)
                {
                    slotButtons[i].id = _id;
                    slotButtons[i].isEmpty = false;
                    break;
                }
            }
        }else if (sT == SlotType.Equipment)
        {
            for (int i = slotButtons.Count - 1; i >= 0; i--)
            {
                if (slotButtons[i].slotType == SlotType.Weapon) Debug.LogError("No free slot for equipment");
                if (slotButtons[i].isEmpty)
                {
                    slotButtons[i].id = _id;
                    slotButtons[i].isEmpty = false;
                    break;
                }
            }
        }
    }*/
}



public enum SlotType { Weapon, Equipment};

[System.Serializable]
public class GunMenuButton
{
    public int id;
    public Image image;
    public Color NormalColor = Color.white;
    public Color HoverColor = Color.gray;
    public Color PressedColor = Color.gray;
    public bool isEmpty = true;
    public bool isActive = false;

    public SlotType slotType;

}