using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSelector : MonoBehaviour
{
    EquipmentController equipmentController;
    [SerializeField] int equipmentIndex;

    Text amount;
    void Start()
    {
        amount = GetComponentInChildren<Text>();
        equipmentController = FindObjectOfType<EquipmentController>();
    }

    public void SelectEquipment()
    {
        equipmentController.ChangeEquipment(equipmentIndex);
    }
    private void Update()
    {
        switch (equipmentIndex)
        {
            case 0:
                amount.text = equipmentController.GetGranadeAmount().ToString();
                break;
            case 1:
                amount.text = equipmentController.GetTowerAmount().ToString();
                break;
            case 2:
                amount.text = equipmentController.GetMolotovAmount().ToString();
                break;
            case 3:
                amount.text = equipmentController.GetMedicineAmount().ToString();
                break;
            case 4:
                amount.text = equipmentController.GetStimulatorAmount().ToString();
                break;
            default:
                amount.text = "Loading Error";
                Debug.LogError("Invalid Equipment Index!");
                break;
        }
    }
}
