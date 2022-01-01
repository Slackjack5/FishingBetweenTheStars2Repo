
using UdonSharp;
using UnityEngine;
using System.Collections.Generic;
using VRC.SDKBase;
using VRC.Udon;

public class LineController : UdonSharpBehaviour
{
    private LineRenderer lineRenderer;
    private Rigidbody hookRigidbody;
    private Vector3 predictedVelocity;
    private Vector3 prevPosition; // previous position the hook was at to do casting velocity calculations
    private Vector3 castPosition; // position at which the hook was cast from (to make sure player doesn't run too far away from where they cast)
    private Vector3[] prevVelocities;
    private GameObject hookParent;
    private int prevVelCount;
    private bool isCast;
    private bool inWater; // if the hook is in the water
    private float reelTime; // remaining time left to reel in
    private float totalReelTime; // total time to reel once the rod has been cast
    private LeverController leverController; // data from the lever of the rod
    [Header("Linked GameObjects")]
    public GameObject hook;
    [Header("Line Attributes")]
    [Tooltip("Max distance the player can move the rod from original cast position before the line breaks")]
    public float maxDistanceFromCast;
    [Tooltip("Time it takes for the hook to reel one unit in once it is fully cast when reeling at minimum speed")]
    public float maxTimeToReelPerUnit;
    [Tooltip("What percentage of reeling to start moving the hook upwards towards the rod")]
    public float percentageToReelUpward;
    void Start()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        hookRigidbody = hook.GetComponent<Rigidbody>();
        leverController = gameObject.transform.parent.parent.GetChild(2).GetComponent<LeverController>();
        lineRenderer.forceRenderingOff = true;
        prevVelocities = new Vector3[10];
        prevPosition = transform.position;
        prevVelCount = 0;
        isCast = false;
        inWater = false;
        reelTime = 0;
    }

    void FixedUpdate()
    {
        if(!isCast) {
            if(prevVelCount < 9) 
            {
                prevVelocities[prevVelCount] = (transform.position - prevPosition);
                prevVelCount++;
            }
            else
            {
                Vector3 temp;
                temp = prevVelocities[prevVelocities.Length - 2];
                prevVelocities[prevVelocities.Length - 2] = prevVelocities[prevVelocities.Length - 1];
                for(int i = prevVelocities.Length - 2; i >= 1; i--) 
                {
                    Vector3 temp2 = prevVelocities[i - 1];
                    prevVelocities[i - 1] = temp;
                    temp = temp2;
                }
                prevVelocities[prevVelocities.Length - 1] = transform.position - prevPosition;
            }
            Vector3 sum = new Vector3();
            for(int i = 0; i < prevVelCount; i++)
            {
                sum += prevVelocities[i];
            }
            predictedVelocity = sum / (float)prevVelCount;
            prevPosition = transform.position;
        }
        else if(Vector3.Distance(transform.position, castPosition) > maxDistanceFromCast)
        {
            ResetLine();
        }
        else if(inWater && leverController.GetReeling())
        {
            ReelLine();
        }
    }

    public void CastLine()
    {
        castPosition = transform.position;
        hookRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        hookRigidbody.AddForce(predictedVelocity);
        hookParent = hook.transform.parent.gameObject; // store the parent for later when we want to reattach the hook
        hook.transform.SetParent(null); // we don't want the hook to move with respect to the rod anymore
        lineRenderer.forceRenderingOff = false;
        isCast = true;
    }

    public void ReelLine()
    {
        float reelPerTick = leverController.GetReelForce()*Time.fixedDeltaTime;
        float percentageToReel = reelPerTick / reelTime;
        reelTime -= reelPerTick;
        Vector3 distToRod = hook.transform.position - transform.position;
        Vector3 xzDistToRod = new Vector3(distToRod.x, 0, distToRod.z); // only get xz distance since we don't want the hook to move vertically as it is reeled in until the end
        if(reelTime / totalReelTime < percentageToReelUpward) // if less than 10% of the time is remaining to reel, start reeling vertically
        {
            hook.transform.SetPositionAndRotation(hook.transform.position - distToRod*percentageToReel, hook.transform.rotation);
        }
        else
        {
            hook.transform.SetPositionAndRotation(hook.transform.position - xzDistToRod*percentageToReel, hook.transform.rotation);
        }
        if(reelTime < 0) 
        {
            ResetLine();
        }
    }

    public void ResetLine()
    {
        prevVelCount = 0;
        hookRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        lineRenderer.forceRenderingOff = true;
        hook.transform.SetParent(hookParent.transform);
        hook.transform.SetPositionAndRotation(transform.position, transform.rotation);
        isCast = false;
        inWater = false;
    }

    public void SetCast(bool cast)
    {
        isCast = cast;
    }

    public bool GetCast()
    {
        return isCast;
    }

    public void SetInWater(bool water)
    {
        reelTime = maxTimeToReelPerUnit * Vector3.Distance(transform.position, hook.transform.position);
        totalReelTime = reelTime;
        Debug.Log(Vector3.Distance(transform.position, hook.transform.position));
        inWater = water;
    }
}
