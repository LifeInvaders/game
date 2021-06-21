using System.Collections;
using People;
using People.Player;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Rendering;

namespace Objects.Powers
{
    public class PhotonSpeedBoost : PowerTools
    {
        private HumanEvent _playerEvent;
        private PlayerControler _playerControler;
        private float _resetTime;

        private Animator _animator;
        [SerializeField] private Volume _volume;
        private Transform[] _members;
        private GameObject[] _bodyMembersSmoke;

        [SerializeField] private GameObject smokeEffect;


        protected override void SetValues()
        {
            _bodyMembersSmoke = new GameObject[2];
            _members =  new Transform[]
                {gameObject.transform.Find("Root/Hips/Spine_01/Spine_02/Spine_03/Clavicle_L/Shoulder_L/Elbow_L/Hand_L").transform, gameObject.transform.Find("Root/Hips/Spine_01/Spine_02/Spine_03/Clavicle_R/Shoulder_R/Elbow_R/Hand_R").transform};
            

            _animator = GetComponent<Animator>();
            _time = 45;
            TimeBeforeUse = 10;
            _resetTime = 6;
            _playerControler = GetComponent<PlayerControler>();
            _playerEvent = GetComponent<HumanEvent>();
        }

        protected override bool IsValid() => _playerControler.CanRun() && _playerEvent.humanTask != HumanTasks.SmokeBomb &&  _playerEvent.humanTask != HumanTasks.Bleeding &&  _playerEvent.humanTask != HumanTasks.Poisoned;

        protected override void Action()
        {
            for (int i = 0; i < 2; i++)
            {
                var smoke = PhotonNetwork.Instantiate("Run Smoke", _members[i].position, _members[i].rotation);
                gameObject.GetPhotonView().RPC(nameof(SetSmokeParent),RpcTarget.All,smoke.GetPhotonView().ViewID,i);
            }
            _animator.SetFloat("super run", 1);
            StartCoroutine(FadeIn());
            _playerControler.SetRunSpeed(10);
            _playerControler.CheckIfRunning();
            _playerEvent.humanTask = HumanTasks.SpeedRunning;
            StartCoroutine(EndPower());
        }

        [PunRPC]
        void SetSmokeParent(int viewID,int index)
        {
            var smoke = PhotonNetwork.GetPhotonView(viewID).gameObject;
            _bodyMembersSmoke[index] = smoke;
            smoke.transform.parent = _members[index].transform;
        }

        [PunRPC]
        void StopParticles(int viewID) =>
            PhotonNetwork.GetPhotonView(viewID).gameObject.GetComponent<ParticleSystem>().Stop();

        private IEnumerator EndPower()
        {
            yield return new WaitForSeconds(_resetTime);
            _playerControler.SetRunSpeed(6);
            _playerControler.CheckIfRunning();
            _animator.SetFloat("super run", 0);
            _playerEvent.humanTask = HumanTasks.Nothing;
            float time = 0.5f;
            for (int i = 0; i < 2; i++)
            {
                gameObject.GetPhotonView().RPC(nameof(StopParticles),RpcTarget.All,_bodyMembersSmoke[i].GetPhotonView().ViewID);
                yield return new WaitForSeconds(5);
                PhotonNetwork.Destroy(_bodyMembersSmoke[i]);
            }
            while (time > 0)
            {
                _volume.weight = time * 2;
                time -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            _volume.weight = 0;
        }

        private IEnumerator FadeIn()
        {
            float time = 0;
            while (time <= 0.5f)
            {
                _volume.weight = time * 2;
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            _volume.weight = 1;
        }
    }
}