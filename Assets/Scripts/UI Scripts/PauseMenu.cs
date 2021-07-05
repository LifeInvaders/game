using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject[] menus;
        [SerializeField] private PlayerInput playerinput;
        [SerializeField] private GameObject pauseMenuHUD;


        void Back()
        {
            var find = GameObject.Find("Back");
            if (find is null)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Resume();
                return;
            }

            find.GetComponent<Button>().onClick.Invoke();
        }

        public void SwitchMenu(int index)
        {
            for (int i = 0; i < menus.Length; i++)
                menus[i].SetActive(i == index);
        }

        public void LeaveGame()
        {
            if (PhotonNetwork.IsConnected)
                PhotonNetwork.LeaveRoom();
            else
                SceneManager.LoadScene("Scenes/MainMenu");
        }

        public void Resume()
        {
            pauseMenuHUD.SetActive(false);
            playerinput.SwitchCurrentActionMap("Player");
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                OnPause();
        }

        public void OnPause()
        {
            if (pauseMenuHUD.activeSelf)
            {
                Back();
            }
            else
            {
                Debug.Log(pauseMenuHUD.activeSelf);
                pauseMenuHUD.SetActive(true);
                playerinput.SwitchCurrentActionMap("Menu");
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}