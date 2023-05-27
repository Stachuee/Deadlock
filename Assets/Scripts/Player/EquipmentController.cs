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

    [SerializeField] private Transform towerPlace;

    [SerializeField]
    Dictionary<EquipmentType, int> backpack = new Dictionary<EquipmentType, int>();

    [SerializeField] MedicineBase medkit;
    [SerializeField] MedicineBase stim;

    Vector2 aimDirection;

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
        playerController.AddMovmentSubscriber(this);

        equiped = EquipmentType.Granade;

        inventorySelector.ActivateSlot(EquipmentType.Molotov);
        inventorySelector.ActivateSlot(EquipmentType.Medkit);
        inventorySelector.ActivateSlot(EquipmentType.Granade);
        inventorySelector.ActivateSlot(EquipmentType.Stim);
        inventorySelector.ActivateSlot(EquipmentType.Turret);
    }

    public void ChangeEquipment(EquipmentType type)
    {
        equiped = type;
    }

    public void UseEquipment()
    {
        GameObject temp;

        switch (equiped)
        {
            case EquipmentType.Granade:
                if (backpack[EquipmentType.Granade] <= 0) return;
                temp = Instantiate(equipment[(int)equiped], transform.position, Quaternion.identity);
                temp.GetComponent<NadeBase>().Lunch(aimDirection.normalized * playerController.playerInfo.throwStrength);
                return;

            case EquipmentType.Molotov:
                if (backpack[EquipmentType.Molotov] <= 0) return;
                temp = Instantiate(equipment[(int)equiped], transform.position, Quaternion.identity);
                temp.GetComponent<NadeBase>().Lunch(aimDirection.normalized * playerController.playerInfo.throwStrength);
                return;

            case EquipmentType.Turret:
                if (backpack[EquipmentType.Turret] <= 0) return;
                backpack[EquipmentType.Turret]--;
                Instantiate(equipment[(int)equiped], (Vector2)transform.position + aimDirection.normalized * 1, Quaternion.identity).GetComponent<Rigidbody2D>().AddForce(aimDirection.normalized * playerController.playerInfo.throwStrength);
                return;

            case EquipmentType.Medkit:
                if (backpack[EquipmentType.Medkit] <= 0 || playerController.GetIsHealing()) return;
                backpack[EquipmentType.Medkit]--;
                medkit.AddEffect(playerController);
                return;

            case EquipmentType.Stim:
                if (backpack[EquipmentType.Stim] <= 0 || playerController.GetIsStimulated()) return;
                backpack[EquipmentType.Stim]--;
                stim.AddEffect(playerController);
                return;
        }
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
