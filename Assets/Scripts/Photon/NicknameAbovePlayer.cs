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

    private Button button;
    [SerializeField] private TextMeshPro floatingText;
    [SerializeField] private Transform textMeshTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        if (!gameObject.GetPhotonView().IsMine)
            floatingText.SetText(gameObject.GetPhotonView().Owner.NickName);
        else
        {
            floatingText.gameObject.SetActive(false);
            enabled = false;
        }
    }

    void Update() => textMeshTransform.rotation = Quaternion.LookRotation( Camera.main.transform.position - textMeshTransform.position );
}
