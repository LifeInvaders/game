using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class InGameStats : MonoBehaviour
{ 
    //Player stats (KD,score etc etc)
    #region Player-specific
    
    public int killCount = 0;
    public int deathCount = 0;
    public int score = 0;
    public int object1 = 0;
    public int object2 = 0;
    public int object3 = 0;
    public GameObject target;
    public bool targetKilled = false;
    public bool isDead = false;
    
    #endregion
    //Game wide (could be timers,events and what not)
    #region Game-wide
    #endregion
}
