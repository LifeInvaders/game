using System;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace MainMenu
{
    public class CreatePrivateRoom : MonoBehaviour
    {
        [SerializeField] private int LowerLimitCodeGen;
        [SerializeField] private int UpperLimitCodeGen;
        [SerializeField] private TextMeshProUGUI codeDisplay;
        [SerializeField] private GameObject DisclaimerBlock;
        [SerializeField] private TMP_InputField nameField;

        [Header("Logs")] [SerializeField] private TextMeshProUGUI errorLog;
        [SerializeField] private GameObject logBlock;


        private int _globalCode;
        private bool _isPrivate;

        void Start()
        {
            _globalCode = GenerateCode();
            codeDisplay.transform.parent.gameObject.SetActive(_isPrivate);
            codeDisplay.text = _globalCode.ToString();
            DisclaimerBlock.SetActive(false);
            logBlock.SetActive(false);
        }


        void ShowError(string message)
        {
            logBlock.SetActive(true);
            errorLog.text = message;
        }


        private int GenerateCode() => Random.Range(LowerLimitCodeGen, UpperLimitCodeGen);

        public void CreateRoom()
        {
            logBlock.SetActive(false);
            if (!_isPrivate)
            {
                if (nameField.text == "")
                {
                    ShowError("Name Field can't be Empty");
                    return;
                }

                if (nameField.text.Contains(" "))
                {
                    ShowError("No Space Allowed In Names");
                    return;
                }
            }


            try
            {
                if (_isPrivate)
                {
                    RoomOptions roomOptions = new RoomOptions();
                    roomOptions.MaxPlayers = 4;
                    roomOptions.IsVisible = false;
                    PhotonNetwork.CreateRoom(_globalCode.ToString(), roomOptions);
                    
                    //DisclaimerCodeField.text = _globalCode.ToString();
                    Debug.Log("Room Private Created");
                }
                else
                {
                    PhotonNetwork.CreateRoom(nameField.text);
                    Debug.Log("Room Created !!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void Private()
        {
            _isPrivate = !_isPrivate;
            nameField.gameObject.SetActive(!_isPrivate);
            codeDisplay.transform.parent.gameObject.SetActive(_isPrivate);
            DisclaimerBlock.SetActive(_isPrivate);
        }
    }
}