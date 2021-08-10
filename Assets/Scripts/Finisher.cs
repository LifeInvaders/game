﻿using System;
using System.Collections;
using Cinemachine;
using People.Player;
using TargetSystem;
using UnityEngine;
using UnityEngine.Serialization;

public class Finisher : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private SkinnedMeshRenderer killer;
    [SerializeField] private SkinnedMeshRenderer dead;
    public CinemachineVirtualCamera camera;
    
    public GameObject player;
    public GameObject victim;
    // public GameObject target;
    [SerializeField] private GameObject[] objectsToDisapear;

    [SerializeField] private Material _material;
    public CinemachineBrain cinemachineBrain;

    public bool animFinished;
    public void LeaveCamera()
    {
        StartCoroutine(WaitForDeathAnim());
        foreach (var toolObject in objectsToDisapear)
        {
            Destroy(toolObject,0.2f);
        }
        animFinished = true;
        camera.Priority = 1;
        player.transform.Find("PlayerCharacter").GetComponent<SkinnedMeshRenderer>().enabled = true;
        player.GetComponent<PlayerControler>().SetMoveBool(true);
        player.GetComponent<PlayerControler>().SetRotateBool(true);
        player.GetComponent<CastTarget>().enabled = true;
        player.transform.position = killer.transform.parent.position;
        player.transform.rotation = transform.rotation;
        killer.enabled = false;
    }   
    public void SetHumans(SkinnedMeshRenderer killerSkin, SkinnedMeshRenderer deadSkin)
    {
        // killer = killerSkin;
        // dead = deadSkin;

        // var killerSkinnedMeshRender = killer.GetComponentInChildren<SkinnedMeshRenderer>();
        // var deadSkinnedMeshRender = dead.GetComponentInChildren<SkinnedMeshRenderer>();  
        killer.sharedMesh = killerSkin.sharedMesh;
        killer.sharedMaterial = killerSkin.sharedMaterial;
        
        dead.sharedMesh = deadSkin.sharedMesh;
        dead.sharedMaterial = deadSkin.sharedMaterial;
        
    }

    public void TargetToSkeleton(GameObject skeleton)
    {
        dead.sharedMesh = skeleton.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;
    }
    
    IEnumerator WaitForDeathAnim()
    {
        yield return new WaitForSeconds(5);
        SkinnedMeshRenderer meshRenderer = dead.GetComponentInChildren<SkinnedMeshRenderer>();
        Texture oldTexture = meshRenderer.sharedMaterial.mainTexture;

        var mat = new Material(_material);
        mat.SetTexture("Texture2D_C902C618", oldTexture);
        meshRenderer.sharedMaterial = mat;

        meshRenderer.sharedMaterial.SetFloat("Vector1_203537A2", Time.time);


        float timeElapsed = 0f;
        float phase = 3;
        float targetPhase = 5.5f;
        while (timeElapsed <= 3)
        {
            timeElapsed += Time.deltaTime;
            meshRenderer.sharedMaterial.SetFloat("Vector1_203537A2",
                Mathf.Lerp(phase, targetPhase, timeElapsed / 3));
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }

    public void SetCameraBlendDuration(int duration)
    {
        cinemachineBrain.m_DefaultBlend.m_Time = duration;
    }

    public void SetCameraBlendToCut()
    {
        cinemachineBrain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Cut;
    }

    public void ResetCameraBlendDuration()
    {
        cinemachineBrain.m_DefaultBlend.m_Time = 2;
        cinemachineBrain.m_DefaultBlend.m_Style  = CinemachineBlendDefinition.Style.EaseInOut;
    }

    public void SetDead(SkinnedMeshRenderer m) => throw new NotImplementedException();
}
