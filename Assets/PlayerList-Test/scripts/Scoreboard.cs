using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Security.Cryptography;
using Scoreboard;
using UnityEditor;

namespace Scoreboard
{
    public class Scoreboard : MonoBehaviour
    {
        [SerializeField] private int maxScoreboardEntries = 5;
        [SerializeField] private Transform highScoreHolderTransform = null;
        [SerializeField] private GameObject scoreboardEntryObject = null;
        [SerializeField] private GameObject instanceRoot = null;
        
        [Header("Test")] [SerializeField] ScoreboardEntryData testEntrydata = new ScoreboardEntryData();

        private string Savepath => $"{Application.persistentDataPath}/highers.json";

        private void Start()
        {
            ScoreboardSaveData savedScores = GetSavedScores();
            UpdateUI(savedScores);
            SaveScores(savedScores);
        }

        private void UpdateUI(ScoreboardSaveData savedScores)
        {
            
            foreach (Transform child in highScoreHolderTransform)
            {
                if (UnityEditor.PrefabUtility.IsPartOfPrefabInstance(transform))
                {
                    UnityEditor.PrefabUtility.UnpackPrefabInstance(instanceRoot, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
                    DestroyImmediate(child);
                }
                    
            }

            foreach (var highscore in savedScores.highscores)
            {
                Instantiate(scoreboardEntryObject, highScoreHolderTransform).GetComponent<ScoreboardEntryUI>().Init(highscore);
            }
        }
        
        [ContextMenu("Add Test Entry !")]
        public void AddTestEntry()
        {
            DeleteChild();
            AddEntry(testEntrydata);
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
                savedScores.highscores.RemoveRange(maxScoreboardEntries, savedScores.highscores.Count - maxScoreboardEntries);
            }
            SaveScores(savedScores);
            UpdateUI(savedScores);
            
        }
        
        [ContextMenu("Get Save Cores")]
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
                return JsonUtility.FromJson<ScoreboardSaveData>(json);
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
            foreach (Transform child in highScoreHolderTransform)
            {
                Destroy(child.gameObject);
            } 
        }
        
    }
}

