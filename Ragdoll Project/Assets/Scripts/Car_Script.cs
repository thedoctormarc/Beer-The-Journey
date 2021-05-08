using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Script : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.worldToLocalMatrix.MultiplyVector(new Vector3(0,0,-1f))*0.1f;
    }
}
