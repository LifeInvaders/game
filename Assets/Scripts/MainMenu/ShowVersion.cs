using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowVersion : MonoBehaviour
{
    void Start() => GetComponent<TextMeshProUGUI>().text = "Version: " + Application.version;
}
