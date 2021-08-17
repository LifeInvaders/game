using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BackgroundTransition : MonoBehaviour
{
    // Start is called before the first frame update
    private Image[] images;
    private int _index;

    private float _time;

    void Start()
    {
        images = GetComponentsInChildren<Image>();
        for (int i = 1; i < images.Length; i++)
        {
            images[i].gameObject.SetActive(false);
        }
        _time = 5;
    }

    // Update is called once per frame
    void Update()
    {
        _time -= Time.deltaTime;
        if (_time <= 0)
        {
            _time = 5;
            StartCoroutine(Fade());
        }
    }

    private IEnumerator Fade()
    {
        var t = 0f;
        var nextImg = (_index + 1) % images.Length;
        images[nextImg].gameObject.SetActive(true);
        
        while (t <= 2)
        {
            t += Time.deltaTime;
            images[_index].color = new Color(1, 1, 1, 1 - t / 2);
            images[nextImg].color = new Color(1, 1, 1, t / 2 + 0.2f);

            yield return new WaitForEndOfFrame();
        }

        images[nextImg].color = Color.white;
        images[_index].gameObject.SetActive(false);
        _index = nextImg;
    }
}