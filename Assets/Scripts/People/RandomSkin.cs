using UnityEngine;

namespace People
{
    public class RandomSkin : MonoBehaviour
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
            SetSkinNPC(_meshNb, _materialNb);
        }

        /// <summary>
        /// Return mesh and material number
        /// </summary>
        /// <returns></returns>
        public (int, int) GetSkinNpc()
        {
            return (_meshNb, _materialNb);
        }

        public void SetSkinNPC(SkinnedMeshRenderer skinnedMeshRenderer)
        {
            _renderer.sharedMesh = skinnedMeshRenderer.sharedMesh;
            _renderer.sharedMaterial = skinnedMeshRenderer.sharedMaterial;
        }

        public void SetSkinNPC(int meshID, int matID)
        {
            _renderer.sharedMesh = meshes[meshID];
            _renderer.sharedMaterial = materials[matID];
        }
    }
}