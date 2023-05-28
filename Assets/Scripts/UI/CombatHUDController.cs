using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatHUDController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;

    [SerializeField] Slider hpSlider;
    [SerializeField] Image reload;


    [SerializeField] List<Sprite> equipmentIcons;
    [SerializeField] Image equipmentIcon;
    [SerializeField] TextMeshProUGUI equipmentCount;

    [SerializeField] Image bulletTypeSprite;
    [SerializeField] Text bulletsAmount;

    GunController gunController;

    bool active = true;

    private void Start()
    {
        if (playerController.isScientist)
        {
            gameObject.SetActive(false);
            active = false;
        }

        gunController = playerController.gunController;
    }

    private void Update()
    {
        if (!active) return;
        hpSlider.maxValue = playerController.playerInfo.maxHp;
        hpSlider.value = playerController.playerInfo.hp;

        bulletTypeSprite.sprite = gunController.GetCurrentGun().GetAmmoIcon();
        bulletsAmount.text = (gunController.GetCurrentGun().GetAmmoAmount());
    }

    public void UpdateReload(float progress)
    {
        reload.fillAmount = progress;
    }

    public void UpdateEquipment(EquipmentType current)
    {
        equipmentIcon.sprite = equipmentIcons[(int)current];
    }

    public void UpdateEquipmentCount(int count)
    {
        equipmentCount.text = count.ToString();
    }
}
