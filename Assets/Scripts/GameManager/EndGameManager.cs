using System.Collections;
using People.Player;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace GameManager
{
    public class EndGameManager : MonoBehaviourPunCallbacks
    {

        [SerializeField] private GameObject statsDisplay;
        [SerializeField] private GameObject choiceDisplay;
        [SerializeField] private GameObject restartDisplay;
        [SerializeField] private InGameStats igs;
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private GameObject timer;
        [SerializeField] private GameObject victoryScene;
        private PlayerDatabase _playerDatabase;
        private Coroutine _leaveCountdown;
        [SerializeField] private Image fadeToBlack;

        void Start()
        {
            _playerDatabase = PlayerDatabase.Instance;
        }

        private void DisplayStats(int exp)
        {
            var displayString = string.Empty;
            displayString += '\n';
            displayString += "Rank: " + scoreManager.GetRank() + '\n';
            displayString += '\n';
            displayString += "Score: " + PhotonNetwork.LocalPlayer.GetScore() + '\n';
            displayString += '\n';
            displayString += "Kills: " + igs.killCount + '\n';
            displayString += '\n';
            displayString += "Deaths: " + igs.deathCount + '\n';
            displayString += '\n';
            displayString += "Experience: " + exp + '\n';
            statsDisplay.GetComponent<Text>().text = displayString;
        }

        private int GetExp()
        {
            var score = PhotonNetwork.LocalPlayer.GetScore();
            var rand = new System.Random();
            var exp = score * rand.Next(1, 3);
            return exp < 0 ? 0 : exp;
        }

        public void StartEndRoundCoroutine()
        {
            Debug.Log("Game End!");
            StartCoroutine(EndRound());
        }

        void EndSceneTransition()
        {
            Mathf.MoveTowards(fadeToBlack.color.a, 1,0.1f); 
            statsDisplay.SetActive(false);
            victoryScene.GetComponent<Victory>().scoreManager = scoreManager;
            Instantiate(victoryScene, transform.position,transform.rotation);
            Mathf.MoveTowards(fadeToBlack.color.a, 0,0.1f);
        }

        IEnumerator EndRound()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            igs.localPlayer.GetComponent<PlayerControler>().enabled = false;
            igs.localPlayer.GetComponent<CameraControler>().enabled = false;
            int exp = GetExp();
            statsDisplay.SetActive(true);
            UpdateDatabase(exp);
            yield return new WaitForSeconds(5);
            EndSceneTransition();
            yield return new WaitForSeconds(5);
            _leaveCountdown = StartCoroutine(PromptRestart());
        }

        private void UpdateDatabase(int exp)
        {
            Levelling(exp);
            _playerDatabase.Deaths += igs.deathCount;
            _playerDatabase.TargetKills += igs.killCount;
            _playerDatabase.Games++;
            _playerDatabase.FirstPlace += scoreManager.GetRank() == 1 ? 1 : 0;
        }

        private void Levelling(int exp)
        {
            int beforeAdd = _playerDatabase.Exp;
            Mathf.MoveTowards(_playerDatabase.Exp,
                Mathf.Min(_playerDatabase.Exp + exp, _playerDatabase.Level * _playerDatabase.LevelExpReqMult), 1);
            if (_playerDatabase.Exp != _playerDatabase.Level * _playerDatabase.LevelExpReqMult) return;
            _playerDatabase.Exp = 0;
            _playerDatabase.Level++;
            Levelling(_playerDatabase.Level * _playerDatabase.LevelExpReqMult - beforeAdd);
        }
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public void Quit()
        {
            PhotonNetwork.LeaveRoom();
        }

        public void CheckChoiceAndRestart()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
            {
                if (!player.CustomProperties.TryGetValue("restart", out var restart)
                    || !(bool) restart)
                {
                    return;
                }
            }
            StartCoroutine(Restart());
        }

        IEnumerator PromptRestart()
        {
            choiceDisplay.SetActive(true);
            Text timer = this.timer.GetComponent<Text>();
            timer.text = "10";
            for (int i = 10; i >= 0; i--)
            {
                timer.text = i.ToString();
                yield return new WaitForSeconds(1);
            }
            Quit();
        }

        public void ChooseRestart()
        {
            StopCoroutine(_leaveCountdown);
            choiceDisplay.SetActive(false);
            restartDisplay.SetActive(true);
            InvokeRepeating(nameof(CheckChoiceAndRestart), 0, 5);
        }

        IEnumerator Restart()
        {
            CancelInvoke(nameof(CheckChoiceAndRestart));
            yield return new WaitForSeconds(5);
            if (!PhotonNetwork.IsMasterClient) yield break;
            PhotonNetwork.CurrentRoom.IsOpen = true;
            PhotonNetwork.LoadLevel("Lobby");
        }
    }
}