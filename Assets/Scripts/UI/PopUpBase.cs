using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public abstract class PopUpBase : MonoBehaviour
{
    [System.Serializable]
    struct Anchor
    {
        public Transform screenPosition;
        public bool active;
    }

    [SerializeField]
    Anchor anchor;


    [SerializeField]
    UnityEngine.UI.Button firstButton;
    [SerializeField]
    UnityEngine.UI.Button activeButton;

    [SerializeField]
    PopUpBase back;

    private void Awake()
    {
        transform.position = anchor.screenPosition.position;
        Destroy(anchor.screenPosition.gameObject);
        gameObject.SetActive(anchor.active);
    }

    
    public void OpenPupUp()
    {
        activeButton = firstButton;
    }

}
