using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Photon
{
    public class InstantiateInLobby : MonoBehaviourPun
    {

        // Start is called before the first frame update
        void Start()
        {
            PhotonNetwork.Instantiate("CustomCharacter", new Vector3(-116,1,-3),Quaternion.identity,0);
            
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
