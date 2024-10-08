using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager : Singleton<LifeManager>
{
    [SerializeField] private int _lifeNumber;
    [SerializeField] private Sprite _lifeSprite;

    private int _currentLifeNumber;

    public void SetLifeNumberToMax()
    {
        _currentLifeNumber = _lifeNumber;
    }

    public void IncreaseLife(int numberOfLifesToAdd)
    {
        _currentLifeNumber += numberOfLifesToAdd;
        Debug.Log($"life increased by {numberOfLifesToAdd}. current life: {_currentLifeNumber}");
        ShowLifeOnScreen();
    }

    public void DecreaseLife(int numberOfLifesToDecrease)
    {
        _currentLifeNumber -= numberOfLifesToDecrease;
        Debug.Log($"life decreased by {numberOfLifesToDecrease}. current life: {_currentLifeNumber}");
        ShowLifeOnScreen();

        if (CheckIfNoLife())
        {
            GameManager.Instance.EndGame();
        }
    }

    public void Disqualified()
    {
        _currentLifeNumber = 0;
    }

    public int GetLifeNumber()
    {
        return _lifeNumber;
    }

    public int GetCurrentLifeNumber()
    {
        return _currentLifeNumber;
    }

    private bool CheckIfNoLife()
    {
        return (_currentLifeNumber <= 0);
    }

    public Sprite GetSprite()
    {
        return _lifeSprite;
    }
    public void ShowLifeOnScreen()
    {
        CanvasManager.Instance.UpdateLife(_currentLifeNumber);
    }

}
