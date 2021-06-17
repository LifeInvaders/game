using System.Collections;
using System.Collections.Generic;
using Scoreboard;
using TMPro;
using UnityEngine;

namespace Scoreboard
{
    public class ScoreboardEntryUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI entryNameText = null;
        [SerializeField] private TextMeshProUGUI entryScoreText = null;
        [SerializeField] private TextMeshProUGUI entryDeadText = null;
        [SerializeField] private TextMeshProUGUI entryRatioText = null;
        
        public void Init(ScoreboardEntryData scoreboardEntryData)
        {
            entryNameText.text = scoreboardEntryData.entryName;
            entryScoreText.text = scoreboardEntryData.entryScore.ToString();
            entryDeadText.text = scoreboardEntryData.entryDead.ToString();
            entryRatioText.text = scoreboardEntryData.entryRatio.ToString();
        }
    }   
}

