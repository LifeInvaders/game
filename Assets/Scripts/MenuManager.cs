using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> menus = new List<GameObject>();
    [SerializeField] private Dictionary<string, GameObject> item;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void SwitchMenu(int index)
    {
        for (int i = 0; i < menus.Count; i++) 
            menus[i].SetActive(i==index);
    }

    public void Quit() => Application.Quit();
}
