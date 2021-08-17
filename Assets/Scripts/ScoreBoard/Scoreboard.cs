using UnityEngine;
using System.IO;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;


namespace Scoreboard
{
    public class Scoreboard : MonoBehaviourPunCallbacks
    {
        [SerializeField] private int maxScoreboardEntries = 8;
        [SerializeField] private Transform highScoreHolderTransform;
        [SerializeField] private GameObject scoreboardEntryObject;

        [Header("Test")] [SerializeField] ScoreboardEntryData testEntrydata;

        private string Savepath => $"{Application.persistentDataPath}/highers.json";

        private void Start()
        {
            ClearData();
            ScoreboardSaveData savedScores = GetSavedScores();
            foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
                AddEntry(new ScoreboardEntryData(player.NickName,0,0,0));
            UpdateUI(savedScores);
            SaveScores(savedScores);
            SortList();
        }

        public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
        {
            RemoveEntry(otherPlayer.NickName);
        }
        
        public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, Hashtable changedProps)
        {
            if (!changedProps.ContainsKey(PunPlayerScores.PlayerScoreProp)
                && !changedProps.ContainsKey("deathCount")) return;
            var nickName = targetPlayer.NickName;
            var score = targetPlayer.GetScore();
            var deaths = (int)targetPlayer.CustomProperties["deathCount"];
            var kills = (int)targetPlayer.CustomProperties["killCount"];
            AddEntry(new ScoreboardEntryData(nickName,score,kills,deaths));
        }


        public void AddEntry(ScoreboardEntryData EntryToAdd)
        {
            ScoreboardSaveData savedScores = GetSavedScores();
            ScoreboardEntryData FoundEntry = null;
            bool exists = false;
            bool scoreAdded = false;
            foreach (var entry in savedScores.highscores)
            {
                if (entry.entryName == EntryToAdd.entryName)
                {
                    entry.entryScore = EntryToAdd.entryScore;
                    entry.entryDead = EntryToAdd.entryDead;
                    entry.entryRatio = EntryToAdd.entryRatio;
                    FoundEntry = entry;
                    exists = true;
                    break;
                }
            }
            if (!exists)
            {
                for (int i = 0; i < savedScores.highscores.Count; i++)
                {
                    if (EntryToAdd.entryScore > savedScores.highscores[i].entryScore)
                    {
                        savedScores.highscores.Insert(i, EntryToAdd);
                        scoreAdded = true;
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < savedScores.highscores.Count; i++)
                {
                    if (FoundEntry.entryScore > savedScores.highscores[i].entryScore && FoundEntry.entryName != savedScores.highscores[i].entryName)
                    {
                        ScoreboardEntryData temp = FoundEntry;
                        savedScores.highscores.Insert(i, temp);
                        savedScores.highscores.Remove(FoundEntry);
                        
                       
                        break;
                    }
                }
            }
            if (!scoreAdded && !exists && savedScores.highscores.Count < maxScoreboardEntries)
            {
                savedScores.highscores.Add(EntryToAdd);
            }
            if (savedScores.highscores.Count > maxScoreboardEntries)
            {
                savedScores.highscores.RemoveRange(maxScoreboardEntries, savedScores.highscores.Count - maxScoreboardEntries);
            }
            SortList();
            SaveScores(savedScores);
            UpdateUI(savedScores);
        }


        public void RemoveEntry(string name)
        {
            ScoreboardSaveData saveData = GetSavedScores();
            foreach (var VARIABLE in saveData.highscores)
            {
                if (VARIABLE.entryName == name)
                {
                    saveData.highscores.Remove(VARIABLE);
                    break;
                }
            }
        }
        
        public void RemoveEntry(ScoreboardEntryData EntryToRemove)
        {
            ScoreboardSaveData saveData = GetSavedScores();
            foreach (var VARIABLE in saveData.highscores)
            {
                if (VARIABLE.entryName == EntryToRemove.entryName)
                {
                    saveData.highscores.Remove(VARIABLE);
                    break;
                }
            }
        }
        
        

        public static int Compare(ScoreboardEntryData data1, ScoreboardEntryData data2)
        {
            int res = data1.entryScore.CompareTo(data2.entryScore);
            return res;
        }
        
        void SortList()
        {
            ScoreboardSaveData savedScores = GetSavedScores();
            savedScores.highscores.Sort(Compare);
            savedScores.highscores.Reverse();

        }
        
        [ContextMenu("update ui")] 
        private void UpdateUI(ScoreboardSaveData savedScores)
        {
            foreach (Transform child in highScoreHolderTransform)
            {
               Destroy(child.gameObject);
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

        private ScoreboardSaveData GetSavedScores()
        {
            if (!File.Exists(Savepath))
            {
                File.Create(Savepath);
                return new ScoreboardSaveData();
            }
            using (StreamReader stream = new StreamReader(Savepath))
            {
                string json = stream.ReadToEnd();
                ScoreboardSaveData s = JsonUtility.FromJson<ScoreboardSaveData>(json);
                return s;
            }
        }
        
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

        
        [ContextMenu("ClearData")]
        private void ClearData()
        {
            File.WriteAllText(Savepath,"{\"highscores\": []}");
        }
    }
}