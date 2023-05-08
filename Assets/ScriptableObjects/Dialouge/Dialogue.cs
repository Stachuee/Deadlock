using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Dialouge/Dialouge", order = 0)]
public class Dialogue : ScriptableObject
{
    public enum Trigger {Kill, OnNewItemPickup }
    [System.Serializable]
    public struct DialougeField
    {
        [TextArea(5, 10)]
        public string text;
        public Hero speaker;
        public float timeOnScreen;
    };

    [SerializeField]
    List<DialougeField> dialogue;
    [SerializeField]
    Trigger trigger;
    public List<DialougeField> GetDialouge()
    {
        return dialogue;
    }
    public Trigger GetTrigger()
    {
        return trigger;
    }
}

