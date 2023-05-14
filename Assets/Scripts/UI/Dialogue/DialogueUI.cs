using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    public enum DialogueStatus {Ready, Playing }

    [SerializeField]
    TextMeshProUGUI dialogueText;

    [SerializeField] Transform parrent;

    [SerializeField] PlayerController playerController;

    Dialogue playing;

    private void Start()
    {
        DialogueManager.instance.AddLisner(this, playerController.isScientist);
    }

    public DialogueStatus GetStatus()
    {
        if (playing == null) return DialogueStatus.Ready;
        else return DialogueStatus.Playing;
    }
    
    public void ShowText(Dialogue.DialougeField dialogue)
    {
        parrent.gameObject.SetActive(true);
        dialogueText.text = "[" + dialogue.speaker.name + "] " + dialogue.text;
    }

    public void ShowDialogue(Dialogue dialogue)
    {
        StopCoroutine("PlayDialogue");
        playing = dialogue;
        StartCoroutine("PlayDialogue");
    }

    IEnumerator PlayDialogue()
    {
        int step = 0;
        while (step < playing.GetDialouge().Count)
        {
            ShowText(playing.GetDialouge()[step]);
            yield return new WaitForSeconds(playing.GetDialouge()[step].timeOnScreen);
            step++;
        }
        parrent.gameObject.SetActive(false);
    }
}
