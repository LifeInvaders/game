using System;
using System.Collections;
using Cinemachine;
using Discord_RPC;
using People.NPC;
using People.Player;
using TargetSystem;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Tutorial
{
    public class Tutorial : MonoBehaviour
    {
        // Start is called before the first frame update
        public GameObject DiscussionGameObject;
        private Discussion discussion;
        [SerializeField] private PlayerControler playerController;
        [SerializeField] private GameObject player;
        [SerializeField] private Camera _camera;
        private Animator _animator;
        private CameraControler _cameraController;
        private CastTarget _castTarget;
        private SelectedTarget _selectedTarget;
        private KillTarget _killTarget;
        private CinemachineBrain _ciinemachineBrain;

        public int _level = 0;
        public bool arrivedToTrigger = false;

        [SerializeField] private GameObject[] Spawns;

        [SerializeField] private Material DefaultMaterial;


        [Header("Level1")] [SerializeField] private CinemachineVirtualCamera VirtualCameraLevel1;
        [SerializeField] private MacroCanvasTutorial _macroCanvasTutorialLevel1;

        [Header("Level2")] [SerializeField] private CinemachineVirtualCamera VirtualCameraLevel2;
        [SerializeField] private MacroCanvasTutorial[] _macroCanvasTutorialsLevel2;

        [FormerlySerializedAs("NPC")] [Header("Level3")] [SerializeField]
        private GameObject NPCLevel3;

        [SerializeField] private GameObject finisher;
        [SerializeField] private CinemachineFollowZoom cinemachineFollowZoom;
        [SerializeField] private Transform NPCDest;
        [SerializeField] private GameObject ladderLevel3;
        [SerializeField] private CinemachineVirtualCamera VirtualCameraLevel3;

        [Header("Level4")] [SerializeField] private GameObject ladderLevel4;
        [SerializeField] private CinemachineVirtualCamera VirtualCameraLevel4;
        [SerializeField] private CinemachineVirtualCamera VirtualCameraLevel4NPC;
        [SerializeField] private GameObject NPCLevel4;
        [SerializeField] private MacroCanvasTutorial[] _macroCanvasTutorialsLevel4;


        [Header("Level5")] [SerializeField] private CinemachineVirtualCamera VirtualCameraLevel5NPC;
        [SerializeField] private CinemachineVirtualCamera VirtualCameraLevel5NPC2;
        [SerializeField] private CinemachineVirtualCamera VirtualCameraLevel5;
        
        [Header("Level6")] [SerializeField] private CinemachineVirtualCamera VirtualCameraLevel6;

        private void Start()
        {
            _animator = player.GetComponent<Animator>();
            _killTarget = player.GetComponent<KillTarget>();
            _selectedTarget = player.GetComponent<SelectedTarget>();
            _castTarget = player.GetComponent<CastTarget>();
            _ciinemachineBrain = player.GetComponentInChildren<CinemachineBrain>();
            _castTarget.enabled = false;
            _selectedTarget.enabled = false;
            _killTarget.enabled = false;
            
            PresenceManager.UpdateState("Joue au tutoriel");
            SetLevel();
        }

        public Discussion GetDiscussion() => discussion;

        public void SetTutorial(string text)
        {
            var i = Instantiate(DiscussionGameObject, transform);
            i.GetComponent<Canvas>().worldCamera = _camera;

            discussion = i.GetComponent<Discussion>();
            discussion.playerController = playerController;

            playerController.SetMoveBool(false);
            playerController.CheckIfRunning();
            playerController.SetRotateBool(false);
            discussion.gameObject.SetActive(true);
            discussion.SetText(text);

            _animator.SetBool("running", false);
            _animator.SetFloat("Speed Front", 0);
            _animator.SetFloat("Speed Side", 0);
        }

        public void TeleportToLevel()
        {
            player.transform.position = Spawns[_level - 1].transform.position;
            player.transform.rotation = Spawns[_level - 1].transform.rotation;
        }

        public void SetLevel()
        {
            arrivedToTrigger = false;
            _level++;
            PresenceManager.UpdatePresence(detail:$"Niveau {_level}");
            switch (_level)
            {
                case 1:
                    StartCoroutine(Level1());
                    break;
                case 2:
                    StartCoroutine(Level2());
                    break;
                case 3:
                    StartCoroutine(Level3());
                    break;
                case 4:
                    StartCoroutine(Level4());
                    break;
                case 5:
                    StartCoroutine(Level5());
                    break;
                case 6:
                    StartCoroutine(Level6());
                    break;
            }
        }

        private IEnumerator Level6()
        {
            TeleportToLevel();
            SetTutorial(
                "Un bon pirate a toujours plus d'un tour dans son  sac, et dans le mien, il y a de nombreuses capacités que tu vas pouvoir débloquer. Mais je te laisserais les découvrir sur le terrain l'ami. Tu as énormément appris aujourd'hui. Prend ce portail afin que je te ramène à la maison");
            while (!discussion.HasExited)
                yield return new WaitForEndOfFrame();

            VirtualCameraLevel6.Priority = 50;
            yield return new WaitForSeconds(2);
            VirtualCameraLevel6.Priority = 0;

            while (!arrivedToTrigger)
                yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(1);
            playerController.SetMoveBool(false);
            playerController.SetRotateBool(false);
            yield return new WaitForSeconds(1);
            Debug.Log("finish turorial");
            SceneManager.LoadScene("Scenes/MainMenu");
        }

        private IEnumerator Level5()
        {
            SetTutorial("Comme je te l'avais dit, il faut rester le plus discret possible.");
            while (!discussion.HasExited)
                yield return new WaitForEndOfFrame();

            VirtualCameraLevel5NPC.Priority = 50;
            yield return new WaitForSeconds(3);
            SetTutorial("Est-ce que tu vois ce groupe de personne discuter, faufile toi et va discuter avec eux.");
            while (!discussion.HasExited)
                yield return new WaitForEndOfFrame();
            VirtualCameraLevel5NPC.Priority = 0;

            while (!arrivedToTrigger)
                yield return new WaitForEndOfFrame();
            playerController.SetMoveBool(false);
            playerController.SetRotateBool(false);
            _ciinemachineBrain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Cut;
            VirtualCameraLevel5NPC2.Priority = 50;
            yield return new WaitForSeconds(2);

            SetTutorial("Tu est plutôt bien caché, où es tu l'ami ?");
            while (!discussion.HasExited)
                yield return new WaitForEndOfFrame();
            VirtualCameraLevel5NPC2.Priority = 0;
            _ciinemachineBrain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.EaseInOut;

            playerController.SetMoveBool(true);
            playerController.SetRotateBool(true);
            yield return new WaitForSeconds(2);
            SetTutorial("Tu sais maintenant comment te cacher.");
            while (!discussion.HasExited)
                yield return new WaitForEndOfFrame();
        }

        private IEnumerator Level4()
        {
            TeleportToLevel();
            SetTutorial(
                "Nous ne sommes pas encore assez puissant pour attaquer Jackie Sparow directement, nous nous devons de rester discret. Viens je vais t'apprendre comment un vrai pirate tue sa cible.");

            while (!discussion.HasExited)
                yield return new WaitForEndOfFrame();
            SetTutorial("Tue cette personne, elle m'a volé par le passé et mérite une petite correction.");
            while (!discussion.HasExited)
                yield return new WaitForEndOfFrame();

            VirtualCameraLevel4NPC.Priority = 50;
            yield return new WaitForSeconds(3);
            VirtualCameraLevel4NPC.Priority = 0;

            SetTutorial("Pour commencer, tu dois la cibler. Passe en mode verrouillage.");
            while (!discussion.HasExited)
                yield return new WaitForEndOfFrame();

            playerController.SetMoveBool(false);
            playerController.SetRotateBool(false);
            _castTarget.enabled = true;

            _macroCanvasTutorialsLevel4[0].FadeIn();
            while (!_castTarget._aiming)
                yield return new WaitForEndOfFrame();
            _macroCanvasTutorialsLevel4[0].FadeOut();
            yield return new WaitForSeconds(1);
            SetTutorial(
                "Un contour blanc autour de ta cible indique que tu la vise actuellement. Maintenant verrouille la !");
            var skinnedMeshRenderer = player.GetComponentInChildren<SkinnedMeshRenderer>();
            skinnedMeshRenderer.enabled = false;

            var NPCMeshRenderer = NPCLevel4.GetComponentInChildren<SkinnedMeshRenderer>();
            NPCMeshRenderer.enabled = false;


            while (!discussion.HasExited)
                yield return new WaitForEndOfFrame();
            playerController.SetMoveBool(false);
            skinnedMeshRenderer.enabled = true;
            NPCMeshRenderer.enabled = true;

            _selectedTarget.enabled = true;

            _macroCanvasTutorialsLevel4[1].FadeIn();

            while (!_selectedTarget.IsTarget())
                yield return new WaitForEndOfFrame();

            _macroCanvasTutorialsLevel4[1].FadeOut();
            yield return new WaitForSeconds(1);
            SetTutorial(
                "Une fois sélectionné tu n'as plus qu'à être assez proche de ta cible pour pouvoir l'éliminer.");
            var outline = NPCLevel4.GetComponentInChildren<Outline>();
            outline.enabled = false;


            while (!discussion.HasExited)
                yield return new WaitForEndOfFrame();

            outline.enabled = true;
            playerController.SetMoveBool(true);
            _macroCanvasTutorialsLevel4[2].FadeIn();
            while (!arrivedToTrigger)
                yield return new WaitForEndOfFrame();
            _macroCanvasTutorialsLevel4[2].FadeOut();
            yield return new WaitForSeconds(6);

            SetTutorial(
                "Bien joué, il me devait trois tournée d'eau-de-vie... C'était capital pour moi que tu me venges. Merci moussaillon !");

            while (!discussion.HasExited)
                yield return new WaitForEndOfFrame();

            VirtualCameraLevel4.Priority = 50;
            yield return new WaitForSeconds(2);
            ladderLevel4.GetComponentInChildren<BoxCollider>().isTrigger = true;
            ladderLevel4.GetComponent<MeshRenderer>().sharedMaterial = DefaultMaterial;
            yield return new WaitForSeconds(2);
            VirtualCameraLevel4.Priority = 0;


            arrivedToTrigger = false;
            while (!arrivedToTrigger)
                yield return new WaitForEndOfFrame();

            SetLevel();
        }

        private IEnumerator Level3()
        {
            TeleportToLevel();
            SetTutorial(
                "Sur l'île de Tortuga, tu vas pouvoir croiser de nombreux habitants. Parmi eux se cache l'horrible clan des...");
            while (!discussion.HasExited)
                yield return new WaitForEndOfFrame();

            SetTutorial("MAIS C'EST ...");
            discussion.SetAnim("Terrified");
            while (!discussion.HasExited)
                yield return new WaitForEndOfFrame();
            playerController.SetRotateBool(false);
            playerController.SetMoveBool(false);
            var navMeshAgent = NPCLevel3.GetComponent<NavMeshAgent>();
            navMeshAgent.destination = NPCDest.position;
            var cinemachineVirtualCamera = cinemachineFollowZoom.GetComponent<CinemachineVirtualCamera>();
            cinemachineVirtualCamera.Priority = 50;
            while (cinemachineFollowZoom.m_Width > 2)
            {
                cinemachineFollowZoom.m_Width -= 15 * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            while (navMeshAgent.pathPending || navMeshAgent.remainingDistance > 0.5f)
                yield return new WaitForEndOfFrame();

            cinemachineVirtualCamera.Priority = 0;

            var g = Instantiate(this.finisher, NPCLevel3.transform.position,
                NPCLevel3.transform.rotation);
            var finisher = g.GetComponent<Finisher>();
            finisher.SetDead(NPCLevel3.GetComponentInChildren<SkinnedMeshRenderer>());

            // finisher.player = player;
            finisher.cinemachineBrain = _ciinemachineBrain;
            Destroy(NPCLevel3);
            TeleportToLevel();
            yield return new WaitForSeconds(16);


            SetTutorial(
                "Mille millions de mille sabords!");

            while (!discussion.HasExited)
                yield return new WaitForEndOfFrame();

            SetTutorial(
                "C'est le capitaine Jackie Sparrow ! Il s'agit d'un redoutable tueur de pirates connu dans les sept mers. Depuis qu'il y a été touché par la malédiction de Barbe-Noire, ce n'est plus le même, il se met à tuer des innocents.");
            while (!discussion.HasExited)
                yield return new WaitForEndOfFrame();

            SetTutorial(
                "Nous devons l'arrêter, lui et ses partisans, quoi qu'il nous en coûte !");

            while (!discussion.HasExited)
                yield return new WaitForEndOfFrame();

            SetTutorial(
                "Bon, sortons d'ici avant qu'il revienne. Prends cette échelle pour t'enfuir.");

            while (!discussion.HasExited)
                yield return new WaitForEndOfFrame();
            _ciinemachineBrain.m_DefaultBlend.m_Time = 2;
            _ciinemachineBrain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.EaseInOut;
            VirtualCameraLevel3.Priority = 50;
            yield return new WaitForSeconds(2);
            ladderLevel3.GetComponentInChildren<BoxCollider>().isTrigger = true;
            ladderLevel3.GetComponent<MeshRenderer>().sharedMaterial = DefaultMaterial;
            yield return new WaitForSeconds(2);
            VirtualCameraLevel3.Priority = 0;


            arrivedToTrigger = false;
            while (!arrivedToTrigger)
                yield return new WaitForEndOfFrame();

            SetLevel();
        }


        private IEnumerator Level2()
        {
            SetTutorial(
                "Ne pas courir te permet de rester un pirate discret et donc de ne pas attirer l'attention sur toi. Mais personnellement j'aime me contre dire. Donc tu vas courir jusqu'au tonneau situé en haut.");
            TeleportToLevel();
            while (!discussion.HasExited)
                yield return new WaitForEndOfFrame();

            VirtualCameraLevel2.Priority = 50;
            yield return new WaitForSeconds(4);
            VirtualCameraLevel2.Priority = 0;

            playerController.SetCanRun(true);
            _macroCanvasTutorialsLevel2[0].FadeIn();
            arrivedToTrigger = false;
            while (!arrivedToTrigger)
                yield return new WaitForEndOfFrame();
            _macroCanvasTutorialsLevel2[0].FadeOut();
            SetTutorial("Pour enjamber la barrière, saute comme un mouton !");
            while (!discussion.HasExited)
                yield return new WaitForEndOfFrame();
            _macroCanvasTutorialsLevel2[1].FadeIn();
            arrivedToTrigger = false;
            while (!arrivedToTrigger)
                yield return new WaitForEndOfFrame();
            _macroCanvasTutorialsLevel2[1].FadeOut();

            SetTutorial(
                "Sur l'île de Tortuga, tu pourras te déplacer de toit en toit. Des échelles te permettront de les atteindre. Pour les utiliser, rien de plus simple, marche sur l'échelle.");
            while (!discussion.HasExited)
                yield return new WaitForEndOfFrame();

            arrivedToTrigger = false;
            while (!arrivedToTrigger)
                yield return new WaitForEndOfFrame();

            playerController.enabled = false;

            _macroCanvasTutorialsLevel2[2].FadeIn();

            playerController.enabled = true;
            playerController.SetRotateBool(false);
            playerController.SetMoveBool(false);
            playerController.SetCanRun(false);

            arrivedToTrigger = false;
            while (!arrivedToTrigger)
                yield return new WaitForEndOfFrame();
            _macroCanvasTutorialsLevel2[2].FadeOut();
            SetTutorial("Voilà, tu connais les bases des déplacements.");
            while (!discussion.HasExited)
                yield return new WaitForEndOfFrame();

            SetLevel();
        }

        private IEnumerator Level1()
        {
            SetTutorial(
                "Ohé jeune matelos. Je suis le capitaine Octopus, laissez moi vous guider et vous apprendre les bases avant que vous vous lanciez dans de trépidantes aventures !");
            while (!discussion.HasExited)
                yield return new WaitForEndOfFrame();
            SetTutorial("Commencons par les bases. Va vers le barril de rhum.");
            while (!discussion.HasExited)
                yield return new WaitForEndOfFrame();
            VirtualCameraLevel1.Priority = 50;
            yield return new WaitForSeconds(4);
            VirtualCameraLevel1.Priority = 0;
            _macroCanvasTutorialLevel1.FadeIn();
            playerController.SetMoveBool(true);
            playerController.SetCanRun(false);
            while (!arrivedToTrigger)
                yield return new WaitForEndOfFrame();
            _macroCanvasTutorialLevel1.FadeOut();
            SetTutorial("Hum le bon rhum...");
            while (!discussion.HasExited)
                yield return new WaitForEndOfFrame();

            SetLevel();
        }


        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
                SetLevel();
        }
    }
}