using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuManager : Singleton<MenuManager>
{
    public GameObject mainMenuPanel;
    public GameObject topScoresPanel;
    public GameObject levelStartPanel;
    public GameObject pausePanel;

    private List<GameObject> menusList;
    
    private void Start()
    {
        menusList = new List<GameObject>
        {
            mainMenuPanel,
            topScoresPanel,
            levelStartPanel,
            pausePanel
        };

        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        ShowMenu(mainMenuPanel);
        StopTime();
    }
    

    public void ShowTopScoresMenu()
    {
        ShowMenu(topScoresPanel);
        StopTime();
    }

    public void ShowLevelStartPanel()
    {
        ShowMenu(levelStartPanel);
        StopTime();
    }
    
    public void ShowPausePanel()
    {
        ShowMenu(pausePanel);
        StopTime();
    }

    // public void ResumeGame()
    // {
    //     ShowMenu(null);
    //     RunTime();
    // }

    public void OnStartGameButtonClicked()
    {
        HideAllMenus();
        RunTime();
        GameManager.Instance.StartGame();
    }

    private void HideAllMenus()
    {
        foreach (var menu in menusList)
        {
            if (menu != null)
            {
                menu.SetActive(false);
            }
        }
    }

    public void ShowMenu(GameObject menuToShow)
    {
        HideAllMenus();

        if (menuToShow != null)
        {
            menuToShow.SetActive(true);
        }
        else
        {
            Debug.LogError("menu not found");
        }
    }

    private void StopTime()
    {
        Time.timeScale = 0;
    }

    private void RunTime()
    {
        Time.timeScale = 1;
    }

    public void ExitGame()
    {
        Debug.Log("Exiting Game");
        Application.Quit();
    }

}
