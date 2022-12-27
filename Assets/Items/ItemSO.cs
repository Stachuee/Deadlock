using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Items", order = 1)]
public class ItemSO : ScriptableObject
{
    [SerializeField] int id;
    [SerializeField] string itemName;
    [SerializeField] Sprite defaultSprite;
    [SerializeField] Sprite icon;

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
