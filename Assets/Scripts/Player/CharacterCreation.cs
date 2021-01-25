using UnityEngine;

namespace Player
{
    public class CharacterCreation : MonoBehaviour
    {

        private void Awake()
        {
            ResetValues();
        }
        
        private string _charID()
        {
            char[] charArray = {_tempRank, _tempGender, _tempVariant, _tempSkinColor};
            return new string(charArray);
        }
        
        public void ResetValues()
        {
            _tempRank = instance.Rank;
            _tempGender = instance.Gender;
            _tempVariant = instance.Variant;
            _tempSkinColor = instance.SkinColor;
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
            PlayerDatabase.Instance.Rank = _tempRank;
            instance.Gender = _tempGender;
            instance.Variant = _tempVariant;
            instance.SkinColor = _tempSkinColor;
        }
        
        private PlayerDatabase instance = PlayerDatabase.Instance;
        private char _tempRank;
        private char _tempGender;
        private char _tempVariant;
        private char _tempSkinColor;
    }   
}
