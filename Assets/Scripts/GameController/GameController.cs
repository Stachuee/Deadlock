using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController gameController;
    public static PlayerController scientist;
    public static PlayerController solider;

    public static bool playersConnected;

    [SerializeField]
    List<DangerLevelSO> dangerLevels;
    [SerializeField] Difficulty difficulty;

    public static DangerLevelSO currentDangerLevel;


    private void Awake()
    {
        if (gameController == null) gameController = this;
        else Destroy(gameObject);
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

    public void IncreaseLevel(int level)
    {
        currentDangerLevel = dangerLevels.Find(x => x.GetDangerLevel() == level && x.GetDifficulty() == difficulty);
    }
}
