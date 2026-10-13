using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UIScripts.Leaderboard
{
    public class HighScoreCounter : MonoBehaviour
    {
        [Header("References")] 
        public GameObject topLayer;
        public GameObject leaderboardgrid;
        public TextMeshProUGUI enemyCounter;
        public TextMeshProUGUI finalScoreCounter;
        public GameObject prompt;
        public Button returnButton;

        private string nameToSave;
        public int finalScore;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            // catch for menu version of scene
            if (!topLayer.activeInHierarchy) return;
 
            // scoring + activating high score prompt
            prompt.SetActive(false);
            int enemyPoints = 0;
            int _enemiesDefeated = 0;

            if (GameManager.Instance.scoreManager != null)
            {
                _enemiesDefeated = GameManager.Instance.scoreManager.enemiesDefeated;

                if (_enemiesDefeated != 0)
                {
                    enemyPoints = _enemiesDefeated * 20;
                }
            }
            
            enemyCounter.text = "ENEMIES SLAIN: " + _enemiesDefeated + " [" + enemyPoints + "]";
            finalScore = enemyPoints;
            List<HighScoreEntry> targets = GameManager.Instance.xmlManager.LoadScores();
            finalScoreCounter.text = "TOTAL SCORE: " + finalScore;
            if (targets.Count < GameManager.Instance.xmlManager.leaderboard.leaderboardMaximum || targets[^1].score < finalScore)
            {
                PromptName();
            }
        }

        // move from scoring to leaderboard
        public void Continue()
        {
            leaderboardgrid.SetActive(true);
            topLayer.SetActive(false);
        }
        
        // prompt change name method
        public void ChangeName(string name)
        {
            nameToSave = name;
            returnButton.interactable = true;
            GameManager.Instance.xmlManager.AddNewScore(nameToSave, finalScore);
            prompt.SetActive(false);
        }

        // turn prompt on method
        public void PromptName()
        {
            returnButton.interactable = false;
            prompt.SetActive(true);
        }

        public void Restart()
        {
            SelectorMapGenerator smg = gameObject.AddComponent<SelectorMapGenerator>();
            smg.ClearJson();
            // TODO should the game be started on the map screen or on a first mission?
            //SceneManager.LoadScene("");
        }

        public void ReturnToMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}