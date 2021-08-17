using System.Net.Mime;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace GameManager
{
    public class ExpBar : MonoBehaviour
    {

        private Text _level;
        private Slider _xpBar;

        void Start()
        {
            _level = GetComponentInChildren<Text>();
            _xpBar = GetComponentInChildren<Slider>();
        }

        void Update()
        {
            _xpBar.maxValue = PlayerDatabase.Instance.Level * PlayerDatabase.Instance.LevelExpReqMult;
            _level.text = "Level " + PlayerDatabase.Instance.Level;
            _xpBar.value = PlayerDatabase.Instance.Exp;
        }
    }
}