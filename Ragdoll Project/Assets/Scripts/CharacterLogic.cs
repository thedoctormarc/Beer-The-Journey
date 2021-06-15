using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CHAR_STATES
{
    IDLE,
    WALKING_FORWARD,
    LEFT,
    RIGHT,
    DOING_ACTION,
    KICK
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
    [SerializeField]
    GameObject kickCollider;
    bool walk = false;

    [SerializeField]
    ParticleSystem kick_smoke;
    CHAR_STATES curr_state;

    int life = 100;
    public RectTransform lifeBar;
    int invulnerabilityTime = 1;
    float invulnerabilityTimer = 0.0f;
    bool ReadyToGetDamage = true;
    public int damage = 25;

    public RectTransform GameOver;
    bool Dead = false;

    Vector3 initPos;




    // Start is called before the first frame update
    void Start()
    {
        curr_state = CHAR_STATES.IDLE;
        kickCollider.SetActive(false);
        initPos = new Vector3(6.53f, 1.77f, -92.03f);


    }
    public void SetCharacterState(CHAR_STATES state)
    {
        if (!Dead)
            curr_state = state;
    }

    // Update is called once per frame
    void Update()
    {

        switch (curr_state)
        {
            case CHAR_STATES.IDLE:

                animator.SetBool("walk", false);
                break;
            case CHAR_STATES.WALKING_FORWARD:

                Vector3 localForward = player.transform.worldToLocalMatrix.MultiplyVector(player.transform.forward);
                Vector3 direction = localForward.normalized;
                Debug.Log("direction" + direction);
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
            case CHAR_STATES.KICK:
                // animator.SetBool("kick", false);
                StartCoroutine(Kicking());
                curr_state = CHAR_STATES.DOING_ACTION;

                break;
        }

        CheckInvulerability();
        if(Dead)
            Death();

    }

    private void CheckInvulerability()
    {
        if (!ReadyToGetDamage)
        {
            invulnerabilityTimer += Time.deltaTime;
            if (invulnerabilityTimer >= invulnerabilityTime)
                ReadyToGetDamage = true;
        }
    }

    IEnumerator Kicking()
    {
        kickCollider.SetActive(true);
        animator.SetBool("kick", true);
        yield return new WaitForSeconds(0.5f);
        kick_smoke.gameObject.SetActive(true);
        kickCollider.SetActive(true);
        yield return new WaitForSeconds(1.0f);


        animator.SetBool("kick", false);
        kickCollider.SetActive(false);
        kick_smoke.gameObject.SetActive(false);

        curr_state = CHAR_STATES.IDLE;
    }
    IEnumerator TurnRight()
    {
        animator.SetBool("walk", false);
        // animator.SetBool("right", true);
        float tmp = player.transform.rotation.eulerAngles.y;
        float rotation = tmp + 90;
        while(tmp <= rotation)
        {
            tmp += 0.5f;
            //hipJoint.targetRotation = Quaternion.Euler(0f, tmp, 0f);
            //player.transform.localRotation = Quaternion.Euler(0, tmp, 0);
            player.transform.rotation = Quaternion.Euler(0f, tmp, 0f);
            yield return null;
        }
       // animator.SetBool("right", false);
        curr_state = CHAR_STATES.IDLE;
    }

    IEnumerator TurnLeft()
    {
        animator.SetBool("walk", false);
        // animator.SetBool("right", true);
        float tmp = player.transform.rotation.eulerAngles.y;
        float rotation = tmp - 90;
        while (tmp >= rotation)
        {
            tmp -= 0.5f;
            //hipJoint.targetRotation = Quaternion.Euler(0f, tmp, 0f);
            player.transform.rotation = Quaternion.Euler(0f, tmp, 0f);
            yield return null;
        }
        //animator.SetBool("right", false);
        curr_state = CHAR_STATES.IDLE;
    }

    public void GetDamage()
    {
        if(life > 0 && ReadyToGetDamage)
        {
            life -= damage;
            lifeBar.sizeDelta = new Vector2(life, lifeBar.sizeDelta.y);
            ReadyToGetDamage = false;
            if (life <= 0)
            {
                GameOver.gameObject.SetActive(true);
                SetCharacterState(CHAR_STATES.IDLE);
                Dead = true;
            }

        }
    }
    private void Death()
    {
        
        if (GameOver.sizeDelta.x < 100) 
            GameOver.sizeDelta = new Vector2(GameOver.sizeDelta.x + 2, GameOver.sizeDelta.y + 2);
    }

    public void Kill()
    {
        Dead = true;
    }

    public void Respawn()
    {
        if (Dead)
        {
            transform.position = initPos;

            life = 100;

            lifeBar.sizeDelta = new Vector2(life, life );
            GameOver.sizeDelta = new Vector2(0, 0);
            GameOver.gameObject.SetActive(false);
            Dead = false;
        }

    }
    public bool GetDead()
    {
        return Dead;
    }


    public CHAR_STATES GetState() => curr_state;
}