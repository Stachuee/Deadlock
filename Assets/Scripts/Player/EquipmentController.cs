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

    [SerializeField] private Transform towerPlace;

    private void Awake()
    {
        playerController = transform.GetComponent<PlayerController>();
    }

    private void Start()
    {
        throwable = equipment[currentEquipmentIndex];

        inventory.AddEquipment(equipment[0].GetComponent<Granade>().GetInventorySlotPrefab());
        inventory.AddEquipment(equipment[1].GetComponent<Granade>().GetInventorySlotPrefab());
        inventory.AddEquipment(equipment[2].GetComponent<ShootingTower>().GetInventorySlotPrefab());
        inventory.AddEquipment(equipment[3].GetComponent<Granade>().GetInventorySlotPrefab());
        inventory.AddEquipment(equipment[4].GetComponent<Medicine>().GetInventorySlotPrefab());
        inventory.AddEquipment(equipment[5].GetComponent<Stimulator>().GetInventorySlotPrefab());
    }

    public void ChangeEquipment(int equipmentIndex)
    {
        currentEquipmentIndex = equipmentIndex;
        throwable = equipment[currentEquipmentIndex];
    }

    public void UseEquipment()
    {

        if (throwable.CompareTag("Tower"))
        {
            Instantiate(throwable, towerPlace.position, Quaternion.identity);
            return;
        }else if (throwable.CompareTag("Medicine"))
        {
            throwable.GetComponent<MedicineBase>().AddEffect(playerController);
            return;
        }
        GameObject temp = Instantiate(throwable, transform.position, Quaternion.identity);
        temp.GetComponent<Rigidbody2D>().AddForce(playerController.currentAimDirection.normalized * playerController.playerInfo.throwStrength);
    }
}
