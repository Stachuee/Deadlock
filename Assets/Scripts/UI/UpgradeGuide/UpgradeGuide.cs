using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeGuide : MonoBehaviour
{
    public enum UpgradePanel {Hero, Weapons }
    [SerializeField] PlayerController playerController;

    [SerializeField] Image itemIcon;
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] Image[] recipie;
    [SerializeField] TextMeshProUGUI[] recipieText;
    [SerializeField] TextMeshProUGUI itemDesc;

    [SerializeField] GameObject upgradeGuide;

    [SerializeField] Slider hpSlider;

    [SerializeField] GameObject guide;

    bool connected = false;

    [SerializeField] List<GameObject> guns;
    [SerializeField] List<GameObject> equipments;

    [SerializeField] List<UpgradeGuideButton> upgrades;


    [SerializeField] GameObject weaponPanel;
    [SerializeField] Button firstSelectedWeapon;
    [SerializeField] GameObject heroPanel;
    [SerializeField] Button firstSelectedHero;

    [SerializeField] Transform infoPanel;

    private void Awake()
    {
        upgrades = new List<UpgradeGuideButton>();
        upgrades = transform.GetComponentsInChildren<UpgradeGuideButton>().ToList();

        guns.ForEach(gun =>
        {
            gun.SetActive(false);
        });

        equipments.ForEach(equipment =>
        {
            equipment.SetActive(false);
        });
    }

    private void Update()
    {
        UpdateStatus();
    }

    public void UpdateStatus()
    {
        if(connected)
        {
            hpSlider.value = GameController.solider.playerInfo.hp;
            hpSlider.maxValue = GameController.solider.playerInfo.maxHp;
        }
        else if(GameController.playersConnected)
        {
            connected = true;
        }
        
    }

    public void Open(bool on, UpgradePanel panel)
    {
        if (on)
        {
            if (panel == UpgradePanel.Weapons)
            {
                weaponPanel.SetActive(true);
                heroPanel.SetActive(false);
                playerController.uiController.myEventSystem.SetSelectedGameObject(firstSelectedWeapon.gameObject);
            }
            else if (panel == UpgradePanel.Hero)
            {
                heroPanel.SetActive(true);
                weaponPanel.SetActive(false);
                playerController.uiController.myEventSystem.SetSelectedGameObject(firstSelectedHero.gameObject);
            }
        }
        else
        {
            weaponPanel.SetActive(false);
            heroPanel.SetActive(false);
        }
        guide.SetActive(on);
    }


    public void SelectRecipe(CraftingRecipesSO recipe)
    {
        itemIcon.sprite = recipe.GetCraftedItem().GetIconSprite();
        itemName.text = recipe.GetCraftedItem().GetItemName();
        itemDesc.text = recipe.GetCraftedItem().GetItemDesc();
        itemName.gameObject.SetActive(true);
        itemIcon.gameObject.SetActive(true);
        itemDesc.gameObject.SetActive(true);

        int index = 0;

        for (int i = 0; i < recipie.Length; i++)
        {
            recipie[i].gameObject.SetActive(false);
        }

        recipe.GetIngredientsItem().ForEach(item =>
        {
            recipie[index].sprite = item.GetIconSprite();
            recipieText[index].text = item.GetItemName();

            recipie[index].gameObject.SetActive(true);
            index++;
        });

        infoPanel.gameObject.SetActive(true);
    }

    public void DeselectRecipie()
    {
        itemName.gameObject.SetActive(false);
        itemIcon.gameObject.SetActive(false);
        itemDesc.gameObject.SetActive(false);

        for (int i = 0; i < recipie.Length; i++)
        {
            recipie[i].gameObject.SetActive(false);
        }
        infoPanel.gameObject.SetActive(false);
    }

    public void UnlockRecipie(CraftingRecipesSO recipe)
    {
        upgrades.Find(x => x.GetRecipe() == recipe).Unlock();
    }

    public void UnlockGun(WeaponType type)
    {
        guns[(int)type].SetActive(true);
    }

    public void UnlockEquipment(EquipmentType type)
    {
        equipments[(int)type].SetActive(true);
    }
}
