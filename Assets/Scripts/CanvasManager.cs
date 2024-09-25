using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class CanvasManager : Singleton<CanvasManager>
{
    [SerializeField] private TextMeshProUGUI _currentScoreText;
    [SerializeField] private TextMeshProUGUI _hiScoreText;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _itemScoreText; 
    [SerializeField] private Camera _mainCamera; 

    private void Start()
    {
        if (_itemScoreText != null)
        {
            _itemScoreText.gameObject.SetActive(false);
        }
    }

    public void UpdateScoreText(int currentScore)
    {
        if (_currentScoreText != null)
        {
            _currentScoreText.text = $"Score: {currentScore}";
        }
        else
        {
            Debug.LogWarning("Score text is not assigned in the inspector!");
        }
    }

    public void UpdateHiScore(int hiScore)
    {
        _hiScoreText.text = hiScore.ToString();
    }


    public void UpdateTimerText(float timeToDisplay)
    {
        if (_timerText != null)
        {
            timeToDisplay += 1;
            float minutes = Mathf.FloorToInt(timeToDisplay / 60);          
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);        
            _timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);    
        }
        else
        {
            Debug.LogWarning("Timer text is not assigned in the inspector!");
        }
    }

     public void ShowItemScore(int score, Vector3 clawWorldPosition)
    {
        if (_itemScoreText != null)
        {
            _itemScoreText.text = $"+{score}";
            Vector3 screenPosition = _mainCamera.WorldToScreenPoint(clawWorldPosition);
            _itemScoreText.transform.position = screenPosition + new Vector3(50, 30, 0); // Adjust Y position to place it above the claw
            _itemScoreText.gameObject.SetActive(true);
            StartCoroutine(HideItemScoreAfterDelay());
        }
    }

    
    private IEnumerator HideItemScoreAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        _itemScoreText.gameObject.SetActive(false);
    }

}


