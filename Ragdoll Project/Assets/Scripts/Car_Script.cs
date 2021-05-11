using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Script : MonoBehaviour
{
    // Start is called before the first frame update

    float speed;
    void Start()
    {
        speed = Random.Range(3.0f, 8.0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.worldToLocalMatrix.MultiplyVector(new Vector3(0,0,-1f))*speed*Time.deltaTime;
    }
}
