using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [SerializeField]
    List<Dialouge> allDialogues;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

}
