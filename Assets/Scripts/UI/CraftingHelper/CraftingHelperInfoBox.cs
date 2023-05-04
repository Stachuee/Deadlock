using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingHelperInfoBox : MonoBehaviour
{
    [SerializeField] Image itemIcon;
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] Image[] recipie;
    [SerializeField] TextMeshProUGUI[] recipieText;

    public void SetUp(CraftingRecipesSO toCraft)
    {

        itemIcon.sprite = toCraft.GetCraftedItem().GetIconSprite();
        itemName.text = toCraft.GetCraftedItem().GetItemName();
        itemName.gameObject.SetActive(true);
        itemIcon.gameObject.SetActive(true);

        int index = 0;

        for(int i = 0; i < recipie.Length; i++)
        {
            recipie[i].gameObject.SetActive(false);
        }

        toCraft.GetIngredientsItem().ForEach(item =>
        {
            recipie[index].sprite = item.GetIconSprite();
            recipieText[index].text = item.GetItemName();

            recipie[index].gameObject.SetActive(true);
            index++;
        });
    }
}
