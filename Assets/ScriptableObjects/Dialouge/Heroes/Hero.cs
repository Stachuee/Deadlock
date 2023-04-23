using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Hero", menuName = "ScriptableObjects/Dialouge/Hero", order = 0)]
public class Hero : ScriptableObject
{
    [SerializeField]
    string heroName;
    [SerializeField]
    Sprite sprite;

    public string GetHeroName()
    {
        return heroName;
    }
    public Sprite GetSprite()
    {
        return sprite;
    }
}