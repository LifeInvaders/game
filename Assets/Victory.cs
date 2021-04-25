using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Victory : MonoBehaviour
{
    [Header("Winners")] 
    [SerializeField] private GameObject FirstWinner;
    [SerializeField] private GameObject SecondWinner;
    [SerializeField] private GameObject ThirdWinner;
    
    [Header("Losers")] 
    [SerializeField] private GameObject FirstLoser;
    [SerializeField] private GameObject SecondLoser;
    [SerializeField] private GameObject ThirdLoser;
    
    [Header("WinnerText")]
    [SerializeField] private TextMeshPro FirstWinnerText;
    [SerializeField] private TextMeshPro SecondWinnerText;
    [SerializeField] private TextMeshPro ThirdWinnerText;
    
    [Header("LoosersText")]
    [SerializeField] private TextMeshPro FirstLoserText;
    [SerializeField] private TextMeshPro SecondLoserText;
    [SerializeField] private GameObject ThirdLoserText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
