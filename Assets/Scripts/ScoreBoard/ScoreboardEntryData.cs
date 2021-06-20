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
            entryName = this.entryName;
            entryScore = this.entryScore;
            entryDead = this.entryDead;
            entryRatio = this.entryRatio;
        }
        
    }
    
}
