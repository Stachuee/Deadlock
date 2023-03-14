using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
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
