
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[DefaultExecutionOrder(0)]
public class LeverController : UdonSharpBehaviour
{
    private const float DEFAULT_TIME_STEP = 0.02f;
    private float timeStepRatio;
    private Rigidbody lever;
    private HingeJoint leverHinge;
    private float angularVelocity;
    private float previousAngle;
    private float initialDrag;
    private bool isHeld;
    private bool isReeling;
    [Header("External GameObjects")]
    public GameObject handleController;
    public GameObject leverHandle;
    public GameObject leverReference;
    public GameObject rodController;
    [Header("Lever variables")]
    public float reelingThreshold; // speed threshold to determine if they are reeling or not
    void Start() 
    {
        timeStepRatio = Time.fixedDeltaTime/DEFAULT_TIME_STEP;
        lever = GetComponent<Rigidbody>();
        leverHinge = GetComponent<HingeJoint>();
        initialDrag = lever.angularDrag;
        isReeling = false;
        previousAngle = 0;
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
        }
        angularVelocity = (leverHinge.angle - previousAngle) * timeStepRatio;
        previousAngle = leverHinge.angle;
        if(angularVelocity > reelingThreshold) 
        {
            isReeling = true;
        }
        else
        {
            isReeling = false;
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
}
