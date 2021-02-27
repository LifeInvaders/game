using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Player;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private List<GameObject> menus = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (PhotonNetwork.IsConnected)
            SwitchMenu(4);
        else SwitchMenu(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            GameObject.Find("Back").GetComponent<Button>().onClick.Invoke();
    }

    public void SwitchMenu(int index)
    {
        for (int i = 0; i < menus.Count; i++) 
            menus[i].SetActive(i==index);
    }

    public void Quit() => Application.Quit();

    public override void OnConnectedToMaster() => SwitchMenu(4);
}
