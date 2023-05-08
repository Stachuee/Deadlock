using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI dialogueText;

    [SerializeField] Transform parrent;

    private void Start()
    {
        DialogueManager.instance.AddLisner(this);
    }

    public void HideText()
    {
        parrent.gameObject.SetActive(false);
    }

    public void ShowText(string speaker, string text)
    {
        parrent.gameObject.SetActive(true);
        dialogueText.text = "[" + speaker + "] " + text;
    }
}
