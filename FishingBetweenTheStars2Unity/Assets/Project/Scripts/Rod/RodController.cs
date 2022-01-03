
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class RodController : UdonSharpBehaviour
{
    private LineController lineController;
    private FishingGameController fishingGameController;
    [UdonSynced] private bool isActive;
    void Start()
    {
        lineController = GetComponentInChildren<LineController>();
        fishingGameController = GetComponentInChildren<FishingGameController>();
    }

    public void Reset()
    {
        SetRodActive(false);
        lineController.ResetLine();
        fishingGameController.Start();
    }

    public void SetRodActive(bool active)
    {
        isActive = active;
        gameObject.SetActive(isActive);
    }

    public override void OnPickupUseUp()
    {
        if(!lineController.GetCast()) {
            lineController.CastLine();
        }
    }

    public override void OnDeserialization()
    {
        gameObject.SetActive(isActive);
    }
}
