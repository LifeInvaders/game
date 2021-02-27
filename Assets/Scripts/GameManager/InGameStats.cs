﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class InGameStats : MonoBehaviour
{ 
    //Player stats (KD,score etc etc)
    #region Player-specific

    public int killCount;
    public int deathCount;
    public int score;
    public int object1;
    public int object2;
    public int object3;
    public GameObject target;
    public bool targetKilled;
    public bool isDead;

    #endregion

    //Game wide (could be timers,events and what not)

    #region Game-wide

    #endregion
}