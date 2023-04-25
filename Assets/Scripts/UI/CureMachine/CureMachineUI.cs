using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CureMachineUI : MonoBehaviour
{
    [SerializeField] GameObject cureMachine;
    [SerializeField] bool active;
    [SerializeField] Slider progressSlider;
    [SerializeField] GameObject noPower;
    public void Open(bool on)
    {
        if (on) cureMachine.SetActive(true);
        else cureMachine.SetActive(false);
        active = on;
        noPower.SetActive(!CureMachine.Instance.IsPowered());
    }

    private void Update()
    {
        if(active)
        {
            progressSlider.value = CureController.instance.GetCureCurrentProgress();
        }
    }
}
