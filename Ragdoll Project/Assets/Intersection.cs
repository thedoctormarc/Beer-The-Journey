using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intersection : MonoBehaviour
{
    [SerializeField]
    GameObject cars;
    [SerializeField]
    GameObject[] direction1Curves;
    [SerializeField]
    GameObject[] direction2Curves;
    [SerializeField]
    [Range(15f, 45f)]
    float swapTime = 20f;
    float currentTime;

    void Start()
    {
        
    }

    void Update()
    {
        if ((currentTime += Time.deltaTime) >= swapTime)
        {

        }
    }
}
