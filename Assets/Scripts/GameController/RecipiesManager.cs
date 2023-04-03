using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipiesManager : MonoBehaviour
{
    [SerializeField]
    List<CraftingRecipesSO> recipiesActive;

    public static List<CraftingRecipesSO> recipies;
    
    private void Awake()
    {
        recipies = recipiesActive;
    }
}
