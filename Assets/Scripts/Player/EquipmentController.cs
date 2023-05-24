using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType { Granade, Molotov, Stim, Medkit, Turret }

public class EquipmentController : MonoBehaviour, IControllSubscriberMovment
{


    GameObject throwable;

    PlayerController playerController;

    [SerializeField] private List<GameObject> equipment;
    private int currentEquipmentIndex = 0;

    [SerializeField] private InventorySelector inventorySelector;

    [SerializeField] private Transform towerPlace;

    [SerializeField]
    Dictionary<EquipmentType, int> backpack = new Dictionary<EquipmentType, int>();


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

        throwable = equipment[currentEquipmentIndex];

        inventorySelector.ActivateSlot(EquipmentType.Molotov);
        inventorySelector.ActivateSlot(EquipmentType.Medkit);
        inventorySelector.ActivateSlot(EquipmentType.Granade);
        inventorySelector.ActivateSlot(EquipmentType.Stim);
        inventorySelector.ActivateSlot(EquipmentType.Turret);
    }

    public void ChangeEquipment(EquipmentType type)
    {
        Debug.Log(type);
        currentEquipmentIndex = (int)type;
        throwable = equipment[currentEquipmentIndex];
    }

    public void UseEquipment()
    {
        if (throwable.CompareTag("Granade") && backpack[EquipmentType.Granade] <= 0)
            return;
        else if (throwable.CompareTag("Molotov") && backpack[EquipmentType.Molotov] <= 0)
            return;
        else if (throwable.CompareTag("Tower") && backpack[EquipmentType.Turret] <= 0)
            return;
        else if (throwable.CompareTag("Medicine") && backpack[EquipmentType.Medkit] <= 0)
            return;
        else if ((throwable.CompareTag("Stimulator") && backpack[EquipmentType.Stim] <= 0) || (throwable.CompareTag("Stimulator") && playerController.GetIsStimulated()))
            return;


        if (throwable.CompareTag("Tower"))
        {
            backpack[EquipmentType.Turret]--;
            Instantiate(throwable, towerPlace.position, Quaternion.identity);
            return;
        }
        else if (throwable.CompareTag("Medicine") || throwable.CompareTag("Stimulator"))
        {
            if (throwable.CompareTag("Medicine"))
                backpack[EquipmentType.Medkit]--;
            else if (throwable.CompareTag("Stimulator"))
                backpack[EquipmentType.Stim]--;
            throwable.GetComponent<MedicineBase>().AddEffect(playerController);
            return;
        }

        if (throwable.CompareTag("Granade"))
            backpack[EquipmentType.Granade]--;
        else if (throwable.CompareTag("Molotov"))
            backpack[EquipmentType.Molotov]--;
        GameObject temp = Instantiate(throwable, transform.position, Quaternion.identity);
        temp.GetComponent<NadeBase>().Lunch(aimDirection.normalized * playerController.playerInfo.throwStrength);
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
