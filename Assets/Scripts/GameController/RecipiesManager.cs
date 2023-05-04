using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipiesManager : MonoBehaviour
{
    public static RecipiesManager instance;

    [SerializeField]
    List<CraftingRecipesSO> recipiesActive;

    [SerializeField] public static readonly int[] PRIMES = { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97 };

    public static Hashtable recipes;
    
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        recipes = new Hashtable();

        recipiesActive.ForEach(recipe =>
        {
            int hash = GetHashFromItems(recipe.GetIngredientsItem());
            if(!recipes.ContainsKey(hash))
            {
                recipes.Add(hash, recipe);
            }
            else
            {
                Debug.LogError("TWO RECIPES WITH THE SAME HASH");
                Debug.Log(recipe.GetCraftedItem() + " " + ((CraftingRecipesSO)recipes[hash]).GetCraftedItem() + " " + hash);
            }
        });
    }

    public static int GetHashFromItems(List<ItemSO> items)
    {
        int hash = 1;
        items.ForEach(item =>
        {
            if (item.GetId() > PRIMES.Length) Debug.LogError("Not enough primes");
            hash *= PRIMES[item.GetId()];
        });
        return hash;
    }

    public List<CraftingRecipesSO> GetAllRecipies()
    {
        return recipiesActive;
    }
}
