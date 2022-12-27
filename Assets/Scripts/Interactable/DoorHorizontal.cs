using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHorizontal : MonoBehaviour
{
    [SerializeField] float timeToOpen;
    [SerializeField] Vector2 openOffset;
    Vector2 startingPos;

    float startOpening;

    bool open;
    bool inOperation;

    private void Awake()
    {
        startingPos = transform.position;
    }

    public void OpenDoor()
    {
        if (!inOperation)
        {
            if (!open) StartCoroutine("Open");
            else StartCoroutine("Close");
        }
    }

    IEnumerator Open()
    {
        inOperation = true;
        startOpening = Time.time;
        do
        {
            transform.position = Vector2.Lerp(startingPos, startingPos + openOffset, (Time.time - startOpening) / timeToOpen);
            yield return new WaitForSeconds(0);
        }
        while (Vector2.Distance(transform.position, startingPos + openOffset) > 0.1f);
        inOperation = false;
        open = true;
    }

    IEnumerator Close()
    {
        inOperation = true;
        startOpening = Time.time;
        do
        {
            transform.position = Vector2.Lerp(startingPos + openOffset, startingPos, (Time.time - startOpening) / timeToOpen);
            yield return new WaitForSeconds(0);
        }
        while (Vector2.Distance(startingPos, transform.position) > 0.1f);
        inOperation = false;
        open = false;
    }
}
