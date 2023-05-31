using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeGuideButton : MonoBehaviour, ISelectHandler, IPointerEnterHandler, IPointerExitHandler
{
    UpgradeGuide guide;

    [SerializeField] CraftingRecipesSO upgradeRecipe;

    Button myButton;

    private void Awake()
    {
        guide = GetComponentInParent<UpgradeGuide>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
       guide.SelectRecipe(upgradeRecipe);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        guide.DeselectRecipie();
    }

    public void OnSelect(BaseEventData eventData)
    {
        guide.SelectRecipe(upgradeRecipe);
    }

    public void SetupUpgradeButton(UpgradeGuide guide)
    {
        myButton = gameObject.GetComponent<Button>();

        this.guide = guide;
    }


}
