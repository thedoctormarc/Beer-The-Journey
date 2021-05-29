using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BansheeGz.BGSpline.Curve;
using BansheeGz.BGSpline.Components;

public class Car_Script : MonoBehaviour
{
    [SerializeField]
    GameObject curve_go;
    BGCurve curve;
    BGCcMath curve_math;
    [SerializeField]
    float speed = 5f;
    float distanceAlongCurve;
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
        distanceAlongCurve += speed * Time.deltaTime;
        Vector3 tangent;
        transform.position = curve_math.CalcPositionAndTangentByDistance(distanceAlongCurve, out tangent);
        transform.rotation = Quaternion.LookRotation(tangent);

        if (distanceAlongCurve > curve_math.GetDistance(curve.Points.Length))
        {
            distanceAlongCurve = 0f;
        }

    }


    private void OnCollisionEnter(Collision collision)
    {

        if(collision.gameObject.GetComponentInParent<CharacterLogic>() != null)
        {
            Vector3 dir = transform.position - collision.gameObject.transform.position;

            dir = -dir.normalized;
            collision.gameObject.GetComponent<Rigidbody>().AddForce(dir*200, ForceMode.Impulse);
        }
    }
}
