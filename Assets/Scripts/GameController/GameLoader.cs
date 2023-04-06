using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameLoader : MonoBehaviour
{
    public static GameLoader instance { get; private set; }

    [SerializeField]
    Transform scientistAnchor;
    [SerializeField]
    Transform soldierAnchor;


    PlayerInputManager playerInputManager;

    private void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(gameObject);

        playerInputManager = transform.GetComponent<PlayerInputManager>();
    }

    private void Start()
    {
        //Generate map

        NavController.instance.CreateNavMesh();


        if(InputInfoHolder.Instance != null)
        {
            List<InputDetection.NewDevice> devices = InputInfoHolder.Instance.GetDevices();
            GameObject temp;

            for(int i = 0; i < 2; i++)
            {
                temp = playerInputManager.JoinPlayer(i, 0, devices[i].controlScheme, devices[i].device).gameObject;
                temp.transform.GetComponent<PlayerController>().SetUpPlayer(devices[i].controlScheme, i, devices[i].scientist);
                if (devices[i].scientist) temp.transform.position = scientistAnchor.position;
                else temp.transform.position = soldierAnchor.position;
            }

            //playerInputManager.JoinPlayer(1, 0, devices[1].controlScheme, devices[1].device).gameObject.transform.GetComponent<PlayerController>()
            //    .SetUpPlayer(devices[1].controlScheme, 1, devices[1].scientist);
        }
        else
        {
            Debug.LogError("No player info");
            playerInputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersWhenButtonIsPressed;
        }
    }
}
