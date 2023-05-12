using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField]
    Dialogue toPlay;
    [SerializeField]
    bool takeControll;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            DialogueManager.instance.PlayDialogue(toPlay, takeControll);
            gameObject.SetActive(false);
        }
    }
}
