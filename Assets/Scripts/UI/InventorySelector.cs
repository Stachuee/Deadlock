using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySelector : MonoBehaviour, IControllSubscriberAim
{
    [SerializeField] List<GunMenuButton> slotButtons = new List<GunMenuButton>();
    
    
    [SerializeField] List<GunMenuButton> mainSlotButtons = new List<GunMenuButton>();
    private int mainMenuItemsAmount;
    [SerializeField] List<GunMenuButton> weaponSlotButtons = new List<GunMenuButton>();
    private int weaponMenuItemsAmount;
    [SerializeField] List<GunMenuButton> equipmentSlotButtons = new List<GunMenuButton>();
    private int equipmentMenuItemsAmount;



    [SerializeField] float chooseCategoryTime;
    [SerializeField] float changeWeaponTime;
    float confirmChoose;
    float confirmWeapon;


    enum Menu {Main, Weapon, Equipment };


    Menu currentMenu;
    Menu CurrentMenu {
        get
        {
            return currentMenu;
        }
        set
        {
            if(value != currentMenu)
            {
                weaponPanel.SetActive(false);
                equipmentPanel.SetActive(false);
                mainPanel.SetActive(false);

                if (value == Menu.Main)
                {
                    mainPanel.SetActive(true);
                }
                else if(value == Menu.Weapon)
                {
                    weaponPanel.SetActive(true);
                }
                else if (value == Menu.Equipment)
                {
                    equipmentPanel.SetActive(true);
                }
            }
            currentMenu = value;
        }
    }

    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject weaponPanel;
    [SerializeField] GameObject equipmentPanel;

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

    [SerializeField] AudioSource hoverItemSFX;


    private void Awake()
    {
        foreach (GunMenuButton gB in slotButtons)
        {
            gB.Hide();
        }
        weaponSlotButtons.ForEach(x => x.Hide());
        equipmentSlotButtons.ForEach(x => x.Hide());
    }

    private void Start()
    {
        if(pC.isScientist)
        {
            active = false;
            return;
        }

        gunMenuItemsAmount = slotButtons.Count;

        equipmentMenuItemsAmount = equipmentSlotButtons.Count;
        weaponMenuItemsAmount = weaponSlotButtons.Count;
        mainMenuItemsAmount = mainSlotButtons.Count;

        gC = pC.gunController;
        eC = pC.equipmentController;

        currentSelectedItem = -1;
        previousSelectedItem = -1;
    }

    public void OpenInventory()
    {
        if (open || !active) return;
        inventoryPanel.SetActive(true);
        CurrentMenu = Menu.Main;
        currentSelectedItem = -1;
        previousSelectedItem = -1;
        RefreshAmmo();
        pC.AddAimSubscriber(this);
        open = true;
    }

    public void ForwardCommandAim(Vector2 controll, Vector2 controllSmooth)
    {
        if (controll.magnitude < 0.2f)
        {
            currentSelectedItem = -1;
            if (previousSelectedItem != -1)
            {
                if (CurrentMenu == Menu.Weapon) weaponSlotButtons[previousSelectedItem].image.color = weaponSlotButtons[previousSelectedItem].NormalColor;
                else if (CurrentMenu == Menu.Equipment) equipmentSlotButtons[previousSelectedItem].image.color = equipmentSlotButtons[previousSelectedItem].NormalColor;
                else if (CurrentMenu == Menu.Main) mainSlotButtons[previousSelectedItem].image.color = mainSlotButtons[previousSelectedItem].NormalColor;
                previousSelectedItem = -1;
            }
            return;
        }
        else
        {
            float rot_z = -(Mathf.Atan2(controll.y, controll.x) * Mathf.Rad2Deg - 90);

            if (rot_z < 0)
                rot_z += 360;

            if (CurrentMenu == Menu.Weapon)
            {
                currentSelectedItem = Mathf.Clamp(Mathf.FloorToInt(rot_z / (360 / weaponMenuItemsAmount)), 0, weaponMenuItemsAmount - 1);
                if (currentSelectedItem != previousSelectedItem)
                {
                    confirmChoose = chooseCategoryTime + Time.time;
                    confirmWeapon = changeWeaponTime + Time.time;
                    if (previousSelectedItem != -1) weaponSlotButtons[previousSelectedItem].image.color = weaponSlotButtons[previousSelectedItem].NormalColor;
                    previousSelectedItem = currentSelectedItem;
                    weaponSlotButtons[currentSelectedItem].image.color = weaponSlotButtons[currentSelectedItem].HoverColor;
                }
                else
                {
                    if (confirmChoose < Time.time && weaponSlotButtons[currentSelectedItem].returnButton)
                    {
                        CurrentMenu = Menu.Main;
                        currentSelectedItem = -1;
                        previousSelectedItem = -1;
                    }
                    else if(confirmWeapon < Time.time)
                    {
                        //weaponSlotButtons[currentSelectedItem].image.color = weaponSlotButtons[currentSelectedItem].PressedColor;
                        if (weaponSlotButtons[currentSelectedItem].isActive && !weaponSlotButtons[currentSelectedItem].isEmpty && !weaponSlotButtons[currentSelectedItem].returnButton)
                        {
                            gC.ChangeWeapon(weaponSlotButtons[currentSelectedItem].weaponType);
                        }
                    }
                }
            }
            else if (CurrentMenu == Menu.Equipment)
            {
                currentSelectedItem = Mathf.Clamp(Mathf.FloorToInt(rot_z / (360 / equipmentMenuItemsAmount)), 0, equipmentMenuItemsAmount - 1);
                if (currentSelectedItem != previousSelectedItem)
                {
                    confirmChoose = chooseCategoryTime + Time.time;
                    confirmWeapon = changeWeaponTime + Time.time;
                    if (previousSelectedItem != -1) equipmentSlotButtons[previousSelectedItem].image.color = equipmentSlotButtons[previousSelectedItem].NormalColor;
                    previousSelectedItem = currentSelectedItem;
                    equipmentSlotButtons[currentSelectedItem].image.color = equipmentSlotButtons[currentSelectedItem].HoverColor;
                }
                else if (confirmWeapon < Time.time)
                {
                    if (confirmChoose < Time.time  && equipmentSlotButtons[currentSelectedItem].returnButton)
                    {
                        CurrentMenu = Menu.Main;
                        currentSelectedItem = -1;
                        previousSelectedItem = -1;
                    }
                    else
                    {
                        //equipmentSlotButtons[currentSelectedItem].image.color = equipmentSlotButtons[currentSelectedItem].PressedColor;
                        if (equipmentSlotButtons[currentSelectedItem].isActive && !equipmentSlotButtons[currentSelectedItem].isEmpty && !equipmentSlotButtons[currentSelectedItem].returnButton)
                        {
                            eC.ChangeEquipment(equipmentSlotButtons[currentSelectedItem].equipmentType);
                        }
                    }
                }
            }
            else if (CurrentMenu == Menu.Main)
            {
                currentSelectedItem = Mathf.Clamp(Mathf.FloorToInt(rot_z / (360 / mainMenuItemsAmount)), 0, mainMenuItemsAmount - 1);
                

                if(currentSelectedItem != previousSelectedItem)
                {
                    confirmChoose = chooseCategoryTime + Time.time;
                    if (previousSelectedItem != -1) mainSlotButtons[previousSelectedItem].image.color = mainSlotButtons[previousSelectedItem].NormalColor;
                    previousSelectedItem = currentSelectedItem;
                    mainSlotButtons[currentSelectedItem].image.color = mainSlotButtons[currentSelectedItem].HoverColor;
                    hoverItemSFX.Play();
                }
                else
                {
                    if(confirmChoose < Time.time)
                    {
                        if (currentSelectedItem == 0) CurrentMenu = Menu.Weapon;
                        if (currentSelectedItem == 1) CurrentMenu = Menu.Equipment;
                        currentSelectedItem = -1;
                        previousSelectedItem = -1;
                    }
                }
            }
        }


        //if (controll.magnitude < 0.2f)
        //{
        //    currentSelectedItem = -1;
        //    if(previousSelectedItem != -1)
        //    {
        //        slotButtons[previousSelectedItem].image.color = slotButtons[previousSelectedItem].NormalColor;
        //        previousSelectedItem = -1;
        //    }
        //    return;
        //}

        //float rot_z = -(Mathf.Atan2(controll.y, controll.x) * Mathf.Rad2Deg - 90);

        //if (rot_z < 0)
        //    rot_z += 360;

        //currentSelectedItem = Mathf.Clamp(Mathf.FloorToInt(rot_z / (360 / gunMenuItemsAmount)), 0, gunMenuItemsAmount - 1);

        //if (currentSelectedItem != previousSelectedItem)
        //{
        //    if(previousSelectedItem != -1) slotButtons[previousSelectedItem].image.color = slotButtons[previousSelectedItem].NormalColor;
        //    previousSelectedItem = currentSelectedItem;
        //    slotButtons[currentSelectedItem].image.color = slotButtons[currentSelectedItem].HoverColor;
        //}
    }

    public void RefreshAmmo()
    {
        weaponSlotButtons.ForEach(button =>
        {
            if(!button.isEmpty || !button.returnButton)
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

        equipmentSlotButtons.ForEach(button =>
        {
            if (!button.isEmpty || !button.returnButton)
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
        //if (currentSelectedItem >= 0)
        //{

        //    if(CurrentMenu == Menu.Weapon)
        //    {
        //        weaponSlotButtons[currentSelectedItem].image.color = weaponSlotButtons[currentSelectedItem].PressedColor;
        //        if (weaponSlotButtons[currentSelectedItem].returnButton)
        //        {
        //            CurrentMenu = Menu.Main;
        //        }
        //        else if (weaponSlotButtons[currentSelectedItem].isActive && !weaponSlotButtons[currentSelectedItem].isEmpty)
        //        {
        //            gC.ChangeWeapon(weaponSlotButtons[currentSelectedItem].weaponType);
        //        }
        //    }
        //    else if(CurrentMenu == Menu.Equipment)
        //    {
        //        equipmentSlotButtons[currentSelectedItem].image.color = equipmentSlotButtons[currentSelectedItem].PressedColor;
        //        if (equipmentSlotButtons[currentSelectedItem].returnButton)
        //        {
        //            CurrentMenu = Menu.Main;
        //        }
        //        else if (equipmentSlotButtons[currentSelectedItem].isActive && !equipmentSlotButtons[currentSelectedItem].isEmpty)
        //        {
        //            eC.ChangeEquipment(equipmentSlotButtons[currentSelectedItem].equipmentType);
        //        }
        //    }

        //}
        inventoryPanel.SetActive(false);
        pC.RemoveAimSubscriber(this);
        open = false;
    }


    public void ActivateSlot(WeaponType weapon)
    {
        Debug.Log(weapon);
        weaponSlotButtons.Find(slot => slot.weaponType == weapon && slot.slotType == SlotType.Weapon).Show();     
    }

    public void ActivateSlot(EquipmentType equipment)
    {
        equipmentSlotButtons.Find(slot => slot.equipmentType == equipment && slot.slotType == SlotType.Equipment).Show();
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
    public bool returnButton;
    

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