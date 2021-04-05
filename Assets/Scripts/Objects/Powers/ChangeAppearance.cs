using System;
using System.Collections;
using People;
using UnityEngine;
using Random = System.Random;

namespace Objects.Powers
{
    public class ChangeAppearance : PowerTools
    {
        private RandomSkin _randomSkin;
        private int _meshID;
        private int _materialID;

        private int _resetTime;

        public void Start()
        {
            _time = 60;
            TimeBeforeUse = 3;
            _resetTime = 15;
            _random = new Random();
            _randomSkin = GetComponentInChildren<RandomSkin>();
        }

        protected override bool IsValid() => true;

        protected override void Action()
        {
            
            (_meshID, _materialID) = _randomSkin.GetSkinNpc();
            Collider[] colliders = Physics.OverlapSphere(transform.position, 3, 1 << 9);

            if (colliders.Length > 0)
            {
                int chosen = _random.Next(colliders.Length);
                var skinnedMesh = colliders[chosen].gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
                _randomSkin.SetSkinNPC(skinnedMesh);
            }
            else
                _randomSkin.SetSkinNPC(_random.Next(15), _random.Next(13));

            StartCoroutine(ResetSkin());

        }

        IEnumerator ResetSkin()
        {
            yield return new WaitForSeconds(_resetTime);
            _randomSkin.SetSkinNPC(_meshID, _materialID);
        } 
    }
}