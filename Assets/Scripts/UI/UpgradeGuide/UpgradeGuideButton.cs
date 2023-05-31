using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeGuideButton : MonoBehaviour, ISelectHandler, IPointerEnterHandler, IPointerExitHandler
{
    UpgradeGuide guide;

    [SerializeField] CraftingRecipesSO upgradeRecipe;
    [SerializeField] CraftingRecipesSO unknownRecipe;
    Button myButton;

    [SerializeField] bool unlocked;

    private void Awake()
    {
        guide = GetComponentInParent<UpgradeGuide>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
       guide.SelectRecipe(unlocked ? upgradeRecipe : unknownRecipe);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        guide.DeselectRecipie();
    }

    public void OnSelect(BaseEventData eventData)
    {
        guide.SelectRecipe(unlocked ? upgradeRecipe : unknownRecipe);
    }

    public void SetupUpgradeButton(UpgradeGuide guide)
    {
        myButton = gameObject.GetComponent<Button>();

        this.guide = guide;
    }

    public CraftingRecipesSO GetRecipe()
    {
        return upgradeRecipe;
    }

    public void Unlock()
    {
        unlocked = true;
    }
}
