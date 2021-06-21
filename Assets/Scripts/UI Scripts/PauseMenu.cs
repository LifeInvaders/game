using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject[] menus;
        [SerializeField] private PlayerInput playerinput;
        void Update()
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                    GameObject.Find("Back").GetComponent<Button>().onClick.Invoke();
            }
        
            public void SwitchMenu(int index)
            {
               
                for (int i = 0; i < menus.Length; i++)
                    menus[i].SetActive(i==index);
            }

            public void LeaveGame()
            {
                
            }
            
            public void Resume()
            {
                gameObject.SetActive(false);
                playerinput.SwitchCurrentActionMap("Player");
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            public void OnPause()
            {
                if (gameObject.activeSelf) return;
                Debug.Log("cc");
                gameObject.SetActive(true);
                playerinput.SwitchCurrentActionMap("Menu");
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

            }
    }
}