using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentController : MonoBehaviour
{


    GameObject throwable;

    PlayerController playerController;

    [SerializeField] private List<GameObject> equipment;
    private int currentEquipmentIndex = 0;

    [SerializeField] private Inventory inventory;

    [SerializeField] private InventorySelector inventorySelector;

    [SerializeField] private Transform towerPlace;

    [SerializeField] private int grenadeAmount;
    [SerializeField] private int molotovAmount;
    [SerializeField] private int towerAmount;
    [SerializeField] private int medicineAmount;
    [SerializeField] private int stimulatorAmount;

    private void Awake()
    {
        playerController = transform.GetComponent<PlayerController>();
    }

    private void Start()
    {
        throwable = equipment[currentEquipmentIndex];

        /*inventory.AddEquipment(equipment[0].GetComponent<Granade>().GetInventorySlotPrefab());
        inventory.AddEquipment(equipment[1].GetComponent<ShootingTower>().GetInventorySlotPrefab());
        inventory.AddEquipment(equipment[2].GetComponent<Granade>().GetInventorySlotPrefab());
        inventory.AddEquipment(equipment[3].GetComponent<Medicine>().GetInventorySlotPrefab());
        inventory.AddEquipment(equipment[4].GetComponent<Stimulator>().GetInventorySlotPrefab());*/

        inventorySelector.AddSlot(SlotType.Equipment, 0);
        inventorySelector.AddSlot(SlotType.Equipment, 1);
        inventorySelector.AddSlot(SlotType.Equipment, 2);
        inventorySelector.AddSlot(SlotType.Equipment, 3);
        inventorySelector.AddSlot(SlotType.Equipment, 4);
    }

    public void ChangeEquipment(int equipmentIndex)
    {
        currentEquipmentIndex = equipmentIndex;
        throwable = equipment[currentEquipmentIndex];
    }

    public void UseEquipment()
    {
        if (throwable.CompareTag("Granade") && grenadeAmount <= 0)
            return;
        else if (throwable.CompareTag("Molotov") && molotovAmount <= 0)
            return;
        else if (throwable.CompareTag("Tower") && towerAmount <= 0)
            return;
        else if (throwable.CompareTag("Medicine") && medicineAmount <= 0)
            return;
        else if ((throwable.CompareTag("Stimulator") && stimulatorAmount <= 0) || (throwable.CompareTag("Stimulator") && playerController.GetIsStimulated()))
            return;


        if (throwable.CompareTag("Tower"))
        {
            towerAmount--;
            Instantiate(throwable, towerPlace.position, Quaternion.identity);
            return;
        }
        else if (throwable.CompareTag("Medicine") || throwable.CompareTag("Stimulator"))
        {
            if (throwable.CompareTag("Medicine"))
                medicineAmount--;
            else if (throwable.CompareTag("Stimulator"))
                stimulatorAmount--;
            throwable.GetComponent<MedicineBase>().AddEffect(playerController);
            return;
        }

        if (throwable.CompareTag("Granade"))
            grenadeAmount--;
        else if (throwable.CompareTag("Molotov"))
            molotovAmount--;
        GameObject temp = Instantiate(throwable, transform.position, Quaternion.identity);
        temp.GetComponent<Rigidbody2D>().AddForce(playerController.currentAimDirection.normalized * playerController.playerInfo.throwStrength);
    }


    public int GetGranadeAmount()
    {
        return grenadeAmount;
    }
    public int GetMolotovAmount()
    {
        return molotovAmount;
    }
    public int GetTowerAmount()
    {
        return towerAmount;
    }
    public int GetMedicineAmount()
    {
        return medicineAmount;
    }
    public int GetStimulatorAmount()
    {
        return stimulatorAmount;
    }
}
