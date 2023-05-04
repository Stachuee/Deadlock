using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingHelperScript : MonoBehaviour
{
    [SerializeField]
    PlayerController playerController;
    [SerializeField] GameObject craftingHelper;

    [SerializeField] CraftingHelperInfoBox craftingHelperInfoBox;

    [SerializeField] Transform spawnUnder;
    [SerializeField] GameObject buttonsPrefab;

    [SerializeField] UnityEngine.UI.Button firstSelected;

    readonly int MAX_CRAFTING_ITEMS_IN_ONE_TAB = 10;

    List<CraftingHelperButtonScript> helpers = new List<CraftingHelperButtonScript>();

    private void Start()
    {
        for(int i =0; i < MAX_CRAFTING_ITEMS_IN_ONE_TAB; i++)
        {
            helpers.Add(Instantiate(buttonsPrefab, spawnUnder).GetComponent<CraftingHelperButtonScript>());
        }

        RefreshCrafting();
    }

    public void ShowInfo(CraftingRecipesSO toShow)
    {
        craftingHelperInfoBox.SetUp(toShow);
    }

    public void Open(bool on)
    {
        if (on) craftingHelper.SetActive(true);
        else craftingHelper.SetActive(false);
        RefreshCrafting();
        playerController.uiController.myEventSystem.SetSelectedGameObject(firstSelected.gameObject);
        firstSelected.onClick.Invoke();
    }

    public void ChangeCategory(int tab)
    {
        List<CraftingRecipesSO> toShow = RecipiesManager.instance.GetAllRecipies().FindAll(x => (int)x.GetTab() == tab);

        if (MAX_CRAFTING_ITEMS_IN_ONE_TAB < toShow.Count) Debug.LogError("add more prefabs");

        for(int i = 0; i < toShow.Count; i++)
        {
            helpers[i].SetUp(this, toShow[i]);
            helpers[i].gameObject.SetActive(true);
        }

        for (int i = toShow.Count; i < MAX_CRAFTING_ITEMS_IN_ONE_TAB; i++)
        {
            helpers[i].gameObject.SetActive(false);
        }
    }

    public void RefreshCrafting()
    {

    }
}
