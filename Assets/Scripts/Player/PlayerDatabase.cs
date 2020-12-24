using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class PlayerDatabase : MonoBehaviour
{
    #region Stats

    public static int TargetKills = 0;
    public static int Points = 0;
    public static int Deaths = 0;
    public static int Games = 0;

    #endregion      
    
   #region PlayerCharacter

   public static char Rank = '0';
   public static char Gender = 'M';
   public static char Variant = '1';
   public static char SkinColor = 'A';

   #endregion
    
    #region Progress
    
    public static int Level = 0;
    public static int Exp = 0;

    #endregion
    
    
}
