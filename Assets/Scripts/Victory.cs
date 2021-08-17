using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using GameManager;
using Photon.Pun;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Victory : MonoBehaviour
{
    [Header("Winners")] [SerializeField] private GameObject[] winners;
    [SerializeField] private TextMeshPro[] textWinners;

    [Header("Losers")] [SerializeField] private GameObject[] losers;
    [SerializeField] private TextMeshPro[] textlosers;

    [SerializeField] private TextMeshPro[] countDeath;
    // Start is called before the first frame update

    public ScoreManager scoreManager;

    [Header("Camera")] [SerializeField] private CinemachineVirtualCamera _camera2;
    private int[] anims = new int[3];

    public void StartAnim(int index)
    {
        Debug.Log(index);
        winners[index].GetComponent<Animator>().SetTrigger((anims[index] + 1).ToString());
    }

    public void InitWinner(Photon.Realtime.Player player, int index)
    {
        var namePlayer = player.NickName;
        var skinnedMeshRenderer = PhotonNetwork.GetPhotonView((int) player.CustomProperties["viewID"]).GetComponentInChildren<SkinnedMeshRenderer>();
        var animId = (int) player.CustomProperties["winAnim"];
        var meshRenderer = winners[index].GetComponentInChildren<SkinnedMeshRenderer>();
        meshRenderer.sharedMesh = skinnedMeshRenderer.sharedMesh;
        meshRenderer.sharedMaterial = skinnedMeshRenderer.sharedMaterial;

        switch (index)
        {
            case 0:
                textWinners[0].text = $"1st\n{namePlayer}";
                break;
            case 1:
                textWinners[1].text = $"2nd\n{namePlayer}";
                break;
            case 2:
                textWinners[2].text = $"3rd\n{namePlayer}";
                break;
        }


        anims[index] = animId;
    }

    public void ChangeCamera()
    {
        _camera2.Priority = 400;
    }

    public void InitLoser(Photon.Realtime.Player player, int index)
    {
        var namePlayer = player.NickName;
        var skinnedMeshRenderer = PhotonNetwork.GetPhotonView((int) player.CustomProperties["viewID"]).GetComponentInChildren<SkinnedMeshRenderer>();
        var deathCount = (int) player.CustomProperties["deathCount"];
        var meshRenderer = winners[index].GetComponentInChildren<SkinnedMeshRenderer>();
        meshRenderer.sharedMesh = skinnedMeshRenderer.sharedMesh;
        meshRenderer.sharedMaterial = skinnedMeshRenderer.sharedMaterial;
        textlosers[index].text = namePlayer;
        countDeath[index].text = $"{deathCount}\nDeath{(deathCount > 1 ? 's' : ' ')}";
    }

    IEnumerator Start()
    {
        yield return new WaitUntil(() => scoreManager != null);
        for (int i = 0; i < 3 && i <= PhotonNetwork.CurrentRoom.PlayerCount; i++)
            InitWinner(scoreManager.scoreBoard[i],i);
        scoreManager.DeathSortFunc();
        for (int i = 0; i < 3 && i <= PhotonNetwork.CurrentRoom.PlayerCount; i++)
            InitLoser(scoreManager.scoreBoard[i],i);
    }
}