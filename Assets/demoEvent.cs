using People.Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class demoEvent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBoostDev(InputValue value)
    {
        var playerControler = GetComponent<PlayerControler>();
        if (value.isPressed)
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
    }
    
}
