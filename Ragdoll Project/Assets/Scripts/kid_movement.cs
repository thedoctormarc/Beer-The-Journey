using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
public class kid_movement : MonoBehaviour
{
    // Start is called before the first frame update
    NavMeshAgent agent;

    [SerializeField]
    GameObject target;

    [SerializeField]
    float detection_rad;

    Animator animator;
    Vector3 dir;
    bool detected = false;

    [SerializeField]
    GameObject kick_smoke;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        dir = new Vector3(detection_rad, 0, 0);
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {

        if (agent.enabled)
        {
            if (Vector3.Distance(target.transform.position, transform.position) <= detection_rad)
            {

                agent.destination = target.transform.position;
                if (!detected)
                {
                    Debug.Log("Detected");
                    animator.SetBool("run", detected = true);
                }

            }

            else
            {
                //Idle Mode
                agent.isStopped = true;
                if (detected)
                {
                    Debug.Log("Out of detection range");
                    animator.SetBool("run", detected = false);
                }
            }
        }
        Debug.DrawLine(transform.position, new Vector3(transform.position.x+detection_rad,transform.position.y,transform.position.z + detection_rad), Color.blue);

        
    }

    public void SelfDestroy()
    {
        kick_smoke.SetActive(true);
        kick_smoke.GetComponent<ParticleSystem>().Play();
        Destroy(gameObject,2f);
    }
    


}
