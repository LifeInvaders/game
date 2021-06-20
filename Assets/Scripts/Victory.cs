using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
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

    [Header("Camera")] [SerializeField] private CinemachineVirtualCamera _camera2;
    private int[] anims = new int[3];

    public void StartAnim(int index)
    {
        Debug.Log(index);
        winners[index].GetComponent<Animator>().SetTrigger((anims[index] + 1).ToString());
    }

    public void InitWinner(int index, SkinnedMeshRenderer skinnedMeshRenderer, string namePlayer, int animId)
    {
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

    public void InitLoser(int index, SkinnedMeshRenderer skinnedMeshRenderer, string namePlayer, int deathCount)
    {
        var meshRenderer = winners[index].GetComponentInChildren<SkinnedMeshRenderer>();
        meshRenderer.sharedMesh = skinnedMeshRenderer.sharedMesh;
        meshRenderer.sharedMaterial = skinnedMeshRenderer.sharedMaterial;
        textlosers[index].text = namePlayer;

        countDeath[index].text = $"{deathCount}\nDeath{(deathCount > 1 ? 's' : ' ')}";
    }

    public void Start()
    {
        // var nightMode = FindObjectOfType<NightMode>();
        // var isDay = nightMode.IsDay();
        // foreach (var light in GetComponentsInChildren<Light>())
        //     light.enabled = !isDay;

        string[] name = new[] {"toto", "paul", "julien", "harrys", "dov"};
        for (int i = 0; i < 3; i++)
        {
            var random = new System.Random();
            InitWinner(i, winners[i].GetComponentInChildren<SkinnedMeshRenderer>(), name[random.Next(name.Length)], i);
            InitLoser(i, GetComponentInChildren<SkinnedMeshRenderer>(), name[random.Next(name.Length)], i + 1);
        }
    }
}