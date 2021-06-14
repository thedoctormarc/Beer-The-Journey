using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class kick : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<kid_movement>() != null)
        {
            Debug.Log("kid touched");
            Vector3 dir = transform.position - collision.gameObject.transform.position;

            dir = -dir.normalized;
            dir.y = 1;

            collision.gameObject.GetComponent<NavMeshAgent>().enabled = false;
            collision.GetComponent<kid_movement>().SelfDestroy();

            collision.gameObject.GetComponent<Rigidbody>().AddForce(dir * 10, ForceMode.Impulse);
        }
    }

}
