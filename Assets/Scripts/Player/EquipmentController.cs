using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType { Granade, Molotov, Stim, Medkit, Turret, RepairKit }

public class EquipmentController : MonoBehaviour, IControllSubscriberAim
{
    readonly float EQUIPMENT_CD = 1;

    EquipmentType equiped;

    PlayerController playerController;

    [SerializeField] private List<GameObject> equipment;

    [SerializeField] private InventorySelector inventorySelector;

    [SerializeField]
    Dictionary<EquipmentType, int> backpack = new Dictionary<EquipmentType, int>();

    [SerializeField] MedicineBase medkit;
    [SerializeField] MedicineBase stim;

    Vector2 aimDirection;

    float equipmentCdRemain;

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
        playerController.AddAimSubscriber(this);

        equiped = EquipmentType.Granade;

        UnlockEquipment(EquipmentType.Granade);
        UnlockEquipment(EquipmentType.RepairKit);
        UnlockEquipment(EquipmentType.Medkit);
        UnlockEquipment(EquipmentType.Stim);
        UnlockEquipment(EquipmentType.Turret);
        UnlockEquipment(EquipmentType.Molotov);

        playerController.uiController.combatHUDController.UpdateEquipment(equiped);
        playerController.uiController.combatHUDController.UpdateEquipmentCount(backpack[equiped]);
    }

    public void UnlockEquipment(EquipmentType type)
    {
        inventorySelector.ActivateSlot(type);
        if (GameController.scientist != null) GameController.scientist.uiController.upgradeGuide.UnlockEquipment(type);
    }

    public void ChangeEquipment(EquipmentType type)
    {
        if (!active) return;
        equiped = type;
        playerController.uiController.combatHUDController.UpdateEquipment(type);
        playerController.uiController.combatHUDController.UpdateEquipmentCount(backpack[type]);
        playerController.RefreshPrompt();
    }

    public void UseEquipment()
    {
        if (!active || equipmentCdRemain > Time.time) return;
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
            case EquipmentType.RepairKit:
                break;
        }
        equipmentCdRemain = EQUIPMENT_CD + Time.time;
        playerController.uiController.combatHUDController.UpdateEquipmentCount(backpack[equiped]);
    }

    public string GetEquipmentAmmo(EquipmentType type)
    {
        return backpack[type].ToString();
    }


    public EquipmentType GetCurrentlyEquiped()
    {
        return equiped;
    }

    public int GetCurrentlyEquipedAmmo()
    {
        return backpack[equiped];
    }

    public void UseCurretnlyEquiped()
    {
        backpack[equiped]--;
        playerController.uiController.combatHUDController.UpdateEquipmentCount(backpack[equiped]);
    }

    public void ForwardCommandAim(Vector2 controll, Vector2 controllSmooth)
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
