using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController gameController;
    public static List<PlayerController> players;


    private void Awake()
    {
        if (gameController == null) gameController = this;
        else Destroy(gameObject);
    }

    public void SetUp()
    {
        players = new List<PlayerController>();
    }

    public void AddPlayer(PlayerController player)
    {
        if(players == null)
        {
            players = new List<PlayerController>();
        }
        players.Add(player);
    }

    public void DestroyedScientistDoor()
    {
        Debug.Log("You loose");
    }
}
