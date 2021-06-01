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

    GameObject[] activeCurves;
    GameObject[] inactiveCurves;

    private void Awake()
    {
        activeCurves = direction1Curves;
        inactiveCurves = direction2Curves;
    }

    void Start()
    {
        IntersectionManager.instance.OnSwapIntersection(transform.position, direction1Curves, direction2Curves); // Initialize traffic, overrate state and speed
    }

    void Update()
    {
        if ((currentTime += Time.deltaTime) >= swapTime)
        {
            SwapActive();
            IntersectionManager.instance.OnSwapIntersection(transform.position, (dir_1) ? direction1Curves : direction2Curves, (dir_1) ? direction2Curves : direction1Curves);
            currentTime = 0f;
        }
    }

    void SwapActive()
    {
        dir_1 = !dir_1;
        GameObject[] aux = activeCurves;
        activeCurves = inactiveCurves;
        inactiveCurves = aux;
    }

    public bool isCurveActive (GameObject curve)
    {
        bool foundActive = false;

        foreach (GameObject go in activeCurves)
        {
            if (go == curve)
            {
                foundActive = true;
                break;
            }
        }

        return foundActive;
    }

 
}
