using UnityEngine;
using static Player.PlayerDatabase;

namespace Player
{
    public class CharacterCreation : MonoBehaviour
    {
        private void Awake()
        {
            ChangeSkin();
        }

        private static string _charID()
        {
        char[] charArray = {Rank, Gender, Variant, SkinColor};
        return new string(charArray);
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

        public void ChangeData(int value)
        {
            switch (value)
            {
                case 0:
                    Rank = (Rank + 1 > '2') ? '0' : (char) (Rank + 1);
                    break;
                case 1:
                    Gender = (Gender == 'F') ? 'M' : 'F';
                    break;
                case 2:
                    Variant = (Variant + 1 > '4') ? '1' : (char) (Variant + 1);
                    break;
                case 3:
                    SkinColor = (SkinColor + 1 > 'C') ? 'A' : (char) (SkinColor + 1);
                    break;
                default:
                    Debug.Log("wrong value");
                    break;
            }
            ChangeSkin();
        }

    }   
}
