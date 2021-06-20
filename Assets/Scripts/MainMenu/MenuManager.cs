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
        Random r = new Random();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (PhotonNetwork.IsConnected)
            SwitchMenu(4);
        else SwitchMenu(0);
        // foreach (Button button in FindObjectsOfType<Button>())
        //     RandomRotate(button.gameObject, r.Next(4));
    }
    
    private void RandomRotate(GameObject button,int rotate)
    {
        RectTransform rectTransform = button.GetComponent<RectTransform>();
        Transform text = button.GetComponentInChildren<Text>().transform;
        Quaternion textRot = text.rotation;
        rectTransform.Rotate(new Vector3(0,0,rotate*90));
        if (rotate % 2 != 0)
        {
            Vector2 sizeDelta = rectTransform.sizeDelta;
            rectTransform.sizeDelta = new Vector2(sizeDelta.y, sizeDelta.x);
        }
        text.rotation = textRot;
    }

    public void RotateTest()
    {
        Random r = new Random();
        foreach (Button button in FindObjectsOfType<Button>())
            RandomRotate(button.gameObject, r.Next(4));
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
