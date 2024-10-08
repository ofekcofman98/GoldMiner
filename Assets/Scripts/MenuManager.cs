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
    public GameObject nameEntryPanel;
    public GameObject BombPanel;
    public GameObject gameOverPanel;
    
    public AudioClip buttonClickSound; 

    private List<GameObject> menusList;
    
    private void Start()
    {
        menusList = new List<GameObject>
        {
            mainMenuPanel,
            topScoresPanel,
            levelStartPanel,
            pausePanel,
            nameEntryPanel,
            BombPanel,
            gameOverPanel
        };

        ShowMainMenu();
    }
    public void PlayButtonClickSound()
    {
        AudioManager.Instance.PlaySound(buttonClickSound);
    }


    public void ShowMainMenu()
    {
        ShowMenu(mainMenuPanel);
        StopTime();
    }
    

    public void ShowTopScoresMenu()
    {
        ShowMenu(topScoresPanel);
        CanvasManager.Instance.UpdateTopScores();
        StopTime();
    }

    public void ShowLevelStartPanel(int levelNumber, int goalScore)
    {
        ShowMenu(levelStartPanel);
        CanvasManager.Instance.UpdateLevelNumberText(levelNumber);
        CanvasManager.Instance.UpdateGoalScoreInLevelStartMenu(goalScore);
        CanvasManager.Instance.HideStoredItemsPanel();
        CanvasManager.Instance.HideLifePanel();
        StopTime();
    }

    public void ShowNameEntryPanel()
    {
        ShowMenu(nameEntryPanel);
        StopTime();
    }
    
    public void ShowPausePanel()
    {
        ShowMenu(pausePanel);
        StopTime();
    }

    public void ShowBombPanel()
    {
        ShowMenu(BombPanel);
        StopTime();
    }

    public void ShowGameOverMenu()
    {
        ShowMenu(gameOverPanel);
        StopTime();
    }

    public void OnStartGameButtonClicked()
    {
        PlayButtonClickSound();
        HideAllMenus();
        RunTime();
        GameManager.Instance.StartGame();
    }

    public void OnTop5ButtonClicked()
    {
        PlayButtonClickSound();
        HideAllMenus();
        ShowTopScoresMenu();
    }

    public void OnExitGameButtonClicked()
    {
        PlayButtonClickSound();
        HideAllMenus();
        ExitGame();
    }

    public void OnLevelStartButtonclicked()
    {
        PlayButtonClickSound();
        HideMenu(levelStartPanel);
        LevelManager.Instance.StartLevel();
        RunTime();
    }

    public void OnSubmutNameButtonClicked()
    {
        PlayButtonClickSound();
        string playerName = CanvasManager.Instance.GetEnteredName();

        if (!string.IsNullOrEmpty(playerName))
        {
            HiScoreManager.Instance.UpdatePlayerNameForTopScore(playerName);
            HideAllMenus();
            CanvasManager.Instance.ClearNameInput();
            ShowTopScoresMenu   ();
        }
        else
        {
            Debug.LogWarning("Player name cannot be empty!");
        }
    }

    public void OnBackButtonClicked()
    {
        PlayButtonClickSound();
        HideAllMenus();
        ShowMainMenu();
    }

    private void HideMenu(GameObject menuToHide)
    {
        if (menuToHide != null)
        {
            menuToHide.SetActive(false);
        }
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
