using System;
using UnityEngine.UI;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

using System.Collections;

namespace Com.lifeInvaders.PanicAtTortuga
{
    
        [RequireComponent(typeof(InputField))]
        public class PlayerInputFields : MonoBehaviour
        {
            #region Private Constants

            private const string PlayerNamePrefKey = "PlayerName";

            #endregion

            #region Monobehaviour Callbacks

            private void Start()
            {
                string defaultName = string.Empty;
                InputField _inputField = GetComponent<InputField>();
                if (_inputField != null)
                {
                    if (PlayerPrefs.HasKey(PlayerNamePrefKey))
                    {
                        defaultName = PlayerPrefs.GetString(PlayerNamePrefKey);
                        _inputField.text = defaultName;
                    }
                }

                PhotonNetwork.NickName = defaultName;
            }
            
            #endregion

            #region Public Methods

            public void SetPlayerName(string value)
            {
                if (string.IsNullOrEmpty(value))
                {
                    Debug.LogError("Name Empty or null");
                    return;
                }

                PhotonNetwork.NickName = value;
                
                PlayerPrefs.SetString(PlayerNamePrefKey,value);

            }

            #endregion
            
            
            
        }
}

