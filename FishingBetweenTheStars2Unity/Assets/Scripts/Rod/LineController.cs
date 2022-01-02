
using UdonSharp;
using UnityEngine;
using System.Collections.Generic;
using VRC.SDKBase;
using VRC.Udon;

public class LineController : UdonSharpBehaviour
{
    private LineRenderer lineRenderer;
    private Rigidbody hookRigidbody;
    private Vector3 castPosition; // position at which the hook was cast from (to make sure player doesn't run too far away from where they cast)
    private GameObject hookParent;
    private bool isCast;
    private bool inWater; // if the hook is in the water
    private float reelTime; // remaining time left to reel in
    private float totalReelTime; // total time to reel once the rod has been cast
    private VelocityEstimator velocityEstimator;
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
    [Tooltip("Multiplier on the launch force of the hook when casting the line")]
    public float launchForceMultiplier;
    void Start()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        hookRigidbody = hook.GetComponent<Rigidbody>();
        leverController = gameObject.transform.parent.parent.GetChild(2).GetComponent<LeverController>();
        lineRenderer.forceRenderingOff = true;
        isCast = false;
        inWater = false;
        reelTime = 0;
        velocityEstimator = GetComponent<VelocityEstimator>();
    }

    void FixedUpdate()
    {
        if(Vector3.Distance(transform.position, castPosition) > maxDistanceFromCast)
        {
            ResetLine();
        }
        else if(inWater && leverController.GetReeling())
        {
            //ReelLine();
        }
    }

    public void CastLine()
    {
        castPosition = transform.position;
        hookRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        hookRigidbody.AddForce(velocityEstimator.GetPredictedVelocity() * launchForceMultiplier);
        hookParent = hook.transform.parent.gameObject; // store the parent for later when we want to reattach the hook
        hook.transform.SetParent(null); // we don't want the hook to move with respect to the rod anymore
        lineRenderer.forceRenderingOff = false;
        isCast = true;
    }

    public void ReelLine()
    {
        // add in some multiplier to reelPerTick that gets data from the fishing minigame
        float reelPerTick = Time.fixedDeltaTime;
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
    
    public bool GetWater()
    {
        return inWater;
    }

    public bool GetReeling()
    {
        return leverController.GetReeling();
    }

    public void SetInWater(bool water)
    {
        reelTime = maxTimeToReelPerUnit * Vector3.Distance(transform.position, hook.transform.position);
        totalReelTime = reelTime;
        inWater = water;
    }
}
