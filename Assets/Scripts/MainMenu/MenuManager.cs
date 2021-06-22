using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using Player;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

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

    public void Play()
    {
        Debug.Log(PlayerDatabase.Instance.finishedTutorial);
        if (!PlayerDatabase.Instance.finishedTutorial)
            menus[8].SetActive(true);

        else
        {
            SwitchMenu(5);
            GetComponent<ServerManager>().Connect();
        }
    }

    public void FinishedTutorial()
    {
        PlayerDatabase.Instance.finishedTutorial = true;
        GetComponent<SaveDatabase>().Save();
    }

    public void Quit() => Application.Quit();

    public override void OnConnectedToMaster() => SwitchMenu(4);
}
