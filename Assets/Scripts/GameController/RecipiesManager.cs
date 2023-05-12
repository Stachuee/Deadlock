using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipiesManager : MonoBehaviour
{
    public static RecipiesManager instance;

    [SerializeField]
    List<CraftingRecipesSO> recipiesActive;

    [SerializeField]
    List<CraftingRecipesSO> recipiesUnlocked = new List<CraftingRecipesSO>();

    [SerializeField] public static readonly int[] PRIMES = { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97 };

    List<CraftingHelperScript> toNotify;

    Dictionary<int, bool> pickedUp;

    public static Hashtable recipes;
    
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        toNotify = new List<CraftingHelperScript>();
        pickedUp = new Dictionary<int, bool>();

        recipiesActive.ForEach(recipe =>
        {
            recipe.GetIngredientsItem().ForEach(item =>
            {
                if(!pickedUp.ContainsKey(item.GetId()))
                {
                    pickedUp.Add(item.GetId(), false);
                }
            });
        });

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

    public List<CraftingRecipesSO> GetUnlockedRecipies()
    {
        return recipiesUnlocked;
    }

    public void AddHelper(CraftingHelperScript toAdd)
    {
        toNotify.Add(toAdd);
    }

    public void PickedUp(ItemSO item)
    {
        if(pickedUp.ContainsKey(item.GetId()) && !pickedUp[item.GetId()])
        {
            pickedUp[item.GetId()] = true;

            DialogueManager.instance.TriggerDialogue(Dialogue.Trigger.OnNewItemPickup); // send dialogue

            List<CraftingRecipesSO> toUnlock = new List<CraftingRecipesSO>();

            recipiesActive.ForEach(recipie =>{
                bool unlock = true;
                recipie.GetIngredientsItem().ForEach(item =>
                {
                    if(!pickedUp[item.GetId()]) unlock = false;
                });
                if(unlock)
                {
                    toUnlock.Add(recipie);
                }
            });

            toUnlock.ForEach(item => recipiesActive.Remove(item));
            recipiesUnlocked.AddRange(toUnlock);
            toNotify.ForEach(helper => helper.RefreshCrafting());
        }
    }

}
