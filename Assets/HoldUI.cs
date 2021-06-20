using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HoldUI : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
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

        _textMeshPro.text = Mathf.RoundToInt(time - _startTime).ToString();
        rectTransform.localScale = new Vector3(1, 1 - _startTime / time, 1);
    }
}