using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public enum CurrentPauseMenuPanel
{
    Main,
    Settings
}
public class PauseMenuController : MonoBehaviour
{
    EventSystem eSystem;
    [SerializeField] GameObject FirstPauseButton, FirstSettingsButton, SettingsButton;


    CurrentPauseMenuPanel currentPanel;


    void Start()
    {
        eSystem = EventSystem.current;
        eSystem.SetSelectedGameObject(null);
        eSystem.SetSelectedGameObject(FirstPauseButton);
        currentPanel = CurrentPauseMenuPanel.Main;
    }

    private void Update()
    {
        if (eSystem.currentSelectedGameObject == null)
            StartCoroutine("SetSelected");
    }

    public void SetLostSelectedGameObject()
    {
        switch (currentPanel)
        {
            case CurrentPauseMenuPanel.Main:
                eSystem.SetSelectedGameObject(FirstPauseButton);
                break;
            case CurrentPauseMenuPanel.Settings:
                eSystem.SetSelectedGameObject(FirstSettingsButton);
                break;
            default:
                eSystem.SetSelectedGameObject(FirstPauseButton);
                break;
        }
    }

    public void OpenSettings()
    {
        currentPanel = CurrentPauseMenuPanel.Settings;
        eSystem.SetSelectedGameObject(null);
        eSystem.SetSelectedGameObject(FirstSettingsButton);
    }
    public void CloseSettings()
    {
        currentPanel = CurrentPauseMenuPanel.Main;
        eSystem.SetSelectedGameObject(null);
        eSystem.SetSelectedGameObject(SettingsButton);
    }

    void Resume()
    {
        Time.timeScale = 1;
    }

    void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    IEnumerator SetSelected()
    {
        yield return new WaitForEndOfFrame();
        SetLostSelectedGameObject();
    }

}
