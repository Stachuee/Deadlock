using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [SerializeField]
    List<Dialogue> newItemDialogues;

    Dialogue currentDialouge;

    List<DialogueUI> dialogueLisners = new List<DialogueUI>();


    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void TriggerDialogue(Dialogue.Trigger trigger, bool cutPlaying = false)
    {
        if (currentDialouge != null && !cutPlaying) return;

        switch(trigger)
        {
            case Dialogue.Trigger.Kill:
                break;
            case Dialogue.Trigger.OnNewItemPickup:
                QueueDialogue(newItemDialogues[Random.Range(0, newItemDialogues.Count)]);
                break;
        }
    }

    public void QueueDialogue(Dialogue toPlay)
    {
        StopCoroutine("PlayDialogue");
        currentDialouge = toPlay;
        StartCoroutine("PlayDialogue");
    }


    IEnumerator PlayDialogue()
    {
        int step = 0;
        while(currentDialouge.GetDialouge().Count > step)
        {
            SendDialogue(currentDialouge.GetDialouge()[step].speaker.GetHeroName(), currentDialouge.GetDialouge()[step].text);
            yield return new WaitForSeconds(currentDialouge.GetDialouge()[step].timeOnScreen);
            step++;
        }
        HideDialogue();
        currentDialouge = null;
    }

    public void SendDialogue(string speaker, string toSend)
    {
        dialogueLisners.ForEach(lisner =>
        {
            lisner.ShowText(speaker, toSend);
        });
    }

    public void HideDialogue()
    {
        dialogueLisners.ForEach(lisner =>
        {
            lisner.HideText();
        });
    }

    public void AddLisner(DialogueUI toAdd)
    {
        dialogueLisners.Add(toAdd);
    }

}
