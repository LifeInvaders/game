using Objects.Powers;
using TMPro;
using UnityEngine;

namespace HUD
{
    public class PowerHud : MonoBehaviour
    {
        [SerializeField] private GameObject[] icons;
        [SerializeField] private GameObject timer;
        [SerializeField] private TextMeshPro textMeshPro;
        private float _time = 0;

        public void SetIcon(PowerTools powerTools)
        {
            var powerType = GetPowerList(powerTools);
            for (var i = 0; i < icons.Length; i++) icons[i].SetActive(powerType == i);
        }

        public void SetTime(float value)
        {
            _time = value;
            timer.SetActive(true);
        }

        public void SetTime(int value)
        {
            _time = value;
            timer.SetActive(true);
        }

        public void HideTimer() => timer.SetActive(false);

        private void Update()
        {
            if (!(_time >= 0)) return;
            textMeshPro.text = Mathf.RoundToInt(_time).ToString();
            _time -= Time.deltaTime;
        }

        private static int GetPowerList(PowerTools powerTools)
        {
            switch (powerTools)
            {
                case SmokeBomb _:
                    return 0;
                case ChangeAppearance _:
                    return 1;
                case Poison _:
                    return 2;
                case SpeedBoost _:
                    return 3;
                case KnifeThrowing _:
                    return 4;
                default:
                    return -1;
            }
        }
    }
}