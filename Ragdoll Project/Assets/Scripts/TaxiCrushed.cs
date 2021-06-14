using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaxiCrushed : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 targetposition = Vector3.zero;

    Transform[] llista;
    float speed = -5f;
    void Start()
    {
       llista = gameObject.GetComponentInParent<Transform>().GetComponentsInChildren<Transform>();

        for(int i =0; i<llista.Length; i++)
        {
            if(llista[i].gameObject.name == "endpos")
            {
                targetposition = llista[i].position;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetposition,step);
        
    }
}
