using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UI;

public class NicknameAbovePlayer : MonoBehaviour
{
    [SerializeField] private Transform floatingText;
    
    // Start is called before the first frame update
    void Start()
    {
        if (!gameObject.GetPhotonView().IsMine)
            floatingText.gameObject.GetComponent<TextMeshPro>().SetText(gameObject.GetPhotonView().Owner.NickName);
        else
        {
            floatingText.gameObject.SetActive(false);
            enabled = false;
        }
    }

    void Update() => floatingText.rotation = Quaternion.LookRotation(  floatingText.position - Camera.main.transform.position);
}
