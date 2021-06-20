using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using TargetSystem;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Random = System.Random;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    
    [SerializeField] private TimerManager timer;
    [SerializeField] private Camera loadCamera;
    [SerializeField] private GameObject hud;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private InGameStats igs;
    [NonSerialized] public Photon.Realtime.Player[] players;
    [NonSerialized] public List<Transform> playerTransforms;
    private Random _rand;
    [SerializeField] private Volume volume;
    [SerializeField] private GameObject classMenu;
    [SerializeField] private string classChoice = "";
    private bool latePick;
    [SerializeField] public Button[] classChoiceButtons; 


    // Start is called before the first frame update
    IEnumerator Start()
    {
        players = PhotonNetwork.PlayerList;
        _rand = new Random();
        List<Transform> spawnPointsCopy = new List<Transform>(spawnPoints);
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (var player in PhotonNetwork.PlayerList)
            {
                int randIndex = _rand.Next(spawnPointsCopy.Count);
                Transform randCopy = spawnPointsCopy[randIndex];
                int randIndexOrig = spawnPoints.IndexOf(randCopy);
                gameObject.GetPhotonView().RPC(nameof(StartSpawnProcess), player, randIndexOrig);
                spawnPointsCopy.RemoveAt(randIndex);
            }
        }
        yield return new WaitUntil(CheckAssignViewID);
        UpdatePlayerPrefabs();
    }

    [PunRPC]
    void StartSpawnProcess(int index) => StartCoroutine(Spawn(index));
    
    IEnumerator Spawn(int index)
    {
        Transform spawnPoint = spawnPoints[index];
        loadCamera.gameObject.SetActive(false);
        yield return new WaitUntil(() => classChoice.Length != 0);
        classMenu.SetActive(false);
        hud.SetActive(true);
        var player = PhotonNetwork.Instantiate(classChoice, spawnPoint.position, spawnPoint.rotation);
        SetLayerRecursive(player);
        igs.localPlayer = player;
        player.GetComponent<CastTarget>().vignette = volume;
        Hashtable customProps = new Hashtable {{"viewID", player.GetPhotonView().ViewID}, {"dead", false}, {"deathCount", 0},{"killCount",0}};
        PhotonNetwork.LocalPlayer.SetCustomProperties(customProps);
    }
    
    public void SetClass(string classChoice) => this.classChoice = classChoice;

    public void RandomPickClass() => classChoiceButtons[_rand.Next(classChoiceButtons.Length)].onClick.Invoke();

    public IEnumerator LatePick()
    {
        yield return new WaitForSeconds(10);
        if (classChoice.Length == 0) RandomPickClass();
    }

    void SetLayerRecursive(GameObject go)
    {
        go.tag = "MyPlayer";
        go.layer = LayerMask.NameToLayer("MyPlayer");
        foreach (Transform child in go.transform)
        {
            SetLayerRecursive(child.gameObject);
        }
    }

    bool CheckAssignViewID()
    {
        foreach (Photon.Realtime.Player player in players)
        {
            if (!player.CustomProperties.TryGetValue("viewID",out var viewID))
                return false;
        }
        return true;
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Debug.Log(otherPlayer.NickName + " left the game.");
        players = PhotonNetwork.PlayerList;
        UpdatePlayerPrefabs();
    }

    void UpdatePlayerPrefabs()
    {
        playerTransforms = new List<Transform>();
        foreach (Photon.Realtime.Player player in players) 
            playerTransforms.Add(PhotonView.Find((int)player.CustomProperties["viewID"]).transform);
    }

    private void SetAlive()
    {
        Hashtable alive = new Hashtable {{"dead", false}};
        PhotonNetwork.LocalPlayer.SetCustomProperties(alive);
    }

    public void RespawnPlayers()
    {
        List<Transform> spawnPointsCopy = new List<Transform>(spawnPoints);
        foreach(Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            bool dead = (bool)player.CustomProperties["dead"];
            PhotonView photonView = PhotonView.Find((int) player.CustomProperties["viewID"]);
            GameObject playerGo = photonView.gameObject;
            if (dead)
            {
                playerGo.SetActive(true);
                if (PhotonNetwork.IsMasterClient)
                {
                    int randIndex = _rand.Next(spawnPointsCopy.Count);
                    Transform randCopy = spawnPointsCopy[randIndex];
                    int randIndexOrig = spawnPoints.IndexOf(randCopy);
                    gameObject.GetPhotonView().RPC(nameof(Respawn), player, randIndexOrig);
                    spawnPointsCopy.RemoveAt(randIndex);
                }
            }
        }
    }
    
    [PunRPC]
    void Respawn(int spawnPoint)
    {
        SetAlive();
        igs.localPlayer.transform.position = spawnPoints[spawnPoint].position;
        var input = igs.localPlayer.GetComponent<PlayerInput>();
        input.enabled = false;
        input.enabled = true;
    }
}
