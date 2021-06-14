using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour
{
    public static CarManager instance;
    public AnimationCurve accelCurve, deAccelCurve;
    public float topSpeed = 8f;
    public float slowDownDistance = 15f;
    public float ignoreIntersectionDistance = 10f;
    public float accelDelay = 1.5f;

    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
