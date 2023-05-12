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

    public void PlayDialogue(Dialogue toPlay, bool cutPlaying)
    {
        if (currentDialouge != null && !cutPlaying) return;
        QueueDialogue(toPlay);
    }

    void QueueDialogue(Dialogue toPlay)
    {
        StopCoroutine("StartDialogue");
        currentDialouge = toPlay;
        StartCoroutine("StartDialogue");
    }


    IEnumerator StartDialogue()
    {
        int step = 0;
        while(currentDialouge.GetDialouge().Count > step)
        {
            SendDialogue(currentDialouge.GetDialouge()[step].speaker.GetHeroName(), currentDialouge.GetDialouge()[step].text, currentDialouge.GetToShow());
            yield return new WaitForSeconds(currentDialouge.GetDialouge()[step].timeOnScreen);
            step++;
        }
        if(currentDialouge.GetNextDialogue() != null)
        {
            PlayDialogue(currentDialouge.GetNextDialogue(), true);
        }
        else
        {
            HideDialogue();
            currentDialouge = null;
        }
    }

    public void SendDialogue(string speaker, string toSend, Dialogue.ShowTo whoToShow)
    {
        dialogueLisners.ForEach(lisner =>
        {
            lisner.ShowText(speaker, toSend, whoToShow);
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
