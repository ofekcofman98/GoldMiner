using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour, IMenu
{
    public void Show()
    {
        gameObject.SetActive(true);
        CanvasManager.Instance.UpdateTopScores();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void OnTop5ButtonClicked()
    {
        MenuManager.Instance.HandleButtonClicked(() =>
        {
            MenuManager.Instance.ShowMenu(MenuManager.MenuType.TopScore);
        });
    }

    public void OnStartGameButtonClicked()
    {
        MenuManager.Instance.HandleButtonClicked(() =>
        {
            MenuManager.Instance.HideAllMenus();
            MenuManager.Instance.RunTime();
            GameManager.Instance.StartGame();
        });
    }

    public void OnExitGameButtonClicked()
    {
        MenuManager.Instance.HandleButtonClicked(() => 
        {
            MenuManager.Instance.HideAllMenus();
            MenuManager.Instance.ExitGame();
        });
    }


}
