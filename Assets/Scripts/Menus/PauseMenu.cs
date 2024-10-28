using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour, IMenu
{
    public void Show()
    {
        gameObject.SetActive(true);
        MenuManager.Instance.StopTime();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void OnContinueButtonClicked()
    {
        Hide();
        CanvasManager.Instance.ShowLifePanel();
        CanvasManager.Instance.ShowStoredItemsPanel();
        MenuManager.Instance.RunTime();
    }

    public void OnExitButtonClicked()
    {
        Hide();
        MenuManager.Instance.ShowMenu(MenuManager.MenuType.MainMenu);
    }

}
