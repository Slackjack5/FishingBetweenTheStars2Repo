
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class RodContainerController : UdonSharpBehaviour
{
    [Header("Required GameObjects")]
    public VRC_Pickup leverHandlePickup;
    public VRC_Pickup rodPickup;
    public LineController lineController;
    public FishingGameController fishingGameController;
    public RodSpawnerController rodSpawnerController;
    [UdonSynced] private bool isActive;
    [UdonSynced] string EXUR_Tag;
    [UdonSynced] int EXUR_LastUsedTime;
    void Start()
    {
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

    public void EXUR_Reinitialize()
    {
        SetRodActive(true);
        leverHandlePickup.pickupable = true;
        rodPickup.pickupable = true;
        transform.GetChild(0).position = rodSpawnerController.gameObject.transform.position;//Networking.LocalPlayer.GetBonePosition(HumanBodyBones.Hips);
        transform.GetChild(0).rotation = rodSpawnerController.gameObject.transform.rotation; //Networking.LocalPlayer.GetBoneRotation(HumanBodyBones.Hips);
    }
    public void EXUR_Finalize()
    {
        leverHandlePickup.pickupable = false;
        rodPickup.pickupable = false;
        ResetRod();
    }

    public void EXUR_RetrievedFromUsing()
    {
        EXUR_Finalize();
    }
}
