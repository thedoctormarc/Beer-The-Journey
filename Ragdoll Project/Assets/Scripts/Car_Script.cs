using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BansheeGz.BGSpline.Curve;
using BansheeGz.BGSpline.Components;

public class Car_Script : MonoBehaviour
{
    public enum State { CIRCULATING, STOPPING, STOPPED, ACCELERATING }

 //   [HideInInspector]
    public State state;
    [HideInInspector]
    public State previousState;
    [SerializeField]
    GameObject curve_go;
    BGCurve curve;
    BGCcMath curve_math;
    float currentSpeed;
    float distanceAlongCurve;
    float currentAccelTime;
    [SerializeField]
    GameObject carInFront;
  //  [HideInInspector]
    public int intersectionPosition = 0;
    bool trackCarInFront = false;
    bool active = false;

    private void Awake()
    {
        IntersectionManager.instance.OnSwapEvent += OnIntersectionSwap;
        state = State.CIRCULATING;
        currentSpeed = CarManager.instance.topSpeed;

    }

    void Start()
    {
        if (curve_go == null)
        {
            this.enabled = false;
            return;
        }

        active = true;

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
        previousState = state;

        CheckFront();
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

    private void LateUpdate()
    {

        if (trackCarInFront)
        {
            if (CarInFrontBeganAccelerating())
            {
                trackCarInFront = false;
                Invoke("DoAccelerate", CarManager.instance.accelDelay);
            }
        }

    }

    // Used when I am still circulating when the next intersection is triggered. Schedule acceleration once the car in front does so
    bool CarInFrontBeganAccelerating()
    {
        if (carInFront == null)
        {
            return false;
        }
        // check that the other car begun accelerating last frame
        Car_Script car = carInFront.GetComponent<Car_Script>();

        if (car.state == State.ACCELERATING && car.previousState != State.ACCELERATING)
            return true;

        return false;
    }

    // Stop if intersection or stopped car in front
    void CheckFront()
    {
        if (state != State.CIRCULATING)
            return;

        Vector3 fw_right = (transform.forward + transform.right).normalized;
        Vector3 fw_right2 = (transform.forward + fw_right).normalized;
        Vector3 fw_right3 = (transform.forward + fw_right2).normalized;
        Vector3 fw_right4 = (fw_right2 + fw_right).normalized;
        Vector3 fw_left = (transform.forward - transform.right).normalized;
        Vector3 fw_left2 = (transform.forward + fw_left).normalized;
        Vector3 fw_left3 = (transform.forward + fw_left2).normalized;
        Vector3 fw_left4 = (fw_left2 + fw_left).normalized;

        if (RaycastFront(transform.forward))
            return;
        if (RaycastFront(fw_right))
            return;
        if (RaycastFront(fw_right2))
            return;
        if (RaycastFront(fw_right3))
            return;
        if (RaycastFront(fw_right4))
            return;
        if (RaycastFront(fw_left))
            return;
        if (RaycastFront(fw_left2))
            return;
        if (RaycastFront(fw_left3))
            return;
        if (RaycastFront(fw_left4))
            return;



    }

    bool RaycastFront(Vector3 direction) // Careful that this does not interpret further cars rather than the one in front
    {
        Vector3 offset = new Vector3(0f, 0.65f, 0f);
        RaycastHit[] hits = Physics.RaycastAll(transform.position + offset, direction, CarManager.instance.slowDownDistance);

        Debug.DrawLine(transform.position + offset, transform.position + offset + direction * CarManager.instance.slowDownDistance, Color.red);

        foreach (var hit in hits)
        {
            Car_Script car = hit.transform.gameObject.GetComponent<Car_Script>();

            Intersection inter = hit.transform.gameObject.GetComponent<Intersection>();

            if (car != null)
            {

                if (car.active == false)
                    continue;

                if (car.curve_go == curve_go) // only detect in the same lane
                {

                    if (carInFront == null)
                        carInFront = car.gameObject;
                    else if (car.gameObject != carInFront)
                        continue;

                    if (car.state == State.STOPPED || car.state == State.STOPPING)
                    {
                        float dist = (hit.point - transform.position).magnitude;
                        if (dist <= CarManager.instance.slowDownDistance)
                        {
                            intersectionPosition = 1 + car.intersectionPosition;
                            state = State.STOPPING;
                            return true;
                        }
                    }
                }
           
            }
            else if (inter != null)
            {
                // Stop in intersection!!
                if (inter.isCurveActive(curve_go) == false)
                {
                    float dist = (hit.point - transform.position).magnitude;
                    if (dist <= CarManager.instance.slowDownDistance)
                    {
                        bool tooLate = TooLateToBreak(inter.transform.position);

                        if (tooLate)
                            continue;

                        state = State.STOPPING;
                        return true;
                    }

                }

            }
        }

        return false;
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

    bool IsCloseToIntersection()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.transform.gameObject.GetComponent<Intersection>() != null)
            {
                float dist = (hit.point - transform.position).magnitude;
                if (dist <= CarManager.instance.slowDownDistance)
                {
                    return true;
                }
            }
        }

        return false;
    }

    bool TooLateToBreak(Vector3 intersectionPos) => (intersectionPos - transform.position).magnitude <= CarManager.instance.ignoreIntersectionDistance;

    private void OnIntersectionSwap(Vector3 intersectionPos, GameObject[] activeCurves, GameObject[] inActiveCurves)
    {
        foreach (GameObject go in inActiveCurves)
        {
            if (go == curve_go)
            {
                bool tooLate = TooLateToBreak(intersectionPos);

                if (tooLate)
                    return;

                if (IsCloseToIntersection())
                {
                    if (state == State.CIRCULATING)
                        state = State.STOPPING;
                }

                return;
            }
        }

        foreach (GameObject go in activeCurves)
        {
            if (go == curve_go)
            {
                Invoke("TriggerAccelerate", intersectionPosition * CarManager.instance.accelDelay);
            }
        }
    }

    void DoAccelerate()
    {
        intersectionPosition = 0;
        state = State.ACCELERATING;
    }


    void TriggerAccelerate()
    {

        if (state == State.STOPPED || state == State.STOPPING) // arrived to intersection already
        {
            DoAccelerate();
        }
        else // did not arrive, still circulating --> wait for car in front to begin accelerating
        {
            trackCarInFront = true;
        }

    }


}


