using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingHelperButtonScript : MonoBehaviour
{
    CraftingHelperScript owner;

    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI itemName;

    CraftingRecipesSO toCraft;

    public void SetUp(CraftingHelperScript helper, CraftingRecipesSO recipes)
    {
        owner = helper;
        toCraft = recipes;
        itemImage.sprite = toCraft.GetCraftedItem().GetIconSprite();
        itemName.text = toCraft.GetCraftedItem().GetItemName();
    }

    public void Click()
    {
        owner.ShowInfo(toCraft);
    }
}
