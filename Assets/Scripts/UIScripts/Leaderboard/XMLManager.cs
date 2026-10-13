using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace UIScripts.Leaderboard
{
    public class XMLManager : MonoBehaviour
    {
    
        public Leaderboard leaderboard;

        private void Awake()
        {
            if (GameManager.Instance.xmlManager == null) GameManager.Instance.xmlManager = this;
            // create highscore file if it doesn't exist
            if (!Directory.Exists(Application.persistentDataPath + "/HighScores/"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/HighScores/");
            }
        }
    
        public void AddNewScore(string entryName, int entryScore)
        {
            List<HighScoreEntry> temp = LoadScores(); 
            temp.Add(new HighScoreEntry { name = entryName, score = entryScore });
            temp.Sort((HighScoreEntry x, HighScoreEntry y) => y.score.CompareTo(x.score));
            if (temp.Count > leaderboard.leaderboardMaximum)
            {
                temp.RemoveAt(leaderboard.leaderboardMaximum);
            }
        
            GameManager.Instance.xmlManager.SaveScores(temp);
        }
    
        // get a list of scores to save to the highscore file
        public void SaveScores(List<HighScoreEntry> scoresToSave)
        {
            leaderboard.list = scoresToSave;
            XmlSerializer serializer = new XmlSerializer(typeof(Leaderboard));
            FileStream stream = new FileStream(Application.persistentDataPath + "/HighScores/highscores.xml", FileMode.Create);
            serializer.Serialize(stream, leaderboard);
            stream.Close();
        }
    
        // load the current scores in the high score file
        public List<HighScoreEntry> LoadScores()
        {
            if (File.Exists(Application.persistentDataPath + "/HighScores/highscores.xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Leaderboard));
                FileStream stream = new FileStream(Application.persistentDataPath + "/HighScores/highscores.xml", FileMode.Open);
                leaderboard = serializer.Deserialize(stream) as Leaderboard;
                stream.Close();
            }

            return leaderboard.list;
        }

        [System.Serializable]
        public class Leaderboard
        {
            public List<HighScoreEntry> list = new List<HighScoreEntry>();
            public int leaderboardMaximum = 10;
        }
    }
}