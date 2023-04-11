using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController gameController;

    private void Awake()
    {
        if (gameController == null) gameController = this;
        else Destroy(gameObject);
    }

    public void DestroyedScientistDoor()
    {
        Debug.Log("You loose");
    }
}
