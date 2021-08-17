using System;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class LevelUnlocks : MonoBehaviour
    {
        [SerializeField] private Button[] winAnimButtons;

        int WinAnimLevelReq(int winIndex) => (int)(Math.Exp(winIndex / 5f) * 1.7f);


        void Start()
        {
            
            for (int i = 0; i < winAnimButtons.Length; i++)
                winAnimButtons[i].interactable = WinAnimLevelReq(i) <= PlayerDatabase.Instance.Level;
        }
    }
}