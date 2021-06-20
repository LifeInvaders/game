using System;
using System.Collections;
using People;
using Photon.Pun;
using UnityEngine;
using Random = System.Random;

namespace Objects.Powers
{
    public class PhotonChangeAppearance : PowerTools
    {
        private PhotonSkin _randomSkin;
        private int _meshID;
        private int _materialID;

        private int _resetTime;

        public void Start()
        {
            _time = 60;
            TimeBeforeUse = 3;
            _resetTime = 15;
            _random = new Random();
            _randomSkin = GetComponentInChildren<PhotonSkin>();
        }

        protected override bool IsValid() => true;

        protected override void Action()
        {
            
            (_meshID, _materialID) = _randomSkin.GetSkinNpc();
            Collider[] colliders = Physics.OverlapSphere(transform.position, 4, 1 << 9);

            if (colliders.Length > 0)
            {
                (int mesh, int material) = colliders[0].gameObject.GetComponentInChildren<PhotonSkin>().GetSkinNpc();
                _randomSkin.gameObject.GetPhotonView()
                    .RPC(nameof(_randomSkin.SetSkinNpc), RpcTarget.All, mesh, material);
            }
            else
                _randomSkin.gameObject.GetPhotonView().RPC(nameof(_randomSkin.SetSkinNpc),RpcTarget.All,_random.Next(15), _random.Next(13));
            StartCoroutine(ResetSkin());

        }

        IEnumerator ResetSkin()
        {
            yield return new WaitForSeconds(_resetTime);
            _randomSkin.gameObject.GetPhotonView().RPC(nameof(_randomSkin.SetSkinNpc),RpcTarget.All,_meshID, _materialID);
        } 
    }
}