using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerManager : MonoBehaviourPunCallbacks
{

    [SerializeField] private TimerManager timer;
    [SerializeField] private Camera loadCamera;
    [SerializeField] private GameObject loadScreen;
	[SerializeField] private GameObject hud;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private InGameStats igs;
    [NonSerialized] public Photon.Realtime.Player[] players;
    [NonSerialized] public List<Transform> playerTransforms;
    private System.Random _rand;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        players = PhotonNetwork.PlayerList;
        _rand = new System.Random();
        List<Transform> spawnPointsCopy = new List<Transform>(spawnPoints);
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (var player in PhotonNetwork.PlayerList)
            {
                int randIndex = _rand.Next(spawnPointsCopy.Count);
                Transform randCopy = spawnPointsCopy[randIndex];
                int randIndexOrig = spawnPoints.IndexOf(randCopy);
                gameObject.GetPhotonView().RPC(nameof(Spawn), player, randIndexOrig);
                spawnPointsCopy.RemoveAt(randIndex);
            }
        }
        yield return new WaitUntil(CheckAssignViewID);
        UpdatePlayerPrefabs();
    }
    [PunRPC]
    void Spawn(int index)
    {
        Transform spawnPoint = spawnPoints[index];
        loadCamera.gameObject.SetActive(false);
        loadScreen.SetActive(false);
		hud.SetActive(true);
        var player = PhotonNetwork.Instantiate("CustomCharacter", spawnPoint.position, spawnPoint.rotation);
        SetLayerRecursive(player);
        igs.localPlayer = player;
        Hashtable customProps = new Hashtable {{"viewID", player.GetPhotonView().ViewID}, {"dead", false}};
        PhotonNetwork.LocalPlayer.SetCustomProperties(customProps);
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
        igs.localPlayer.transform.position = spawnPoints[spawnPoint].position;
    }
}
