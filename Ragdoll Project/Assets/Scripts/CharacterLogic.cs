using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CHAR_STATES
{
    IDLE,
    WALKING_FORWARD,
    STOP
}
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

    CHAR_STATES curr_state;
    // Start is called before the first frame update
    void Start()
    {
        curr_state = CHAR_STATES.IDLE;
    }
    public void SetCharacterState(CHAR_STATES state)
    {
        curr_state = state;
    }

    // Update is called once per frame
    void Update()
    {

        switch(curr_state)
        {
            case CHAR_STATES.IDLE:

                animator.SetBool("walk", false);
                break;
            case CHAR_STATES.WALKING_FORWARD:

                Vector3 localForward = transform.worldToLocalMatrix.MultiplyVector(transform.forward);
                Vector3 direction = localForward.normalized;
                float targetAngle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
                hipJoint.targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
                hipRigidBody.AddForce(direction * speed);
                animator.SetBool("walk", true);

                break;

            case CHAR_STATES.STOP:
                break;
        }
        //float x = Input.GetAxisRaw("Horizontal");
        //float y = Input.GetAxisRaw("Vertical");

        //Debug.Log("X: " + x + "   Y: " +  y);

        //Vector3 dir = new Vector3(x, 0f, y).normalized;

        //if (dir.magnitude >= 0.1f)
        //{
        //    float targetAngle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        //    hipJoint.targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
        //    hipRigidBody.AddForce(dir * speed);
        //    walk = true;
        //}
        //else
        //{
        //    walk = false;
        //}

        //animator.SetBool("walk", walk);
    }
}