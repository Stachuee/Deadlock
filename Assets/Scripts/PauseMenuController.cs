using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public enum CurrentPauseMenuPanel
{
    Main,
    Settings,
    Confirmation
}
public class PauseMenuController : MonoBehaviour
{
    EventSystem eSystem;
    [SerializeField] GameObject FirstPauseButton, FirstSettingsButton, SettingsButton, ExitButton, YesButton, ConfirmationWindow, PauseWindow, OptionsWindow;


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

        PauseWindow.SetActive(false);

        OptionsWindow.SetActive(true);
    }
    public void CloseSettings()
    {
        currentPanel = CurrentPauseMenuPanel.Main;
        eSystem.SetSelectedGameObject(null);
        eSystem.SetSelectedGameObject(SettingsButton);

        PauseWindow.SetActive(true);

        OptionsWindow.SetActive(false);
    }


    public void OpenExitConfirmation()
    {
        currentPanel = CurrentPauseMenuPanel.Confirmation;
        eSystem.SetSelectedGameObject(null);
        eSystem.SetSelectedGameObject(YesButton);

        PauseWindow.SetActive(false);

        ConfirmationWindow.SetActive(true);
    }

    public void CloseExitConfirmation()
    {
        currentPanel = CurrentPauseMenuPanel.Main;
        eSystem.SetSelectedGameObject(null);
        eSystem.SetSelectedGameObject(ExitButton);

        PauseWindow.SetActive(true);

        ConfirmationWindow.SetActive(false);
    }

    public void Resume()
    {
        Time.timeScale = 1;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    IEnumerator SetSelected()
    {
        yield return new WaitForEndOfFrame();
        SetLostSelectedGameObject();
    }

}
