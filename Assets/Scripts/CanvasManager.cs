using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;


public class CanvasManager : Singleton<CanvasManager>
{
    // [Header("Score Texts")]
    [SerializeField] private TextMeshProUGUI _currentScoreText;
    [SerializeField] private TextMeshProUGUI _hiScoreText;
    [SerializeField] private TextMeshProUGUI _topScoresText; 
    [SerializeField] private TextMeshProUGUI _goalScoreText;
    [SerializeField] private TextMeshProUGUI _goalScoreInLeveLMenuText;
    [SerializeField] private TextMeshProUGUI _itemScoreText; 

    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _timeBonusText;
    [SerializeField] private TextMeshProUGUI _levelNumberText;
    [SerializeField] private TextMeshProUGUI _levelCommentText; 

    [SerializeField] private List<Image> _lifeImages;
    [SerializeField] private GameObject _lifePanel;

    [SerializeField] private List<Image> _storedItemsImages;
    [SerializeField] private GameObject _storedItemsPanel;

    [SerializeField] private Camera _mainCamera; 

    [SerializeField] private TMP_InputField nameInputField; 


    private void Start()
    {
        if (_itemScoreText != null)
        {
            _itemScoreText.gameObject.SetActive(false);
        }
        if (_timeBonusText != null)
        {
            _timeBonusText.gameObject.SetActive(false);
        }
    }

    public void UpdateScoreText(int currentScore)
    {
        if (_currentScoreText != null)
        {
            _currentScoreText.text = $"Score: {currentScore}$";
        }
        else
        {
            Debug.LogWarning("Score text is not assigned in the inspector!");
        }
    }

    public void UpdateHiScore(int hiScore)
    {
        // _hiScoreText.text = hiScore.ToString();
        if (_hiScoreText != null)
        {
            _hiScoreText.text = $"HiScore: {hiScore}$";
        }
    }

    public void UpdateTopScores()
    {
        List<HiScoreManager.ScoreEntry> topScoresList = HiScoreManager.Instance.GetTopScores();
        string displayText = "";
        for (int i = 0; i < topScoresList.Count; i++)
        {
            displayText += $"{i+1}. {topScoresList[i].playerName}: {topScoresList[i].score}\n";
        }

        if (_topScoresText != null)
        {
            _topScoresText.text = displayText;
        }
        else
        {
            Debug.LogWarning("Top scores text is not found.");
        }
    }

    public void UpdateGoalScore(int cumulativeScoreGoal)
    {
        if (_goalScoreText != null)
        {
            _goalScoreText.text = $"Goal: {cumulativeScoreGoal}$";
        }
    }

    public void UpdateGoalScoreInLevelStartMenu(int goalScore)
    {
        if (_goalScoreInLeveLMenuText != null)
        {
            _goalScoreInLeveLMenuText.text = $"Goal: {goalScore}$";
        }
    }

    public void UpdateLevelNumberText(int levelNumber)
    {
        if (_levelNumberText != null)
        {
            _levelNumberText.text = $"Level Number {levelNumber}";
        }
    }

    public void UpdateLevelComment(string comment)
    {
        if (_levelCommentText != null)
        {
            _levelCommentText.text = comment;
        }
    }


    public void UpdateTimerText(float timeToDisplay)
    {
        if (_timerText != null)
        {
            timeToDisplay += 1;
            float minutes = Mathf.FloorToInt(timeToDisplay / 60);          
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);        
            _timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        }
        else
        {
            Debug.LogWarning("Timer text is not assigned in the inspector!");
        }
    }

    public void ShowTimeBonusText(float seconds)
    {
        if (_timeBonusText != null)
        {
            _timeBonusText.text = $"+{seconds}";
            _timeBonusText.gameObject.SetActive(true);
            StartCoroutine(HideTextAfterDelay(_timeBonusText));
        }
    }

    public string GetEnteredName()
    {
        return nameInputField.text;
    }

    public void ClearNameInput()
    {
        nameInputField.text = string.Empty;
    }

    public void ShowItemScore(int score, Vector3 clawWorldPosition)
    {
        if (_itemScoreText != null)
        {
            _itemScoreText.text = $"+{score}$";
            Vector3 screenPosition = _mainCamera.WorldToScreenPoint(clawWorldPosition);
            _itemScoreText.transform.position = screenPosition + new Vector3(50, 30, 0); // Adjust Y position to place it above the claw
            _itemScoreText.gameObject.SetActive(true);
            StartCoroutine(HideTextAfterDelay(_itemScoreText));
        }
    }

    private IEnumerator HideTextAfterDelay(TextMeshProUGUI i_text)
    {
        yield return new WaitForSeconds(2f);
        i_text.gameObject.SetActive(false);
    }
    public void ShowStoredItemsPanel()
    {
        _storedItemsPanel.gameObject.SetActive(true);
    }

    public void HideStoredItemsPanel()
    {
        _storedItemsPanel.gameObject.SetActive(false);
    }

    public void ShowLifePanel()
    {
        _lifePanel.gameObject.SetActive(true);
    }

    public void HideLifePanel()
    {
        _lifePanel.gameObject.SetActive(false);
    }
    public void UpdateLife(int currentLife)
    {
        foreach (var lifeImage in _lifeImages)
        {
            lifeImage.enabled = false;
            lifeImage.gameObject.SetActive(false);
        }

        Sprite lifeSprite = LifeManager.Instance.GetSprite();
        for (int i = 0; i < currentLife; i++)
        {
            _lifeImages[i].gameObject.SetActive(true);
            Image lifeImage = _lifeImages[i];
            lifeImage.enabled = true;
            
            lifeImage.sprite = lifeSprite;
        }
    }

    public void UpdateStoredItemsContainer(List<IBooster> storedItemsList)
    {
        foreach (var itemImage in _storedItemsImages)
        {
            itemImage.enabled = false;
            itemImage.gameObject.SetActive(false);
        }

        for (int i = 0; i < storedItemsList.Count; i++)
        {
            _storedItemsImages[i].gameObject.SetActive(true);
            Image itemImage = _storedItemsImages[i];
            itemImage.enabled = true;

            if (storedItemsList[i] is BoosterItem boosterItem)
            {
                itemImage.sprite = boosterItem.sprite;
            }
        }
    }


}


