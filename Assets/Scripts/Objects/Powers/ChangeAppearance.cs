using System;
using UnityEngine;

namespace Objects.Powers
{
    public class ChangeAppearance : PowerTools
    {
        private GameObject _player;
        public ChangeAppearance()
        {
            _time = 60;
            TimeBeforeUse = 0;
        }

        protected override void Action()
        {
            throw new NotImplementedException();
        }
        
    }
}
