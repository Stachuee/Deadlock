using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ItemSO : ScriptableObject
{
    [SerializeField] int id;
    [SerializeField] string itemName;
    [SerializeField] Sprite defaultSprite;
    [SerializeField] Sprite icon;

    public abstract bool PickUp(PlayerController player, Item item);
    public abstract void Drop(PlayerController player, Item item);


    #region Get/Set
    public int GetId()
    {
        return id;
    }
    public string GetItemName()
    {
        return itemName;
    }
    public Sprite GetDefaultSprite()
    {
        return defaultSprite;
    }
    public Sprite GetIconSprite()
    {
        return icon;
    }
    #endregion
}
