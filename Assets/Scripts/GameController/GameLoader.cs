using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameLoader : MonoBehaviour
{
    public static GameLoader instance { get; private set; }


    PlayerInputManager playerInputManager;

    private void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(gameObject);

        playerInputManager = transform.GetComponent<PlayerInputManager>();
    }

    private void Start()
    {
        if(InputInfoHolder.Instance != null)
        {
            List<InputDetection.NewDevice> devices = InputInfoHolder.Instance.GetDevices();

            playerInputManager.JoinPlayer(0, 0, devices[0].controlScheme, devices[0].device).gameObject.transform.GetComponent<PlayerController>()
                .SetUpPlayer(devices[0].controlScheme, 0, devices[0].scientist);

            playerInputManager.JoinPlayer(1, 0, devices[1].controlScheme, devices[1].device).gameObject.transform.GetComponent<PlayerController>()
                .SetUpPlayer(devices[1].controlScheme, 1, devices[1].scientist);
        }
        else
        {
            Debug.LogError("No player info");
            playerInputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersWhenButtonIsPressed;
        }
    }
}
