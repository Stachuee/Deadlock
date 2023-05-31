using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeGuide : MonoBehaviour
{
    [SerializeField] PlayerController playerController;

    [SerializeField] Image itemIcon;
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] Image[] recipie;
    [SerializeField] TextMeshProUGUI[] recipieText;
    [SerializeField] TextMeshProUGUI itemDesc;

    [SerializeField] Button firstSelected;

    [SerializeField] GameObject upgradeGuide;

    [SerializeField] Slider hpSlider;

    private void Update()
    {
        UpdateStatus();
    }

    public void UpdateStatus()
    {
        hpSlider.value = GameController.solider.playerInfo.hp;
        hpSlider.maxValue = GameController.solider.playerInfo.maxHp;
    }

    public void Open(bool on)
    {
        if (on) upgradeGuide.SetActive(true);
        else upgradeGuide.SetActive(false);
        DeselectRecipie();
        playerController.uiController.myEventSystem.SetSelectedGameObject(firstSelected.gameObject);
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
    }
}
