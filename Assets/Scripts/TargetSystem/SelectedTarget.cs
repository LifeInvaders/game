using System;
using UnityEngine;
using Photon.Pun;

namespace TargetSystem
{
    public class SelectedTarget : MonoBehaviour
    {
        private GameObject _selectedTarget;
        private Outline _outlineTarget;
        private bool _isselectedtarget = false;
        private RaycastHit _raycastHit;

        [SerializeField] private Camera camera;

        /// <summary>
        /// Retire la surbrillance de l'objet sélectionné
        /// </summary>
        private void ResetOutline()
        {
            if (_selectedTarget != null)
            {
                _outlineTarget.OutlineColor = Color.white;
                _outlineTarget.enabled = false;
                _outlineTarget = null;
            }
        }
        /// <summary>
        /// Retourne true si il a verrouilé une cible
        /// </summary>
        /// <returns></returns>
        public bool IsTarget()
        {
            return _isselectedtarget;
        }
        /// <summary>
        /// Retourne le Gameobject de la cible verouillée
        /// </summary>
        /// <returns></returns>
        public GameObject GetTarget()
        {
            return _selectedTarget;
        }
        /// <summary>
        /// Retourne true si traget est la cible verouillée
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool IsSelectedTarget(GameObject target)
        {
            return _selectedTarget != null && target.name == _selectedTarget.name;
        }
        /// <summary>
        /// Met à jour la surbrillance du joueur sélectionné.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="outline"></param>
        public void UpdateSelectedTarget(GameObject target, Outline outline)
        {
            ResetOutline();
            if (IsSelectedTarget(target))
            {
                _selectedTarget = null;
                _isselectedtarget = false;
            }
            else
            {
                _selectedTarget = target;
                _outlineTarget = outline;
                _outlineTarget.OutlineColor = Color.yellow;
                _isselectedtarget = true;
            }
        }

        private void Update()
        {
            if (IsTarget() && Vector3.Distance(_selectedTarget.transform.position, transform.position) > 30)
            {
                UpdateSelectedTarget(_selectedTarget, _outlineTarget);
            }
        }

        private void Start()
        {
            if (PhotonNetwork.IsConnected && !gameObject.GetPhotonView().IsMine)
                enabled = false;
        }
    }
}