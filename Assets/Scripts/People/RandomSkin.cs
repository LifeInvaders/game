using System.Collections;
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

        [SerializeField] private Material dissolveMat;

        void Start()
        {
            _meshNb = Random.Range(0, meshes.Length - 1);
            _materialNb = Random.Range(0, materials.Length - 1);
            
            _renderer = GetComponent<SkinnedMeshRenderer>();
            StartCoroutine(FadeIn());
        }

        IEnumerator FadeIn()
        {
            _renderer.sharedMesh = meshes[_meshNb];
            var mat = new Material(dissolveMat);
            mat.SetTexture("Texture2D_C902C618", materials[_materialNb].mainTexture);
            _renderer.sharedMaterial = mat;
            float timeElapsed = 0f;
            float phase = 0;
            float targetPhase = 3.5f;
            _renderer.sharedMaterial.SetFloat("Vector1_203537A2", Time.time);
            while (timeElapsed <= 3)
            {
                timeElapsed += Time.deltaTime;
                _renderer.sharedMaterial.SetFloat("Vector1_203537A2",
                    Mathf.Lerp(phase, targetPhase, timeElapsed / 3));
                yield return new WaitForEndOfFrame();
            }
            _renderer.sharedMaterial = materials[_materialNb];
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