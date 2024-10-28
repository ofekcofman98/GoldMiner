using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopScoresMenu : MonoBehaviour, IMenu
{
    public void Show()
    {
        gameObject.SetActive(true);
        // MenuManager.Instance.ShowMenu(MenuManager.MenuType.mainMenu);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void OnBackButtonClicked()
    {
        MenuManager.Instance.HandleButtonClicked(() =>
        {
            MenuManager.Instance.ShowMenu(MenuManager.MenuType.MainMenu);
        });
    }
}
