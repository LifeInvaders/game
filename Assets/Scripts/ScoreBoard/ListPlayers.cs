using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ScoreBoard
{

    public class ListPlayers : MonoBehaviourPunCallbacks
    {

        // Start is called before the first frame update
        //[SerializeField] public TextMeshProUGUI printPlayer;
        [SerializeField] public Canvas playerBoard;
        [SerializeField] public int Mode;

        private List<Photon.Realtime.Player> _players;

        void Start()
        {
            if (Mode == 0)
            {
                playerBoard.enabled = (false);
            }


        }


        // Update is called once per frame
        void Update()
        {
            if (Mode == 0)
            {
                if (Keyboard.current.tabKey.wasPressedThisFrame)
                {

                    playerBoard.enabled = (true);

                }

                if (Keyboard.current.tabKey.wasReleasedThisFrame)
                {
                    playerBoard.enabled = (false);
                }
            }


        }
    }
}

