using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaxiCrushed : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject part;
    [SerializeField]
    GameObject driver;
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

    }
}
