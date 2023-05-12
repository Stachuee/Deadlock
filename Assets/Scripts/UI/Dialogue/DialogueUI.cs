using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI dialogueText;

    [SerializeField] Transform parrent;

    [SerializeField] PlayerController playerController;

    private void Start()
    {
        DialogueManager.instance.AddLisner(this);
    }

    public void HideText()
    {
        parrent.gameObject.SetActive(false);
    }

    public void ShowText(string speaker, string text, Dialogue.ShowTo whoToShow)
    {
        if ((whoToShow == Dialogue.ShowTo.All) || (whoToShow == Dialogue.ShowTo.Scientist && playerController.isScientist) || (whoToShow == Dialogue.ShowTo.Solider && !playerController.isScientist))
        {
            parrent.gameObject.SetActive(true);
            dialogueText.text = "[" + speaker + "] " + text;
        }
    }
}
