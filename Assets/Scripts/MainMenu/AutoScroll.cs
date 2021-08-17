using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AutoScroll : MonoBehaviour
{
    [SerializeField] private RectTransform content;
    private float _startPos;

    [SerializeField] private float speed;

    [SerializeField] private UnityEvent whenFinished;
    [SerializeField] private UnityEvent OnStart;

    private void Awake() => _startPos = content.localPosition.y;

    private void OnEnable()
    {
        var pos = content.localPosition;
        Debug.Log(_startPos);
        Debug.Log(pos.y);
        content.localPosition = new Vector3(pos.x, _startPos, pos.z);
        OnStart.Invoke();
    }

    void Update()
    {
        if (content.localPosition.y >= 4750)
        {
            StartCoroutine(Wait());
        }
        else
        {
            content.localPosition += Vector3.up * (Time.deltaTime * speed * (Input.GetKey(KeyCode.DownArrow) ? 3 : 1));
        }
    }

    public void Quit() => whenFinished.Invoke();
    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);
        Quit();
    }
}