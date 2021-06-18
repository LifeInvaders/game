using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HoldUI : MonoBehaviour
{
    [SerializeField] private Material material;
    [SerializeField] private TextMeshPro _textMeshPro;
    public float time;
    private float _startTime = 0;
    public Transform player;

    private void Update()
    {
        if (_startTime >= time)
            Destroy(gameObject);

        // gameObject.transform.LookAt(player);
        _startTime += Time.deltaTime;

        Debug.Log( Mathf.RoundToInt(time - _startTime));
        _textMeshPro.text = Mathf.RoundToInt(time - _startTime).ToString();
        material.SetFloat("Vector1_A5BC52FF", _startTime * 57 / time);
    }
}