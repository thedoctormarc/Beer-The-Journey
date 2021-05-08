using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroycars : MonoBehaviour
{
    // Start is called before the first frame update



    private void OnTriggerEnter(Collider coll)
    {
        if(coll.GetComponent<Car_Script>())
        {
            Destroy(coll.gameObject);
            return;
        }
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.GetComponent<Car_Script>())
        {
            Destroy(coll.gameObject);
            return;
        }
    }
}
