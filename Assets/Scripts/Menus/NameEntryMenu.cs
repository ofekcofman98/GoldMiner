using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameEntryMenu : MonoBehaviour, IMenu
{
    public void Show()
    {
        gameObject.SetActive(true);
        // MenuManager.Instance.ShowMenu(MenuManager.MenuType.NameEntry);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void OnSubmitNameButtonClicked()
    {
        MenuManager.Instance.HandleButtonClicked(() =>
        {
            string playerName = CanvasManager.Instance.GetEnteredName();

            if (!string.IsNullOrEmpty(playerName))
            {
                HiScoreManager.Instance.UpdatePlayerNameForTopScore(playerName);
                MenuManager.Instance.HideAllMenus();
                CanvasManager.Instance.ClearNameInput();
                MenuManager.Instance.ShowMenu(MenuManager.MenuType.TopScore);
            }
            else
            {
                Debug.LogWarning("Player name cannot be empty!");
            }
        });
    }

}
