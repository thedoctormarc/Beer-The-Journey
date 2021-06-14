using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.GetComponentInParent<CharacterLogic>() == null)
        {
            Vector3 dir = transform.position - collision.gameObject.transform.position;

            dir = -dir.normalized;
            collision.gameObject.GetComponent<Rigidbody>().AddForce(dir * 200, ForceMode.Impulse);
        }
    }
}
