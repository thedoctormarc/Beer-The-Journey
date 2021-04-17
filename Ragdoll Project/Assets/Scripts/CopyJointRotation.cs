using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyJointRotation : MonoBehaviour
{
    [SerializeField]  
    Transform targetLimb;
    ConfigurableJoint joint;
    Quaternion intiialRot;

    // Start is called before the first frame update
    void Start()
    {
        intiialRot = targetLimb.transform.localRotation;
        joint = GetComponent<ConfigurableJoint>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        joint.targetRotation = copyRotation();
    }

    private Quaternion copyRotation()
    {
        return Quaternion.Inverse(targetLimb.localRotation) * intiialRot;
    }
}
