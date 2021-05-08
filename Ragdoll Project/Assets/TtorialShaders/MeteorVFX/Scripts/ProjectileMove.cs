using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMove : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    public GameObject impactprefab;
    private Rigidbody rb;
    public List<GameObject> trails;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(speed != 0 && rb != null)
        {
            rb.position += transform.forward * (speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        speed = 0;
        ContactPoint contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point;
        if(impactprefab != null)
        {
            GameObject impactVFX = Instantiate(impactprefab, pos, rot) as GameObject;
        }


        if(trails.Count >0)
        {
            for(int i = 0; i < trails.Count; ++i)
            {
                trails[i].transform.parent = null;
                ParticleSystem ps = trails[i].GetComponent<ParticleSystem>();
                if(ps != null)
                {
                    ps.Stop();
                    Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
                }
            }
        }
        Destroy(gameObject);

    }
}
