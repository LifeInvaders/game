using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talking : MonoBehaviour
{
    private Animator anim;

    private bool animationOn = false;

    private IEnumerator _enumerator;

    // Start is called before the first frame update
    void Start()
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
        // Debug.Log("Coucou");
        anim.SetBool("talk",false);
        
        // anim.Play("talking");

        yield return new WaitForSeconds(Random.Range(15, 25));
        anim.SetBool("talk",true);
        
        animationOn = false;

    }
}
