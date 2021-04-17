using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLogic : MonoBehaviour
{
    [SerializeField]
    float speed = 15f;
    [SerializeField]  
    ConfigurableJoint hipJoint;
    [SerializeField]  
    Rigidbody hipRigidBody;
    [SerializeField]  
    Animator animator;

    bool walk = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(x, 0f, y).normalized;

        if (dir.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
            hipJoint.targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            hipRigidBody.AddForce(dir * speed);
            walk = true;
        }
        else
        {
            walk = false;
        }

        animator.SetBool("walk", walk);
    }
}