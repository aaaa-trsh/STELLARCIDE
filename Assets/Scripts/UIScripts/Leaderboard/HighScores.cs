using System.Collections.Generic;
using UnityEngine;

namespace UIScripts.Leaderboard
{
    public class HighScores : MonoBehaviour
    {
        public HighScoreDisplay[] highScoreDisplayArray;
        public List<HighScoreEntry> scores = new List<HighScoreEntry>();
        public int leaderboardMaximum = 10;

        void OnEnable()
        {
            GetScores();
            UpdateDisplay();
        }

        void UpdateDisplay()
        {
            for (int i = 0; i < highScoreDisplayArray.Length; i++)
            {
                if (i < scores.Count)
                {
                    highScoreDisplayArray[i].DisplayHighScore(scores[i].name, scores[i].score);
                }
                else
                {
                    highScoreDisplayArray[i].HideEntryDisplay();
                }
            }
        }

        void GetScores()
        {
            scores = GameManager.Instance.xmlManager.LoadScores();
        }
    }

    public class HighScoreEntry
    {
        public string name;
        public int score;
    }
}