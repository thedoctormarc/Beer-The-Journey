using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_NPC : MonoBehaviour
{
    // Start is called before the first frame update
    float speed = 1f;
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("walk", true);
    }

    // Update is called once per frame
    void Update()
    {

        //Provisional
        transform.position += transform.worldToLocalMatrix.MultiplyVector(new Vector3(0, 0, -1f)) * speed * Time.deltaTime;

    }
}
