
using UdonSharp;
using UnityEngine;
using System.Collections.Generic;
using VRC.SDKBase;
using VRC.Udon;

[DefaultExecutionOrder(1)]
public class LineController : UdonSharpBehaviour
{
    private LineRenderer lineRenderer;
    private Rigidbody hookRigidbody;
    private Vector3 castPosition; // position at which the hook was cast from (to make sure player doesn't run too far away from where they cast)
    private Vector3 hookDistAfterCast; // position at which hook landed after it was cast
    [UdonSynced] private Vector3 hookPos;
    [UdonSynced] private bool showWire;
    [UdonSynced] private bool updateHook;
    private float percentageLeftToReel;
    private bool isCast;
    private bool inWater; // if the hook is in the water
    private FishingGameController fishingGameController;
    private VelocityEstimator velocityEstimator;
    private LeverController leverController; // data from the lever of the rod
    private float maximumReelProgress; // the maximum progress reached on the current fishing game
    [Header("Linked GameObjects")]
    public GameObject hook;
    public GameObject lever;
    public GameObject fishingGame;
    public GameObject container;
    [Header("Line Attributes")]
    [Tooltip("Max distance the player can move the rod from original cast position before the line breaks")]
    public float maxDistanceFromCast;
    [Tooltip("What percentage of reeling to start moving the hook upwards towards the rod")]
    public float percentageToReelUpward;
    [Tooltip("Multiplier on the launch force of the hook when casting the line")]
    public float launchForceMultiplier;
    void Start()
    {
        fishingGameController = fishingGame.GetComponent<FishingGameController>();
        leverController = lever.GetComponent<LeverController>();
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        hookRigidbody = hook.GetComponent<Rigidbody>();
        lineRenderer.enabled = false;
        showWire = false;
        isCast = false;
        inWater = false;
        updateHook = false;
        velocityEstimator = GetComponent<VelocityEstimator>();
        percentageLeftToReel = 1;
    }

    void FixedUpdate()
    {
        if(Networking.GetOwner(gameObject) == Networking.LocalPlayer)
        {
            if(Vector3.Distance(transform.position, castPosition) > maxDistanceFromCast)
            {
                ResetLine();
            }
            else if(inWater)
            {   
                ReelLine();
            }
            hookPos = hook.transform.position;
            updateHook = !updateHook;
        }
    }

    public void CastLine()
    {
        castPosition = transform.position;
        hookRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        hookRigidbody.AddForce(velocityEstimator.GetPredictedVelocity() * launchForceMultiplier);
        hook.transform.SetParent(null); // we don't want the hook to move with respect to the rod anymore
        fishingGameController.Start();
        lineRenderer.enabled = true;
        showWire = true;
        isCast = true;
    }

    public void ReelLine()
    {
        if(maximumReelProgress < fishingGameController.GetPercentageCaught())
        {
            float previousReelProgress = maximumReelProgress;
            maximumReelProgress = fishingGameController.GetPercentageCaught();
            float percentageToReel = maximumReelProgress - previousReelProgress;
            percentageLeftToReel -= percentageToReel;
            Vector3 hookDistXZ = new Vector3(hookDistAfterCast.x, 0, hookDistAfterCast.z);
            Vector3 hookDistY = new Vector3(0, hookDistAfterCast.y, 0);
            if(percentageLeftToReel < percentageToReelUpward) // if less than 10% of the time is remaining to reel, start reeling vertically
            {
                hook.transform.SetPositionAndRotation(hook.transform.position - hookDistY*percentageToReel*(1/percentageToReelUpward) - hookDistXZ*percentageToReel, hook.transform.rotation);
            }
            else
            {
                hook.transform.SetPositionAndRotation(hook.transform.position - hookDistXZ*percentageToReel, hook.transform.rotation);
            }
        }
    }

    public void ResetLine()
    {
        hookRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        lineRenderer.enabled = false;
        hook.transform.SetParent(transform);
        hook.transform.SetPositionAndRotation(transform.position, transform.rotation);
        maximumReelProgress = 0;
        percentageLeftToReel = 1;
        showWire = false;
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
        hook.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        hookDistAfterCast = hook.transform.position - transform.position;
        inWater = water;
    }

    public override void OnDeserialization()
    {
        if(Networking.GetOwner(gameObject) != Networking.LocalPlayer)
        {
            hook.transform.position = hookPos;
            lineRenderer.enabled = showWire; 
        }
    }
}
