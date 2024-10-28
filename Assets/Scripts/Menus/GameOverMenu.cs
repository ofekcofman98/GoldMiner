using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenu : MonoBehaviour, IMenu
{
    public void Show()
    {
        gameObject.SetActive(true);
        // MenuManager.Instance.ShowMenu(MenuManager.MenuType.GameOver);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
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
