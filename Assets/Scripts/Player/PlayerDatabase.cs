using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Player
{
    [Serializable] //Save system compatible
    
    //Do not modify structure!
    //Do not change default values!
    //You can add additional values
    //Add existing/Create new categories as needed
    public class PlayerDatabase
    {
        //Don't touch! Singleton constructor
        #region Constructor
        private static PlayerDatabase _instance;
        private PlayerDatabase(){}
        public static PlayerDatabase Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new PlayerDatabase();
                return _instance;
            }
            set => _instance = value;
        }
        #endregion
        
        #region Settings

        public Dictionary<Guid, string> controls = new Dictionary<Guid, string>();
        
        #endregion
        
        //Keeping track of player statistics
        #region Stats

        public int TargetKills = 0;
        public int Points = 0;
        public int Deaths = 0;
        public int Games = 0;

        #endregion    
        
        //Character customization settings
       #region PlayerCharacter

       public string Nickname = "Player";
       public char Rank = '0';
       public char Gender = 'M';
       public char Variant = '1';
       public char SkinColor = 'A';

       #endregion
       
       //Progress system 
        #region Progress
        
        public  int Level = 0;
        public int Exp = 0;

        #endregion
    }
}
