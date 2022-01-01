
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class RodController : UdonSharpBehaviour
{
    public GameObject line;
    private LineController lineController;
    void Start()
    {
        lineController = line.GetComponent<LineController>();
    }

    public override void OnPickupUseUp()
    {
        if(!lineController.GetCast()) {
            lineController.CastLine();
        }
    }
}
