using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Rendering;

public class NightMode : MonoBehaviour
{
    // Start is called before the first frame update
    public Light[] _lights;
    
    [Header("Day/Night Lights")]
    [SerializeField] private GameObject dayLight;
    [SerializeField] private GameObject nightLight;
    
    
    [SerializeField] private GameObject rain;
    [SerializeField] private Volume volume;

    void Start()
    {
        var rand = new System.Random();
        FindLights();
        if (PhotonNetwork.IsMasterClient && rand.NextDouble() < 0.01) gameObject.GetPhotonView().RPC("ChangeMode",RpcTarget.All);
    }

    public void FindLights() =>  _lights = FindObjectsOfType<Light>().Where(l => l.type != LightType.Directional).ToArray();
    
    /// <summary>
    /// Change Day/Night Mode
    /// </summary>
    /// <param name="activated">True for the day, false for the night</param>

    [PunRPC]
    public void ChangeMode()
    {
        bool activated = volume.enabled;
        volume.enabled = !activated;
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
        rain.SetActive(!activated);
    }
}