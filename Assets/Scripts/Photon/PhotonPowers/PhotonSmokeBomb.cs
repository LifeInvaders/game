using Photon.Pun;
using UnityEngine;


namespace Objects.Powers
{
    public class PhotonSmokeBomb : PowerTools
    {

        protected override void SetValues()
        {
            _time = 120;
            TimeBeforeUse = 3;
        }

        protected override bool IsValid() => true;

        protected override void Action()
        {
            var bomb = PhotonNetwork.Instantiate("Photon Smoke Bomb", transform.position, Quaternion.Euler(-90, 0, 0));
            bomb.GetComponent<PhotonSmokeBombManager>().SetPlayer(gameObject);
        }

     

        
    }
}