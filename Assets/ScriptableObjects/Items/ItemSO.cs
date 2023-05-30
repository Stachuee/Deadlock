using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ItemSO : ScriptableObject
{
    [SerializeField] protected int id;
    [SerializeField] string itemName;
    [SerializeField] Sprite defaultSprite;
    [SerializeField] Sprite icon;
    [SerializeField, TextArea(5, 10)] string itemDesc;

    public abstract bool PickUp(PlayerController player, Item item, out bool destroy);
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
    public string GetItemDesc()
    {
        return itemDesc;
    }
    #endregion
}
