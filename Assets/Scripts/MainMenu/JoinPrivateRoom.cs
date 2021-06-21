using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace MainMenu
{
    public class JoinPrivateRoom : MonoBehaviour
    {
      [SerializeField] private TMP_InputField codeField;

        [Header("Logs")] [SerializeField] private TextMeshProUGUI errorLog;
        [SerializeField] private GameObject logBlock;


        void Start()
        {
            logBlock.SetActive(false);
        }


        void ShowError(string message)
        {
            logBlock.SetActive(true);
            errorLog.text = message;
        }

        public void JoinRoom()
        {
            logBlock.SetActive(false);
            if (codeField.text == "")
            {
                ShowError("Code Field can't be Empty");
                return;
            }

            if (codeField.text.Contains(" "))
            {
                ShowError("No Space Allowed In Code");
                return;
            }
            try
            {
                //PhotonNetwork.JoinRoom(codeField.text);
                Debug.Log("Room Joined !!");
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }
}