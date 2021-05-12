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


    private void OnCollisionEnter(Collision collision)
    {

        if(collision.gameObject.GetComponentInParent<CharacterLogic>() != null)
        {
            Vector3 dir = transform.position - collision.gameObject.transform.position;

            dir = -dir.normalized;
            collision.gameObject.GetComponent<Rigidbody>().AddForce(dir*200, ForceMode.Impulse);
        }
    }
}
