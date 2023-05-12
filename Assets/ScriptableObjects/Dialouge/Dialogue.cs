using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Dialouge/Dialouge", order = 0)]
public class Dialogue : ScriptableObject
{
    public enum Trigger { None, Kill, OnNewItemPickup}
    public enum ShowTo {All, Scientist, Solider }

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
    Dialogue nextDialogue;
    [SerializeField]
    ShowTo toShow;

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
    public Dialogue GetNextDialogue()
    {
        return nextDialogue;
    }
    public ShowTo GetToShow()
    {
        return toShow;
    }
}

