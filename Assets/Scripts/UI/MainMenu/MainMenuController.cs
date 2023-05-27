using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MainMenuController : MonoBehaviour
{
    public static MainMenuController Instance { get; private set; }

    [SerializeField] Toggle useGlasses;

    EventSystem eSystem;
    [SerializeField] GameObject FirstMenuButton, FirstSettingsButton, FirstChoosingButton, SettingsButton;

    Resolution[] resolutions;
    [SerializeField] Dropdown resolutionsDropdown;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        eSystem = EventSystem.current;
        eSystem.SetSelectedGameObject(null);
        eSystem.SetSelectedGameObject(FirstMenuButton);

        resolutions = Screen.resolutions;

        resolutionsDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].height == Screen.currentResolution.height && resolutions[i].width == Screen.currentResolution.width)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionsDropdown.AddOptions(options);
        resolutionsDropdown.value = currentResolutionIndex;
        resolutionsDropdown.RefreshShownValue();

    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void StartGame()
    {
        List<InputDetection.NewDevice> newDevices = InputDetection.Instance.GetAllDevices();
        if (newDevices.Count != 2) return;
        InputInfoHolder.Instance.SaveDevices(newDevices);
        InputInfoHolder.Instance.SetGlasses(useGlasses.isOn);
        SceneManager.LoadScene(1);
    }

    public void OpenSettings()
    {
        eSystem.SetSelectedGameObject(null);
        eSystem.SetSelectedGameObject(FirstSettingsButton);
    }
    public void CloseSettings()
    {
        eSystem.SetSelectedGameObject(null);
        eSystem.SetSelectedGameObject(SettingsButton);
    }
    public void OpenChoosing()
    {
        eSystem.SetSelectedGameObject(null);
        eSystem.SetSelectedGameObject(FirstChoosingButton);
    }
}
