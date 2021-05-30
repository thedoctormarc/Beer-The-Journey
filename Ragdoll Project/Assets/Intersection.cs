using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intersection : MonoBehaviour
{
    [SerializeField]
    GameObject[] direction1Curves;
    [SerializeField]
    GameObject[] direction2Curves;
    [SerializeField]
    [Range(15f, 45f)]
    float swapTime = 20f;
    float currentTime;
    bool dir_1 = true;


    void Start()
    {
        IntersectionManager.instance.OnSwapIntersection(direction1Curves, direction2Curves); // Initialize traffic, overrate state and speed
    }

    void Update()
    {
        if ((currentTime += Time.deltaTime) >= swapTime)
        {
            dir_1 = !dir_1;
            IntersectionManager.instance.OnSwapIntersection((dir_1) ? direction1Curves : direction2Curves, (dir_1) ? direction2Curves : direction1Curves);
            currentTime = 0f;
        }
    }
    

}
