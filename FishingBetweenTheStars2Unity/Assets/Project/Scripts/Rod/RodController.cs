
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class RodController : UdonSharpBehaviour
{
    private LineController lineController;    
    void Start()
    {
        lineController = GetComponentInChildren<LineController>();
    }

    public override void OnPickupUseUp()
    {
        if(!lineController.GetCast()) {
            lineController.CastLine();
        }
    }
}
