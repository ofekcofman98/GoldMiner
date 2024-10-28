using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class MenuManager : Singleton<MenuManager>
{
    public MainMenu mainMenu;
    public TopScoresMenu topScores;
    public LevelStartMenu levelStart;
    public PauseMenu pause;
    public NameEntryMenu nameEntry;
    public GameOverMenu gameOver;
    
    public AudioClip buttonClickSound; 

    private Dictionary<MenuType, IMenu> menusDict = new Dictionary<MenuType, IMenu>();

    public enum MenuType
    {
        MainMenu,
        TopScore,
        LevelStart,
        Pause,
        NameEntry,
        GameOver 
    }
    
    private void Start()
    {
            menusDict.Add(MenuType.MainMenu,     mainMenu);
            menusDict.Add(MenuType.TopScore,     topScores);
            menusDict.Add(MenuType.LevelStart,   levelStart);
            menusDict.Add(MenuType.Pause,        pause);
            menusDict.Add(MenuType.NameEntry,    nameEntry);
            menusDict.Add(MenuType.GameOver,     gameOver);
    }


    public void ShowMenu(MenuType menuType)
    {
        HideAllMenus();

        if (menusDict.TryGetValue(menuType, out IMenu menuToShow))
        {
            menuToShow.Show();
            StopTime();
        }
        else
        {
            Debug.LogError($"{menuType} not found");
        }
    }

    public void HideMenu(MenuType menuType)
    {
        if (menusDict.TryGetValue(menuType, out IMenu menuToHide))
        {
            menuToHide.Hide();
        }
    }


    public void HideAllMenus()
    {
        CanvasManager.Instance.HideLifePanel();
        CanvasManager.Instance.HideStoredItemsPanel();

        foreach (var menuToHide in menusDict.Values)
        {
            menuToHide.Hide();
        }
    }

    public void HandleButtonClicked(Action buttonAction)
    {
        PlayButtonClickSound();
        buttonAction?.Invoke();
    }


    public void PlayButtonClickSound()
    {
        AudioManager.Instance.PlaySound(buttonClickSound);
    }

    public void StopTime() => Time.timeScale = 0;
    public void RunTime() => Time.timeScale = 1;
    
    
    public void ExitGame()
    {
        Debug.Log("Exiting Game");
        Application.Quit();
    }

}
