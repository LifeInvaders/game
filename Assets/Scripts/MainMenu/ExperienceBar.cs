using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceBar : MonoBehaviour
{
    [SerializeField] private Slider xpBar;
    private PlayerDatabase _instance;
    [SerializeField] private Text xp;
    [SerializeField] private Text level;

    void Start()
    {
        _instance = PlayerDatabase.Instance;
        xpBar.maxValue = _instance.Level * _instance.LevelExpReqMult;
        xpBar.value = _instance.Exp;
        xp.text = _instance.Exp.ToString();
        level.text = "Level " + _instance.Level;
    }

    public void Test()
    {
        _instance.Exp += 10;
        Start();
    }
}
