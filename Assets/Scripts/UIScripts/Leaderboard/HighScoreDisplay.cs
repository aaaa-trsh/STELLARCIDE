using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UIScripts.Leaderboard
{
    public class HighScoreDisplay : MonoBehaviour
    {
        public TMP_Text nameText;
        public TMP_Text scoreText;

        public void DisplayHighScore(string name, int score)
        {
            nameText.text = name;
            scoreText.text = string.Format("{0:0}", score);
        }

        public void HideEntryDisplay()
        {
            nameText.text = "NONE";
            scoreText.text = "0";
        }
    }
}