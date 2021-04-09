using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField] private TimerManager timer;
    [SerializeField] private Camera loadCamera;
    [SerializeField] private GameObject loadScreen;
    [SerializeField] private List<Transform> spawnPoints;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        if (!PhotonNetwork.IsMasterClient) yield return 0;
        List<Transform> spawnPointsCopy = new List<Transform>(spawnPoints);
        System.Random rand = new System.Random();
        Transform masterClientSpawn = null;
        foreach (var player in PhotonNetwork.PlayerList)
        {
            int randIndex = rand.Next(spawnPointsCopy.Count);
            if (player.IsLocal)
                masterClientSpawn = spawnPointsCopy[randIndex];
            else gameObject.GetPhotonView().RPC(nameof(Spawn), player, spawnPointsCopy[randIndex]);
            spawnPointsCopy.RemoveAt(randIndex);
        }
        yield return new WaitForSeconds(5);
        Spawn(masterClientSpawn);
    }

    [PunRPC]
    void Spawn(Transform spawnPoint)
    {
        loadCamera.gameObject.SetActive(false);
        loadScreen.SetActive(false);
        PhotonNetwork.Instantiate("CustomPlayer", spawnPoint.position, spawnPoint.rotation);
        timer.enabled = true;
    }
}
