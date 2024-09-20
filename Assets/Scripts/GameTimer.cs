using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public float startTime = 60f;
    public Text timerText;    
    private float timeRemaining;
    private bool timerRunning = false;    // הפעלה   
    
    public LevelManager levelManager; 

    void Start()
    {
        timeRemaining = startTime;
        timerRunning = true;    
    }

    void Update()    
    {        
        if (timerRunning)
        {            
            if (timeRemaining > 0)
                {                
                    timeRemaining -= Time.deltaTime;            
                    DisplayTime(timeRemaining);
                }
            else
                {
                    timeRemaining = 0;
                    timerRunning = false;
                    TimeUp();
                }
        }
    }    
        
    public void StartTimer()
    {
        timeRemaining = startTime;
        timerRunning = true;
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);          
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);        
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);    
    }
    
    
    // פונקציה לסיום הזמן    
    void TimeUp()
    {
        Debug.Log("Time's up!");        
        // כאן תוסיף את הלוגיקה של סיום המשחק
        GameManager.Instance.EndGame();
    }
}
