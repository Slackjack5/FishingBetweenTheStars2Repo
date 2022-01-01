
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class LeverController : UdonSharpBehaviour
{
    private GameObject handleController;
    private GameObject leverHandle;
    private GameObject leverReference;
    private Rigidbody lever;
    private HingeJoint leverHinge;
    private float initialDrag;
    private bool isHeld;
    private bool isReeling;
    public float reelingThreshold; // speed threshold to determine if they are reeling or not
    void Start() 
    {
        leverReference = gameObject.transform.parent.GetChild(1).gameObject;
        lever = gameObject.GetComponent<Rigidbody>();
        leverHinge = gameObject.GetComponent<HingeJoint>();
        initialDrag = lever.angularDrag;
        leverHandle = gameObject.transform.GetChild(0).gameObject;
        handleController = gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject; // get the second child
        isReeling = false;
    }

    void FixedUpdate()
    {
        if(isHeld)
        {
            Vector3 transformedDist = leverReference.transform.InverseTransformPoint(Vector3.ProjectOnPlane(handleController.transform.position, leverReference.transform.up));
            float rad = Mathf.Atan2(transformedDist.z, -transformedDist.x);
            float angle = Mathf.Rad2Deg * rad;
            JointSpring spring = leverHinge.spring;
            spring.targetPosition = angle;
            leverHinge.spring = spring;
            if(leverHinge.currentTorque.magnitude > reelingThreshold) 
            {
                isReeling = true;
            }
            else
            {
                isReeling = false;
            }
        }
    }

    void OnDrawGizmos()
    {
        if(leverHandle != null) {
            Gizmos.DrawSphere(leverHandle.transform.position, 1f);
            Gizmos.DrawSphere(handleController.transform.position, 1f);
        }
    }

    public void SetHeld(bool held)
    {
        if(held)
        {
            leverHinge.useSpring = true;
        }
        else
        {
            leverHinge.useSpring = false;
        }
        isHeld = held;
    }

    public bool GetReeling()
    {
        return isReeling;
    }

    public float GetReelForce()
    {
        return leverHinge.currentTorque.magnitude/reelingThreshold;
    }
}
