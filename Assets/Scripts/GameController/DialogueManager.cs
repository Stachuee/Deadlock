using GD.MinMaxSlider;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [SerializeField]
    List<Dialogue> newItemDialogues;
    [SerializeField]
    List<Dialogue> quipDialogues;
    [SerializeField]
    Dialogue startingDialogue;

    [SerializeField, MinMaxSlider(0, 360)] Vector2 minMaxQuipTimer; 

    Dialogue currentDialouge;

    DialogueUI dialogueLisnerScientist;
    DialogueUI dialogueLisnerSolider;


    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void StartQuips()
    {
        PlayDialogue(startingDialogue, true);
        StartCoroutine("NewQuip");
    }


    public IEnumerator NewQuip()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(minMaxQuipTimer.x, minMaxQuipTimer.y));
            TriggerDialogue(Dialogue.Trigger.Random, false);
        }
    }

    public void TriggerDialogue(Dialogue.Trigger trigger, bool cutPlaying = false)
    {
        switch(trigger)
        {
            case Dialogue.Trigger.Kill:
                break;
            case Dialogue.Trigger.OnNewItemPickup:
                PlayDialogue(newItemDialogues[Random.Range(0, newItemDialogues.Count)], cutPlaying);
                break;
            case Dialogue.Trigger.Random:
                Dialogue chosen = quipDialogues[Random.Range(0, quipDialogues.Count)];
                quipDialogues.Remove(chosen);
                PlayDialogue(chosen, cutPlaying);
                break;
        }
    }

    public void PlayDialogue(Dialogue toPlay, bool cutPlaying)
    {
        //if (toPlay.GetToShow() == Dialogue.ShowTo.Scientist && !(dialogueLisnerScientist.GetStatus() == DialogueUI.DialogueStatus.Playing && !cutPlaying))
        //{
        //    dialogueLisnerScientist.ShowDialogue(toPlay);
        //}
        //else if (toPlay.GetToShow() == Dialogue.ShowTo.Solider && !(dialogueLisnerSolider.GetStatus() == DialogueUI.DialogueStatus.Playing && !cutPlaying))
        //{
        //    dialogueLisnerSolider.ShowDialogue(toPlay);
        //}
        //else if(!(dialogueLisnerScientist.GetStatus() == DialogueUI.DialogueStatus.Playing && !cutPlaying) && !(dialogueLisnerSolider.GetStatus() == DialogueUI.DialogueStatus.Playing && !cutPlaying))
        //{
        //    dialogueLisnerScientist.ShowDialogue(toPlay);
        //    dialogueLisnerSolider.ShowDialogue(toPlay);
        //}
    }


    public void AddLisner(DialogueUI toAdd, bool scientist)
    {
        if (scientist)
        {
            dialogueLisnerScientist = toAdd;
        }
        else
        {
            dialogueLisnerSolider = toAdd;
        }
        
    }

}
