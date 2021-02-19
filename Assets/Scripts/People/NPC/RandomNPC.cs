using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNPC : MonoBehaviour
{
    private SkinnedMeshRenderer _renderer;

    public Mesh[] meshes;

    public Material[] materials;

    private int _meshNb;
    private int _materialNb;

    void Start()
    {
        _meshNb = Random.Range(0, meshes.Length - 1);
        _materialNb = Random.Range(0, materials.Length - 1);
        
        _renderer = GetComponent<SkinnedMeshRenderer>();
        _renderer.sharedMesh = meshes[_meshNb];
        _renderer.sharedMaterial = materials[_materialNb];
    }
    /// <summary>
    /// Return mesh and material number
    /// </summary>
    /// <returns></returns>
    public (int, int) GetSkinNpc()
    {
        return (_meshNb,_materialNb);
    }


}
