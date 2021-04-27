using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkingNPC : MonoBehaviour
{
    private Animator anim;

    private bool animationOn;

    private IEnumerator _enumerator;

    // Start is called before the first frame update
    void OnDisable()
    {
        animationOn = false;
    }

    
    void Awake()
    {
        // _enumerator = ;
        anim = GetComponent<Animator>();
    }
    

    // Update is called once per frame
    void Update()
    {
        if (!animationOn)
        {
            StartCoroutine(ExampleCoroutine());
            // Debug.Log("Au revoir"); 
        }
    }

    IEnumerator ExampleCoroutine()
    {
        animationOn = true;
        yield return new WaitForSeconds(Random.Range(0, 5));
        anim.SetBool("talk", true);


        yield return new WaitForSeconds(Random.Range(5, 10));
        anim.SetBool("talk", false);
        animationOn = false;
    }
}