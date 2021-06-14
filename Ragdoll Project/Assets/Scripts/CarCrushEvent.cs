using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCrushEvent : MonoBehaviour
{
    bool triggered = false;
    // Start is called before the first frame update
    [SerializeField]
    GameObject car;

    [SerializeField]
    GameObject startpoint;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<CopyJointRotation>() !=null && !triggered)
        {
            triggered = true;
            Debug.Log("triggered");

            Instantiate(car,startpoint.transform.position, startpoint.transform.rotation);
        }
        if(other.gameObject.GetComponent<TaxiCrushed>() != null)
        {
            other.gameObject.GetComponent<TaxiCrushed>().CrushFloor();

            CameraShake.Instance.ShakeCamera(10, 0.2f);
        }
    }
}
