using System;

namespace Scoreboard
{ 
    [Serializable] 
    public class ScoreboardEntryData
    {
        public string entryName;
        public int entryScore;
        public int entryDead;
        public int entryRatio;

        public ScoreboardEntryData(string entryName, int entryScore,int entryDead, int entryRatio  )
        {
            this.entryName = entryName;
            this.entryScore = entryScore;
            this.entryDead = entryDead;
            this.entryRatio = entryRatio;
        }
        
    }
    
}
