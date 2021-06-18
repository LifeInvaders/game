using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

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

        public float soundLevel = 0;
        public bool finishedTutorial = false;

        public int qualitySetting;
        public int fpsSettings = 30;
        #endregion
        
        //Keeping track of player statistics
        #region Stats

        public int FirstPlace = 0;
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
       public int SelectedEmote = 0;

       #endregion
       
       //Progress system 
        #region Progress

        public int LevelExpReqMult = 100;
        public  int Level = 1;
        public int Exp = 0;

        #endregion
    }
}
