using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BansheeGz.BGSpline.Curve;
using BansheeGz.BGSpline.Components;

public class NPC : MonoBehaviour
{
    [SerializeField]
    GameObject curve_go;
    BGCurve curve;
    BGCcMath curve_math;
    float distanceAlongCurve;
    [SerializeField]
    float speed;
    AudioSource aS;

    // Start is called before the first frame update
    void Start()
    {
        aS = gameObject.GetComponent<AudioSource>();

        if (curve_go == null)
        {
            this.enabled = false;
            return;
        }
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

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.I))
        {
            OnPlayerInteraction();
        }

        distanceAlongCurve += speed * Time.deltaTime;
        Vector3 tangent;


        transform.position = curve_math.CalcPositionAndTangentByDistance(distanceAlongCurve, out tangent);
        transform.rotation = Quaternion.LookRotation(tangent);

        if (distanceAlongCurve > curve_math.GetDistance())
        {
            distanceAlongCurve = 0f;
        }
    }

    public void OnPlayerInteraction()
    {
        if (aS.isPlaying == false)
        {
            aS.Play();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject go = collision.gameObject;

        if (go.CompareTag("Player"))
        {
            OnPlayerInteraction();
        }
    }
}
