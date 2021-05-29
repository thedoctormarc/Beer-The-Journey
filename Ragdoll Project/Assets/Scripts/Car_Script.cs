using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BansheeGz.BGSpline.Curve;
using BansheeGz.BGSpline.Components;

public class Car_Script : MonoBehaviour
{
    [SerializeField]
    GameObject curve_go;
    BGCcMath curve_math;
    [SerializeField]
    float speed;
    float distanceAlongCurve;
    void Start()
    {
        curve_math = curve_go.GetComponent<BGCcMath>();
    }

    void Update()
    {
        distanceAlongCurve += speed * Time.deltaTime;
        Vector3 tangent;
        transform.position = curve_math.CalcPositionAndTangentByDistance(distanceAlongCurve, out tangent);
        transform.rotation = Quaternion.LookRotation(tangent);
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
