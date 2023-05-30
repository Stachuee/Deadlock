using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySelector : MonoBehaviour, IControllSubscriberMovment
{
    [SerializeField] List<GunMenuButton> slotButtons = new List<GunMenuButton>();

    [SerializeField] GameObject inventoryPanel;
    private Vector2 mousePos;
    private Vector2 fromVector2M = new Vector2(0.5f, 1.0f);
    private Vector2 centerCircle = new Vector2(0.5f, 0.5f);
    private Vector2 toVector2M;

    private int gunMenuItemsAmount;
    private int currentSelectedItem;
    private int previousSelectedItem;

    [SerializeField]
    private PlayerController pC;

    private GunController gC;
    private EquipmentController eC;

    bool open;
    bool active = true;


    private void Awake()
    {
        foreach (GunMenuButton gB in slotButtons)
        {
            gB.Hide();
        }
    }

    private void Start()
    {
        if(pC.isScientist)
        {
            active = false;
            return;
        }
        gunMenuItemsAmount = slotButtons.Count;
        gC = pC.gunController;
        eC = pC.equipmentController;


        currentSelectedItem = -1;
        previousSelectedItem = -1;
    }

    public void OpenInventory()
    {
        if (open || !active) return;
        inventoryPanel.SetActive(true);
        RefreshAmmo();
        pC.AddMovmentSubscriber(this);
        open = true;
    }

    public void ForwardCommandMovment(Vector2 controll)
    {
        if (controll.magnitude < 0.2f)
        {
            currentSelectedItem = -1;
            if(previousSelectedItem != -1)
            {
                slotButtons[previousSelectedItem].image.color = slotButtons[previousSelectedItem].NormalColor;
                previousSelectedItem = -1;
            }
            return;
        }

        float rot_z = -(Mathf.Atan2(controll.y, controll.x) * Mathf.Rad2Deg - 90);

        if (rot_z < 0)
            rot_z += 360;

        currentSelectedItem = Mathf.Clamp(Mathf.FloorToInt(rot_z / (360 / gunMenuItemsAmount)), 0, gunMenuItemsAmount - 1);

        if (currentSelectedItem != previousSelectedItem)
        {
            if(previousSelectedItem != -1) slotButtons[previousSelectedItem].image.color = slotButtons[previousSelectedItem].NormalColor;
            previousSelectedItem = currentSelectedItem;
            slotButtons[currentSelectedItem].image.color = slotButtons[currentSelectedItem].HoverColor;
        }
    }

    public void RefreshAmmo()
    {
        slotButtons.ForEach(button =>
        {
            if(!button.isEmpty)
            {
                if (button.slotType == SlotType.Weapon)
                {
                    button.ammoCount.text = gC.GetAmmoString(button.weaponType);
                }
                else
                {
                    button.ammoCount.text = eC.GetEquipmentAmmo(button.equipmentType);
                }
            }
            else
            {
                button.ammoCount.text = "";
            }
        });
    }

    public void ChangePlayerSlot()
    {
        if (!open) return;
        if (currentSelectedItem >= 0)
        {
            slotButtons[currentSelectedItem].image.color = slotButtons[currentSelectedItem].PressedColor;
            if (slotButtons[currentSelectedItem].isActive && !slotButtons[currentSelectedItem].isEmpty)
            {
                if (slotButtons[currentSelectedItem].slotType == SlotType.Weapon) gC.ChangeWeapon(slotButtons[currentSelectedItem].weaponType);
                else if (slotButtons[currentSelectedItem].slotType == SlotType.Equipment) eC.ChangeEquipment(slotButtons[currentSelectedItem].equipmentType);
            }
        }
        inventoryPanel.SetActive(false);
        pC.RemoveMovmentSubscriber(this);
        open = false;
    }


    public void ActivateSlot(WeaponType weapon)
    {

        slotButtons.Find(slot => slot.weaponType == weapon && slot.slotType == SlotType.Weapon).Show();     
    }

    public void ActivateSlot(EquipmentType equipment)
    {
        slotButtons.Find(slot => slot.equipmentType == equipment && slot.slotType == SlotType.Equipment).Show();
    }

//    if (sT == SlotType.Weapon)
//        {
//            for (int i = 0; i<slotButtons.Count; i++)
//            {
//                if (slotButtons[i].slotType == SlotType.Weapon && slotButtons[i].id == _id)
//                {
//                    slotButtons[i].isActive = true;
//                    break;
//                }
//            }
//        }
//        else if (sT == SlotType.Equipment)
//{
//    for (int i = 0; i < slotButtons.Count; i++)
//    {
//        if (slotButtons[i].slotType == SlotType.Equipment && slotButtons[i].id == _id)
//        {
//            slotButtons[i].isActive = true;
//            break;
//        }
//    }
//}


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
    public Image image;
    public GameObject slot;
    public TextMeshProUGUI ammoCount;
    public Color NormalColor = Color.white;
    public Color HoverColor = Color.gray;
    public Color PressedColor = Color.gray;
    public bool isEmpty = true;
    public bool isActive = false;
    

    public SlotType slotType;

    public WeaponType weaponType;
    public EquipmentType equipmentType;

    public void Hide()
    {
        isActive = false;
        slot.SetActive(false);
    }

    public void Show()
    {
        isActive = true;
        slot.SetActive(true);
    }

}