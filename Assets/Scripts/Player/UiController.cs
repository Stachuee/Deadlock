using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;

public class UiController : MonoBehaviour
{
    [System.Serializable]
    struct Panel
    {
        public Transform panel;
        public Transform anchor;
        public bool isOn;
    }

    [SerializeField]
    List<Panel> panels;

    public FuseBoxUIScript fuseBox;
    public ComputerUI computer;

    public MultiplayerEventSystem myEventSystem;        

    private void Start()
    {
        panels.ForEach(x =>
        {
            x.panel.transform.position = x.anchor.transform.position;
            x.panel.gameObject.SetActive(x.isOn);

            Destroy(x.anchor.gameObject);
        });
    }

    public void Computer()
    {
        
    }
}
