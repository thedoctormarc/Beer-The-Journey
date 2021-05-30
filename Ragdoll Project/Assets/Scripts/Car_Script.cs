using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BansheeGz.BGSpline.Curve;
using BansheeGz.BGSpline.Components;

public class Car_Script : MonoBehaviour
{
    public enum State { CIRCULATING, STOPPING, STOPPED, ACCELERATING }
    public State state;
    [SerializeField]
    GameObject curve_go;
    BGCurve curve;
    BGCcMath curve_math;
    float currentSpeed;
    float distanceAlongCurve;
    float currentAccelTime;

    private void Awake()
    {
        IntersectionManager.instance.OnSwapEvent += OnIntersectionSwap;
        state = State.CIRCULATING;
        currentSpeed = CarManager.instance.topSpeed;

    }

    void Start()
    {
     
        curve_math = curve_go.GetComponent<BGCcMath>();
        curve = curve_go.GetComponent<BGCurve>();
    
        int closest_index = 0;
        float closest_dist = float.MaxValue;
        for (int i = 0; i < curve.Points.Length; ++i)
        {
            var point = curve.Points[i];
            float dist = (point.PositionWorld - transform.position).magnitude;

            if (dist < closest_dist)
            {
                closest_dist = dist;
                closest_index = i;
            }
        }

        distanceAlongCurve = curve_math.GetDistance(closest_index);
    }

    void Update()
    {
        HandleSpeed();

        distanceAlongCurve += currentSpeed * Time.deltaTime;
        Vector3 tangent;
        transform.position = curve_math.CalcPositionAndTangentByDistance(distanceAlongCurve, out tangent);
        transform.rotation = Quaternion.LookRotation(tangent);

        if (distanceAlongCurve > curve_math.GetDistance())
        {
            distanceAlongCurve = 0f;
        }

    }

    void HandleSpeed ()
    {
        switch(state)
        {
            case State.ACCELERATING:
                {
                    currentSpeed = CarManager.instance.accelCurve.Evaluate(currentAccelTime += Time.deltaTime) * CarManager.instance.topSpeed;

                    if (currentSpeed >= CarManager.instance.topSpeed)
                    {
                        currentSpeed = CarManager.instance.topSpeed;
                        currentAccelTime = 0f;
                        state = State.CIRCULATING;
                    }

                    break;
                }
            case State.STOPPING:
                {
                    currentSpeed = CarManager.instance.deAccelCurve.Evaluate(currentAccelTime += Time.deltaTime) * CarManager.instance.topSpeed;

                    if (currentSpeed <= 0f)
                    {
                        currentSpeed = 0f;
                        currentAccelTime = 0f;
                        state = State.STOPPED;
                    }

                    break;
                }
        }
    }


    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.GetComponentInParent<CharacterLogic>() != null)
        {
            Vector3 dir = transform.position - collision.gameObject.transform.position;

            dir = -dir.normalized;
            collision.gameObject.GetComponent<Rigidbody>().AddForce(dir * 200, ForceMode.Impulse);
        }
    }

    private void OnIntersectionSwap(GameObject[] activeCurves, GameObject[] inActiveCurves)
    {

        if (state == State.CIRCULATING)
        {
            foreach (GameObject go in inActiveCurves)
            {
                if (go == curve_go)
                {
                    state = State.STOPPING;
                    return;
                }
            }
        }
        else if (state == State.STOPPED)
        {
            foreach (GameObject go in activeCurves)
            {
                if (go == curve_go)
                {
                    state = State.ACCELERATING;
                    return;
                }
            }
        }
    }
}


