using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController gameController;
    public static PlayerController scientist;
    public static PlayerController solider;

    public static bool playersConnected;

    public static Difficulty difficulty;




    private void Awake()
    {
        if (gameController == null) gameController = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        
    }

    public void AddPlayer(PlayerController player)
    {
        if(player.isScientist)
            scientist = player;
        else
            solider = player;
    }

    public void DestroyedScientistDoor()
    {
        Debug.Log("You loose");
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }
}
