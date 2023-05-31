using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType { Granade, Molotov, Stim, Medkit, Turret }

public class EquipmentController : MonoBehaviour, IControllSubscriberMovment
{


    EquipmentType equiped;

    PlayerController playerController;

    [SerializeField] private List<GameObject> equipment;

    [SerializeField] private InventorySelector inventorySelector;

    [SerializeField]
    Dictionary<EquipmentType, int> backpack = new Dictionary<EquipmentType, int>();

    [SerializeField] MedicineBase medkit;
    [SerializeField] MedicineBase stim;

    Vector2 aimDirection;

    bool active = true;

    private void Awake()
    {
        for(int i = 0; i < System.Enum.GetValues(typeof(EquipmentType)).Length; i++)
        {
            backpack.Add((EquipmentType)i, 10); // starting ammo. Change to 0 later
        }

        playerController = transform.GetComponent<PlayerController>();
    }

    private void Start()
    {
        if(playerController.isScientist)
        {
            active = false;
            return;
        }
        playerController.AddMovmentSubscriber(this);

        equiped = EquipmentType.Granade;

        UnlockEquipment(EquipmentType.Granade);

        playerController.uiController.combatHUDController.UpdateEquipment(equiped);
        playerController.uiController.combatHUDController.UpdateEquipmentCount(backpack[equiped]);
    }

    public void UnlockEquipment(EquipmentType type)
    {
        inventorySelector.ActivateSlot(type);
        GameController.scientist.uiController.upgradeGuide.UnlockEquipment(type);
    }

    public void ChangeEquipment(EquipmentType type)
    {
        if (!active) return;
        equiped = type;
        playerController.uiController.combatHUDController.UpdateEquipment(type);
        playerController.uiController.combatHUDController.UpdateEquipmentCount(backpack[type]);
    }

    public void UseEquipment()
    {
        if (!active) return;
        GameObject temp;

        switch (equiped)
        {
            case EquipmentType.Granade:
                if (backpack[EquipmentType.Granade] <= 0) return;
                temp = Instantiate(equipment[(int)equiped], transform.position, Quaternion.identity);
                backpack[EquipmentType.Granade]--;
                temp.GetComponent<NadeBase>().Lunch(playerController.currentAimDirection.normalized * playerController.playerInfo.throwStrength);
                break;

            case EquipmentType.Molotov:
                if (backpack[EquipmentType.Molotov] <= 0) return;
                temp = Instantiate(equipment[(int)equiped], transform.position, Quaternion.identity);
                backpack[EquipmentType.Molotov]--;
                temp.GetComponent<NadeBase>().Lunch(playerController.currentAimDirection.normalized * playerController.playerInfo.throwStrength);
                break;

            case EquipmentType.Turret:
                if (backpack[EquipmentType.Turret] <= 0) return;
                backpack[EquipmentType.Turret]--;
                Instantiate(equipment[(int)equiped], (Vector2)transform.position + playerController.currentAimDirection.normalized * 1, Quaternion.identity).GetComponent<Rigidbody2D>().AddForce(aimDirection.normalized * playerController.playerInfo.throwStrength);
                break;

            case EquipmentType.Medkit:
                if (backpack[EquipmentType.Medkit] <= 0 || playerController.GetIsHealing()) return;
                backpack[EquipmentType.Medkit]--;
                medkit.AddEffect(playerController);
                break;

            case EquipmentType.Stim:
                if (backpack[EquipmentType.Stim] <= 0 || playerController.GetIsStimulated()) return;
                backpack[EquipmentType.Stim]--;
                stim.AddEffect(playerController);
                break;
        }

        playerController.uiController.combatHUDController.UpdateEquipmentCount(backpack[equiped]);
    }

    public string GetEquipmentAmmo(EquipmentType type)
    {
        return backpack[type].ToString();
    }

    public void ForwardCommandMovment(Vector2 controll)
    {
        aimDirection = controll;
    }

    //public int GetGranadeAmount()
    //{
    //    return grenadeAmount;
    //}
    //public int GetMolotovAmount()
    //{
    //    return molotovAmount;
    //}
    //public int GetTowerAmount()
    //{
    //    return towerAmount;
    //}
    //public int GetMedicineAmount()
    //{
    //    return medicineAmount;
    //}
    //public int GetStimulatorAmount()
    //{
    //    return stimulatorAmount;
    //}
}
