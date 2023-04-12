using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSelector : MonoBehaviour
{
    EquipmentController equipmentController;
    [SerializeField] int equipmentIndex;
    void Start()
    {
        equipmentController = FindObjectOfType<EquipmentController>();
    }

    public void SelectEquipment()
    {
        equipmentController.ChangeEquipment(equipmentIndex);
    }
}
