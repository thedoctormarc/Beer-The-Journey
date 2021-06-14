using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaxiCrushed : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject part;
    [SerializeField]
    GameObject driver;

    [SerializeField]
    Transform origin;
    [SerializeField]
    Transform end;
    void Start()
    {
       part = GetComponentInChildren<ParticleSystem>().gameObject;
       part.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CrushFloor()
    {
        part.SetActive(true);
        Instantiate(driver, transform);
        Transform[] transforms = driver.GetComponentsInChildren<Transform>();
        foreach(Transform t in transforms)
        {
            if (t.name == "Root")
            {
                Vector3 dir = end.position - origin.position;
                dir = dir.normalized;
                t.GetComponentInChildren<Rigidbody>().AddForce(dir * 50, ForceMode.Impulse);
            }
            else break;
        }
    }

}
