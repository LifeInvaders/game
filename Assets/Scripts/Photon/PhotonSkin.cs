using Photon.Pun;
using UnityEngine;

namespace People
{
    public class PhotonSkin : MonoBehaviour
    {
        private SkinnedMeshRenderer _renderer;

        public Mesh[] meshes;

        public Material[] materials;

        private int _meshNb;
        private int _materialNb;

        void Start()
        {
            if (!gameObject.GetPhotonView().IsMine) return;
            var meshID = Random.Range(0, meshes.Length - 1);
            var matID = Random.Range(0, materials.Length - 1);
            gameObject.GetPhotonView().RPC(nameof(SetSkinNpc),RpcTarget.All,meshID,matID);
        }

        /// <summary>
        /// Return mesh and material number
        /// </summary>
        /// <returns></returns>
        public (int, int) GetSkinNpc()
        {
            return (_meshNb, _materialNb);
        }
        
        [PunRPC]
        public void SetSkinNpc(int meshID, int matID)
        {
            _renderer = GetComponent<SkinnedMeshRenderer>();
            _meshNb = meshID;
            _renderer.sharedMesh = meshes[meshID];
            _renderer.sharedMaterial = materials[matID];
        }
    }
}