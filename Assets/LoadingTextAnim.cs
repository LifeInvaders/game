using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LoadingTextAnim : MonoBehaviour

{
    private Text _text;
    private int _index;
    private bool loading = true;

    // Start is called before the first frame update
    void Awake()
    {
        _text = GetComponent<Text>();
        StartCoroutine(UpdateText());
    }

    // Update is called once per frame
    private IEnumerator UpdateText()
    {
        yield return new WaitForSeconds(0.5f);
        while (loading)
        {
            switch (_index)
            {
                case 0:
                    _text.text = "Loading";
                    break;
                case 1:
                    _text.text = "Loading.";
                    break;
                case 2:
                    _text.text = "Loading..";
                    break;
                case 3:
                    _text.text = "Loading...";
                    break;
            }
        
        yield return new WaitForSeconds(0.5f);
            _index = (_index + 1) % 4;
        }
        
    }

    private void OnDisable()
    {
        loading = false;
    }
}