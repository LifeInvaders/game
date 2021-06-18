using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scoreboard
{
    [Serializable]
    public class ScoreboardSaveData
    {
        public List<ScoreboardEntryData> highscores = new List<ScoreboardEntryData>();
    }
}

