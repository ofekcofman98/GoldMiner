using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiScoreManager : Singleton<HiScoreManager>
{
    private const int MaxTopScores = 5;

    public struct ScoreEntry
    {
        public string playerName;
        public int score;

        public ScoreEntry(string playerName, int score)
        {
            this.playerName = playerName;
            this.score = score;
        }

    }

    private List<ScoreEntry> topScores = new List<ScoreEntry>();

    void Start()
    {
        LoadTopFiveScores();
    }

    public void LoadTopFiveScores()
    {
        topScores.Clear();
        for (int i = 0; i < MaxTopScores; i++)
        {
            int score = PlayerPrefs.GetInt($"TopScore{i}", 0);
            string name = PlayerPrefs.GetString($"TopName{i}", "Unknown");
            topScores.Add(new ScoreEntry(name, score));
        }
    }

    private void SaveTopFiveScores()
    {
        for (int i = 0; i < MaxTopScores; i++)
        {
            PlayerPrefs.SetInt($"TopScore{i}", topScores[i].score);
            PlayerPrefs.SetString($"TopName{i}", topScores[i].playerName);
        }
        PlayerPrefs.Save();
    }

    public void CheckForTopFive(int currentScore, string playerName)
    {
        for (int i = 0; i < topScores.Count; i++)
        {
            if (currentScore > topScores[i].score)
            {
                topScores.Insert(i, new ScoreEntry(playerName, currentScore));
                
                if (topScores.Count > MaxTopScores)
                {
                    topScores.RemoveAt(MaxTopScores);
                }
                SaveTopFiveScores();
                Debug.Log($"New score added to top 5: {currentScore} by {playerName}");
                CanvasManager.Instance.UpdateHiScore(topScores[0].score);
                break;
            }
        }
    }

    public void ResetTopFiveScores()
    {
        for (int i = 0; i < MaxTopScores; i++)
        {
            PlayerPrefs.DeleteKey($"TopScore{i}");
            PlayerPrefs.DeleteKey($"TopName{i}");
        }
        PlayerPrefs.Save();
        LoadTopFiveScores();
        Debug.Log("Top 5 scores reset.");
    }

    public List<ScoreEntry> GetTopScores()
    {
        return new List<ScoreEntry>(topScores);
    }

}


