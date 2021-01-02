using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RandomNPC : MonoBehaviour
{
    private SkinnedMeshRenderer renderer;

    public Mesh[] meshes;

    public Material[] materials;
    // private Mesh meshes[];
    // public Mesh mesh;
    //
    // public Material material;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SkinnedMeshRenderer>();
        renderer.sharedMesh = meshes[Random.Range(0,meshes.Length-1)];
        renderer.sharedMaterial = materials[Random.Range(0, materials.Length - 1)];
    }


}
