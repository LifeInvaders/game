using System;
using UnityEngine;
using System.IO;
using UnityEditor;

// ReSharper disable All

namespace Scoreboard
{
    public class Scoreboard : MonoBehaviour
    {
        [SerializeField] private int maxScoreboardEntries = 5;
        [SerializeField] private Transform highScoreHolderTransform;
        [SerializeField] private GameObject scoreboardEntryObject;
        [SerializeField] private GameObject instanceRoot;

        [Header("Test")] [SerializeField] ScoreboardEntryData testEntrydata;

        private string Savepath => $"{Application.persistentDataPath}/highers.json";

        private void Start()
        {
            ScoreboardSaveData savedScores = GetSavedScores();
            UpdateUI(savedScores);
            SaveScores(savedScores);
        }


        public void AddEntry(ScoreboardEntryData scoreboardEntryData)
        {
            ScoreboardSaveData savedScores = GetSavedScores();

            bool scoreAdded = false;
            for (int i = 0; i < savedScores.highscores.Count; i++)
            {
                if (scoreboardEntryData.entryScore > savedScores.highscores[i].entryScore)
                {
                    savedScores.highscores.Insert(i, scoreboardEntryData);
                    scoreAdded = true;
                    break;
                }
            }

            if (!scoreAdded && savedScores.highscores.Count < maxScoreboardEntries)
            {
                savedScores.highscores.Add(scoreboardEntryData);
            }


            if (savedScores.highscores.Count > maxScoreboardEntries)
            {
                savedScores.highscores.RemoveRange(maxScoreboardEntries,
                    savedScores.highscores.Count - maxScoreboardEntries);
            }

            SaveScores(savedScores);
            // UpdateUI(savedScores);
        }


        // [ContextMenu("update ui")] 
        private void UpdateUI(ScoreboardSaveData savedScores)
        {
            //DeleteChild();
            foreach (Transform child in highScoreHolderTransform)
            {
                // if (PrefabUtility.IsPartOfPrefabInstance(transform))
                // {
                //     PrefabUtility.UnpackPrefabInstance(instanceRoot, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
                Destroy(child);
                // }
            }

            foreach (var highscore in savedScores.highscores)
            {
                Instantiate(scoreboardEntryObject, highScoreHolderTransform).GetComponent<ScoreboardEntryUI>()
                    .Init(highscore);
            }
        }
        //
        // [ContextMenu("Add Test Entry !")]
        // public void AddTestEntry()
        // {
        //     DeleteChild();
        //     AddEntry(testEntrydata);
        // }


        private ScoreboardSaveData GetSavedScores()
        {
            if (!File.Exists(Savepath))
            {
                File.Create(Savepath).Dispose();
                return new ScoreboardSaveData();
            }

            using (StreamReader stream = new StreamReader(Savepath))
            {
                string json = stream.ReadToEnd();
                ScoreboardSaveData s = JsonUtility.FromJson<ScoreboardSaveData>(json);
                return s;
            }
        }

        [ContextMenu("Save Scores")]
        private void SaveScores(ScoreboardSaveData scoreboardSaveData)
        {
            using (StreamWriter stream = new StreamWriter(Savepath))
            {
                string json = JsonUtility.ToJson(scoreboardSaveData, true);
                stream.Write(json);
            }
        }

        [ContextMenu("Delete Child")]
        private void DeleteChild()
        {
            foreach (Transform obj in highScoreHolderTransform)
            {
                Destroy(obj.gameObject);
                int debug = highScoreHolderTransform.childCount;
            }
        }


        /*
        [ContextMenu("ClearData")]
        private void ClearData()
        {
            ScoreboardEntryData s = new ScoreboardEntryData("Cleared", 0, 0, 0);
            ScoreboardSaveData s2 = new ScoreboardSaveData();
            s2.highscores.Add(s);
            string json = JsonUtility.ToJson(s, true);
            StreamWriter streamWriter = new StreamWriter(Savepath);
            streamWriter.Write(json);
        }*/
    }
}