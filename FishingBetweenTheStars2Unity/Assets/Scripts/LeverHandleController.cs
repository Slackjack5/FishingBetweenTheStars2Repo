
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class LeverHandleController : UdonSharpBehaviour
{
    private LeverController controller;
    void Start()
    {
        controller = GetComponentInParent<LeverController>();
    }
    public override void OnPickup()
    {
        controller.SetHeld(true);
    }
    public override void OnDrop()
    {
        controller.SetHeld(false);
        transform.localPosition = new Vector3(0, 0, 0);
    }
}
