
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class RodContainerController : UdonSharpBehaviour
{
    private LineController lineController;
    private FishingGameController fishingGameController;
    [UdonSynced] private bool isActive;
    void Start()
    {
        lineController = GetComponentInChildren<LineController>();
        fishingGameController = GetComponentInChildren<FishingGameController>();
        isActive = false;
    }

    public void ResetRod()
    {
        lineController.ResetLine();
        fishingGameController.Start();
        SetRodActive(false);
    }

    public void SetRodActive(bool active)
    {
        isActive = active;
        for(int i = 0; i < gameObject.transform.childCount; i++)
        {
            gameObject.transform.GetChild(i).gameObject.SetActive(active);
        }
    }

    public override void OnDeserialization()
    {
        for(int i = 0; i < gameObject.transform.childCount; i++)
        {
            gameObject.transform.GetChild(i).gameObject.SetActive(isActive);
        }
    }

    public bool isRodActive()
    {
        return isActive;
    }
}
