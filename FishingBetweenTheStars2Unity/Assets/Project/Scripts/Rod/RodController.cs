
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class RodController : UdonSharpBehaviour
{
    private LineController lineController;
    [Header("Required VRC components")]
    public VRC_Pickup leverHandlePickup; // needed to make handle pickuppable locally for the person who claimed the rod
    public VRC_Pickup rodPickup;
    [UdonSynced] private bool isInUse;
    private VRCPlayerApi currentPlayer;
    void Start()
    {
        lineController = GetComponentInChildren<LineController>();
    }

    void FixedUpdate()
    {
        if(currentPlayer != null && currentPlayer.IsValid())
        {
            isInUse = false;
        }
    }

    public override void OnPickup()
    {
        leverHandlePickup.pickupable = true;
    }
    
    public override void OnPickupUseUp()
    {
        if(!lineController.GetCast()) {
            lineController.CastLine();
        }
    }

    public override void OnDeserialization()
    {
        rodPickup.pickupable = !isInUse;
    }

    public override bool OnOwnershipRequest(VRCPlayerApi requester, VRCPlayerApi newOwner)
    {
        currentPlayer = newOwner;
        return true;
    }
}
