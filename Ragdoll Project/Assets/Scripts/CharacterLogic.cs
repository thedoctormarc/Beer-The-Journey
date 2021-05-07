using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CHAR_STATES
{
    IDLE,
    WALKING_FORWARD,
    LEFT,
    RIGHT,
    DOING_ACTION
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
    [SerializeField]
    GameObject player;

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

                Vector3 localForward = player.transform.worldToLocalMatrix.MultiplyVector(transform.forward);
                Vector3 direction = localForward.normalized;
                float targetAngle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
                hipJoint.targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
                hipRigidBody.AddForce(direction * speed);
                animator.SetBool("walk", true);

                break;

            case CHAR_STATES.LEFT:

                //Vector3 rotate = new Vector3(0,-90,0);

                //transform.localRotation = Quaternion.Euler(0, -90, 0);

                StartCoroutine(TurnLeft());
                curr_state = CHAR_STATES.DOING_ACTION;
 
                break;

            case CHAR_STATES.RIGHT:
                //Vector3 rotate = new Vector3(0, 90, 0);
                //animator.SetBool("right", true);
                //player.transform.localRotation = Quaternion.Euler(0, 90, 0);
                //hipJoint.targetRotation = Quaternion.Euler(0f, 90, 0f);
                //hipJoint.breakTorque = 100f;

                //hipRigidBody.AddTorque(0f, 90, 0f,ForceMode.Force);

                StartCoroutine(TurnRight());
                curr_state = CHAR_STATES.DOING_ACTION;
                break;

            case CHAR_STATES.DOING_ACTION:
                

                break;
        }

    }

    IEnumerator TurnRight()
    {
        animator.SetBool("right", true);
        float tmp = player.transform.eulerAngles.y;
        float rotation = tmp + 90;
        while(tmp <= rotation)
        {
            tmp += 0.25f;
            //hipJoint.targetRotation = Quaternion.Euler(0f, tmp, 0f);
            player.transform.rotation = Quaternion.Euler(0f, tmp, 0f);
            yield return null;
        }
        animator.SetBool("right", false);
        curr_state = CHAR_STATES.IDLE;
    }

    IEnumerator TurnLeft()
    {
       // animator.SetBool("right", true);
        float tmp = player.transform.eulerAngles.y;
        float rotation = tmp - 90;
        while (tmp >= rotation)
        {
            tmp -= 0.25f;
            //hipJoint.targetRotation = Quaternion.Euler(0f, tmp, 0f);
            player.transform.rotation = Quaternion.Euler(0f, tmp, 0f);
            yield return null;
        }
        //animator.SetBool("right", false);
        curr_state = CHAR_STATES.IDLE;
    }


    public CHAR_STATES GetState() => curr_state;
}