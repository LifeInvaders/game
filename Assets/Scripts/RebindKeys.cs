using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public class RebindKeys : MonoBehaviour
{
    [SerializeField] private Button forward;
    [SerializeField] private Button backward;
    [SerializeField] private Button left;
    [SerializeField] private Button right;
    [SerializeField] private Button run;
    [SerializeField] private Button attack;
    [SerializeField] private Button target;



    private Dictionary<int, Button> ButtonBind()
    {
        Dictionary<int, Button> dict = new Dictionary<int, Button>();
        dict.Add(0, forward);
        dict.Add(1, backward);
        dict.Add(0, left);
        dict.Add(0, right);
        dict.Add(0, run);
        dict.Add(0, attack);
        dict.Add(0, target);
        return dict;
    }
    
    void Start()
    {
    }
    
    
    private void UpdateButton()
    {
        
    }
}
