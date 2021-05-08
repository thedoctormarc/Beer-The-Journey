using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinObject : MonoBehaviour
{
    // Start is called before the first frame update
    public float spin_speed = 0.02f;
    public bool backwards = false;

    // Update is called once per frame
    void Update()
    {
        Vector3 init = transform.localRotation.eulerAngles;

        if (!backwards)
            init.x += spin_speed;
        else
            init.x -= spin_speed;

        transform.Rotate(init, Space.Self);
    }
}
