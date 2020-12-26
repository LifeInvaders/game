using System;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using static Player.PlayerDatabase;

namespace Player
{
    public class CharacterCreation : MonoBehaviour
    {

        private void Awake()
        {
            ResetValues();
        }
        
        private static string _charID()
        {
            char[] charArray = {_tempRank, _tempGender, _tempVariant, _tempSkinColor};
            return new string(charArray);
        }
        
        public void ResetValues()
        {
            _tempRank = Rank;
            _tempGender = Gender;
            _tempVariant = Variant;
            _tempSkinColor = SkinColor;
            ChangeSkin();
        }
        
        private void ChangeSkin()
        {
            string searchName = _charID();
            foreach (Transform child in transform)
            {
                if (child.gameObject.name == searchName)
                    child.gameObject.SetActive(true);
                else if (child.gameObject.activeSelf)
                    child.gameObject.SetActive(false);
            }
        }

        public void ChangeDataPassRef(int value)
        {
            switch (value)
            {
                case 0:
                    ChangeDataRef(ref _tempRank, '0', '2');
                    break;
                case 2:
                    ChangeDataRef(ref _tempVariant, '1', '4');
                    break;
                case 3:
                    ChangeDataRef(ref _tempSkinColor, 'A', 'C');
                    break;
                default:
                    _tempGender = _tempGender == 'M' ? 'F' : 'M';
                    ChangeSkin();
                    break;
            }
        }

        public void ChangeDataRef(ref char info, char min, char max)
        {
            info = info + 1 > max ? (char) (min + 1 - (max - info) - 1) : (char) (info + 1);
            ChangeSkin();
        }
        
        public void ApplyChanges()
        {
            Rank = _tempRank;
            Gender = _tempGender;
            Variant = _tempVariant;
            SkinColor = _tempSkinColor;
        }

        private static char _tempRank;
        private static char _tempGender;
        private static char _tempVariant;
        private static char _tempSkinColor;
    }   
}
