using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PrivateRoomHandler : MonoBehaviour
{
    [SerializeField] private int LowerLimitCodeGen;
    [SerializeField] private int UpperLimitCodeGen;
    [SerializeField] private TMP_InputField nameField;
    [SerializeField] private TMP_InputField codeField;
    [SerializeField] private Toggle IsPrivate;
    [SerializeField] private TextMeshProUGUI codeDisplay;
    [SerializeField] private GameObject DisclaimerBlock;
    [SerializeField] private TextMeshProUGUI DisclaimerCodeField;
    [SerializeField] private GameObject HandlerBlock;
    [SerializeField] private TextMeshProUGUI ErrorLog;
    [SerializeField] private GameObject LogBlock;
    
    private int GlobalCode;
    void Start()
    {
        GlobalCode = GenerateCode();
        codeDisplay.gameObject.SetActive(false);
        codeDisplay.text = GlobalCode.ToString();
        DisclaimerBlock.gameObject.SetActive(false);
        ErrorLog.gameObject.SetActive(false);
        LogBlock.SetActive(false);

    }


    public void CloseError()
    {
        LogBlock.SetActive(false);
    }

    void ShowError(string message)
    {
        LogBlock.SetActive(true);
        ErrorLog.gameObject.SetActive(true);
        ErrorLog.text = message;
    }
    
    public void JoinRoom()
    {
        if (codeField.text == "")
        {
            ShowError("Code Field can't be Empty");
            goto END;
            
        }
        if (codeField.text == null)
        {
            ShowError("Code Field can't be null");
            goto END;
        }

        if (codeField.text.Contains(" "))
        {
            ShowError("No Space Allowed In Code");
            goto END;
        }
        try
        {
            HandlerBlock.SetActive(false);
            DisclaimerBlock.SetActive(true);
            //PhotonNetwork.JoinRoom(codeField.text);
            Debug.Log("Room Joined !!");
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }END: ;
        
    }

    int GenerateCode()
    {
        int code = Random.Range(LowerLimitCodeGen, UpperLimitCodeGen);
        return code;
    }

    public void CreateRoom()
    {
        if (nameField.text == "" && !IsPrivate.isOn)
        {
             ShowError("Name Field can't be Empty");
             goto END;
        }
        if (nameField.text == null && !IsPrivate.isOn)
        {
            ShowError("Name Field can't be null");
            goto END;
        }

        if (nameField.text.Contains(" ") && !IsPrivate.isOn)
        {
            ShowError("No Space Allowed In Names");
            goto END;
        }
        
        try
        {
            if (IsPrivate.isOn)
            {
                RoomOptions roomOptions = new RoomOptions();
                roomOptions.IsVisible = false;
                //PhotonNetwork.CreateRoom(GlobalCode.ToString(), roomOptions);
                
                DisclaimerBlock.gameObject.SetActive(true);
                HandlerBlock.SetActive(false);
                
                DisclaimerCodeField.text = GlobalCode.ToString();
                Debug.Log("Room Private Created");
            }
            else
            {
                //PhotonNetwork.CreateRoom(nameField.text);
                Debug.Log("Room Created !!");
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        END: ;
    }
    

    private void Update()
    {
        if (IsPrivate.isOn)
        {
            nameField.gameObject.SetActive(false);
            codeDisplay.gameObject.SetActive(true);
        }
        else
        {
            nameField.gameObject.SetActive(true);
            codeDisplay.gameObject.SetActive(false);
        }
    }
}
