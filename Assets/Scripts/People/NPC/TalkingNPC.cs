using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkingNPC : MonoBehaviour
{
    private Animator _anim;

    private bool _animationOn;
    private string _animationString = "talk";

    public void SetAnim(string anim) => _animationString = anim;
    private IEnumerator _enumerator;

    // Start is called before the first frame update
    void OnDisable()
    {
        _animationOn = false;
    }

    
    void Awake()
    {
        // _enumerator = ;
        _anim = GetComponent<Animator>();
    }
    

    // Update is called once per frame
    void Update()
    {
        if (!_animationOn)
        {
            StartCoroutine(ExampleCoroutine());
            // Debug.Log("Au revoir"); 
        }
    }

    IEnumerator ExampleCoroutine()
    {
        _animationOn = true;
        yield return new WaitForSeconds(Random.Range(0, 5));
        _anim.SetBool("talk", true);


        yield return new WaitForSeconds(Random.Range(5, 10));
        _anim.SetBool("talk", false);
        _animationOn = false;
    }
}