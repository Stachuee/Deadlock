using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipiesManager : MonoBehaviour
{
    [SerializeField]
    List<CraftingRecipesSO> recipiesActive;

    [SerializeField] public static readonly int[] PRIMES = { 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97 }; // works if 3 or less items

    public static Hashtable recipes;
    
    private void Awake()
    {
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
            }
        });
    }

    public static int GetHashFromItems(List<ItemSO> items)
    {
        int hash = 0;
        items.ForEach(item =>
        {
            if (item.GetId() > PRIMES.Length) Debug.LogError("Not enough primes");
            hash += PRIMES[item.GetId()];
        });
        return hash;
    }
}
