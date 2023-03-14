using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacingController : MonoBehaviour
{
    public static PacingController pacingController;

    [SerializeField]
    float pacing;
    [SerializeField]
    float updateDelay;

    float nextUpdate;


    private void Awake()
    {
        if (pacingController == null) pacingController = this;
        else Destroy(gameObject);
    }


    private void Update()
    {
        if (nextUpdate < Time.time)
        {
            UpdatePacing();
            nextUpdate = Time.time + updateDelay;
        }
    }

    void UpdatePacing()
    {

        if (pacing < 0.25f) // its too easy
        {
            EventManager.eventManager.TriggerEvent(pacing);
        }
        else if (pacing < 0.4f) // slightly too easy
        {
            EventManager.eventManager.TriggerEvent(pacing);
        }
        else if (pacing < 0.6f) // good balance
        {

        }
        else if (pacing < 0.75f) // slightly too hard
        {
            EventManager.eventManager.TriggerEvent(pacing);
        }
        else // too hard
        {
            EventManager.eventManager.TriggerEvent(pacing);
        }
    }
}
