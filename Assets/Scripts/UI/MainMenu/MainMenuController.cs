using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public static MainMenuController Instance { get; private set; }

    [SerializeField] Toggle useGlasses;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartGame()
    {
        List<InputDetection.NewDevice> newDevices = InputDetection.Instance.GetAllDevices();
        if (newDevices.Count != 2) return;
        InputInfoHolder.Instance.SaveDevices(newDevices);
        InputInfoHolder.Instance.SetGlasses(useGlasses.isOn);
        SceneManager.LoadScene(1);
    }
}
