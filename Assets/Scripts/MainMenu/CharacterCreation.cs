using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

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
            _tempRank = PlayerDatabase.Instance.Rank;
            _tempGender = PlayerDatabase.Instance.Gender;
            _tempVariant = PlayerDatabase.Instance.Variant;
            _tempSkinColor = PlayerDatabase.Instance.SkinColor;
            ChangeSkin();
        }

        public void ChangeSkin()
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
            PlayerDatabase.Instance.Gender = _tempGender;
            PlayerDatabase.Instance.Variant = _tempVariant;
            PlayerDatabase.Instance.SkinColor = _tempSkinColor;
            ChangeSkin();
        }

        void Update()
        {
            if ( RotateWithMouse && Input.GetMouseButton(0))
                transform.parent.Rotate(0,(_mousePos-Input.mousePosition.x)/3,0); 
            _mousePos = Input.mousePosition.x;
        }
        
        private char _tempRank;
        private char _tempGender;
        private char _tempVariant;
        private char _tempSkinColor;
        private float _mousePos;

        public bool RotateWithMouse;
    }   
}
