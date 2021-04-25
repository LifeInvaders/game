using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class NightMode : MonoBehaviour
{
    // Start is called before the first frame update
    public Light[] _lights;
    
    [Header("Day/Night Lights")]
    [SerializeField] private GameObject dayLight;
    [SerializeField] private GameObject nightLight;
    
    
    [SerializeField] private GameObject[] rain;
    [SerializeField] private Volume _volume;

    void Start() => FindLights();

    public void FindLights() =>  _lights = FindObjectsOfType<Light>().Where(l => l.type != LightType.Directional).ToArray();
    
    /// <summary>
    /// Change Day/Night Mode
    /// </summary>
    /// <param name="activated">True for the day, false for the night</param>
    
    public void ChangeMode(bool activated = true)
    {
        _volume.enabled = !activated;
        dayLight.SetActive(activated);
        nightLight.SetActive(!activated);
        foreach (var light in _lights)
        {
            light.enabled = !activated;
            for (int i = 0; i < light.gameObject.transform.childCount; i++)
            {
                light.gameObject.transform.GetChild(i).gameObject.SetActive(!activated);
            }
        }

        foreach (var r in rain)
            r.SetActive(!activated);
    }
    
}