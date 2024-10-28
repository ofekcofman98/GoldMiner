using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStartMenu : MonoBehaviour, IMenu
{
    public void Show()
    {
        gameObject.SetActive(true);

        Debug.Log("HIIIIIIIIIII");
        int levelNumber = GameManager.Instance.GetCurrentLevelNumber();
        Level currentLevel = GameManager.Instance.GetCurrentLevel();
        int cumulativeScoreGoal= GameManager.Instance.GetCumulativeScoreGoal();

        CanvasManager.Instance.UpdateLevelNumberText(levelNumber);
        CanvasManager.Instance.UpdateGoalScoreInLevelStartMenu(cumulativeScoreGoal);
        CanvasManager.Instance.UpdateLevelComment(currentLevel.comment);        
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void OnLevelStartButtonClicked()
    {
        MenuManager.Instance.HandleButtonClicked(() => 
        {
            MenuManager.Instance.HideMenu(MenuManager.MenuType.LevelStart);
            LevelManager.Instance.StartLevel();
            MenuManager.Instance.RunTime();
        });
    }


}
