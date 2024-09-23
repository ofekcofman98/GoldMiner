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
    }
    public void ShowMainMenu()
    {
        ShowMenu(mainMenuPanel);
    }

    public void ShowTopScoresMenu()
    {
        ShowMenu(topScoresPanel);
    }

    public void ShowLevelStartPanel()
    {
        ShowMenu(levelStartPanel);
    }
    
    public void ShowPausePanel()
    {
        ShowMenu(pausePanel);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        ShowMenu(null);
        Time.timeScale = 1;
    }

    public void ShowMenu(GameObject menuToShow)
    {
        if (menusList.Contains(menuToShow))
        {
            foreach (var menu in menusList)
            {
                if (menu != null)
                {
                    menu.SetActive(false);
                }
            }
            if (menuToShow != null)
            {
                menuToShow.SetActive(true);
            }
            else
            {
                Debug.LogError("Menu not found");
            }
        }
        else
        {
            Debug.LogError("Menu not found.");
        }
    }

    public void ExitGame()
    {
        Debug.Log("Exiting Game");
        Application.Quit();
    }

}
