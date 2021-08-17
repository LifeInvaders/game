using System.Collections;
using Photon.Pun;
using UnityEngine;

namespace People
{
    public class PhotonSkin : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer _renderer;

        public Mesh[] meshes;

        public Material[] materials;

        private int _meshNb;
        private int _materialNb;
        [SerializeField] private Material dissolveMat;

        void Start()
        {
            if (!gameObject.GetPhotonView().IsMine) return;
            var meshID = Random.Range(0, meshes.Length - 1);
            var matID = Random.Range(0, materials.Length - 1);
            gameObject.GetPhotonView().RPC(nameof(SetSkinNpc),RpcTarget.All,meshID,matID);
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
        public (int mesh, int material) GetSkinNpc()
        {
            return (_meshNb, _materialNb);
        }
        
        [PunRPC]
        public void SetSkinNpc(int meshID, int matID)
        {
            _materialNb = matID;
            _meshNb = meshID;
            StartCoroutine(FadeIn());
        }
    }
}