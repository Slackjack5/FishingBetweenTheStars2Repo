
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class FishWorldObjectContainer : UdonSharpBehaviour
{
    [Header("Required GameObjects")]
    public VRC_Pickup fishPickup;
    [UdonSynced] private bool isActive;
    [UdonSynced] string EXUR_Tag;
    [UdonSynced] int EXUR_LastUsedTime;
    public void EXUR_Reinitialize()
    {
        isActive = true;
        transform.GetChild(0).gameObject.GetComponent<FishWorldObject>().Start();
        transform.GetChild(0).gameObject.SetActive(isActive);
        fishPickup.pickupable = true;
    }
    
    public void EXUR_Finalize()
    {
        fishPickup.pickupable = false;
        isActive = false;
        transform.GetChild(0).gameObject.SetActive(isActive);
    }

    public override void OnDeserialization()
    {
        transform.GetChild(0).gameObject.SetActive(isActive);
    }
}
