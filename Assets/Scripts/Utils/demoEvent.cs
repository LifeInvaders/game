using People.Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class demoEvent : MonoBehaviour
{
    // Start is called before the first frame update
    private bool changeValue = false;
    private PlayerControler playerControler;

    private Rigidbody rb;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerControler = GetComponent<PlayerControler>();
        rb = GetComponent<Rigidbody>();
    }

    public void OnBoostDev()
    {
        
        if (changeValue)
        {
            playerControler.SetWalkSpeed(6);
            playerControler.SetRunSpeed(12);
            playerControler.SetJumpSpeed(10);
        }
        else
        {
            playerControler.SetWalkSpeed(3);
            playerControler.SetRunSpeed(6);
            playerControler.SetJumpSpeed(5);
        }
        changeValue = !changeValue;
    }

    public void OnGravityDev()
    {
        rb.useGravity = !rb.useGravity;
    }

    public void OnLeave()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
}
