using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiScoreManager : Singleton<HiScoreManager>
{
    private const int MaxTopScores = 5;
    private bool isPlayerInTopFive;
    private int playerPosition;

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
        isPlayerInTopFive = false;
        playerPosition = -1;
    }

    public void LoadTopFiveScores()
    {
        topScores.Clear();
        for (int i = 0; i < MaxTopScores; i++)
        {
            int score = PlayerPrefs.GetInt($"TopScore{i}", 0);
            string name = PlayerPrefs.GetString($"TopName{i}", "Unknown");
            Debug.Log($"Loaded score {i}: {name} - {score}");
            topScores.Add(new ScoreEntry(name, score));
        }
    }

    private void SaveTopFiveScores()
    {
        for (int i = 0; i < topScores.Count; i++)
        {
            Debug.Log($"Saving score {i}: {topScores[i].playerName} - {topScores[i].score}");
            PlayerPrefs.SetInt($"TopScore{i}", topScores[i].score);
            PlayerPrefs.SetString($"TopName{i}", topScores[i].playerName);
        }
        PlayerPrefs.Save();
    }

    public void CheckForTopFive(int currentScore, string playerName)
    {
        // Create a new score entry with "Player" as a placeholder
        ScoreEntry newScore = new ScoreEntry(playerName, currentScore);

        // Check if the current score qualifies for the top 5
        for (int i = 0; i < topScores.Count; i++)
        {
            if (currentScore > topScores[i].score)
            {
                // Insert the score at the correct position
                topScores.Insert(i, newScore);

                // Ensure the list contains only 5 scores
                if (topScores.Count > MaxTopScores)
                {
                    topScores.RemoveAt(MaxTopScores);  // Remove the lowest score if needed
                }

                SaveTopFiveScores();  // Save top 5 to PlayerPrefs

                CanvasManager.Instance.UpdateHiScore(topScores[0].score);  // Update the hiScore on the screen
                Debug.Log($"New score added to top 5: {currentScore} by Player at position {i}");

                // Set flags for name updating
                isPlayerInTopFive = true;
                playerPosition = i;
                return;
            }
        }

        // Handle case where the score is lower than the top 5
        if (topScores.Count < MaxTopScores)
        {
            topScores.Add(newScore);  // Add the score at the bottom if the list isn't full
            SaveTopFiveScores();
        }
    }


    public void UpdatePlayerNameForTopScore(string playerName)
    {
        // Only proceed if the player is in the top 5
        if (isPlayerInTopFive && playerPosition >= 0 && playerPosition < topScores.Count)
        {
            // Replace the placeholder "Player" with the actual name entered by the player
            topScores[playerPosition] = new ScoreEntry(playerName, topScores[playerPosition].score);

            // Save the updated top scores with the correct player name
            SaveTopFiveScores();
            Debug.Log($"Player name updated to {playerName} for score {topScores[playerPosition].score}");
        }
        else
        {
            Debug.LogWarning("Player is not in the top 5 or invalid position");
        }
    }

    public bool CheckIfPlayerIsInTopFive()
    {
        return isPlayerInTopFive;
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


