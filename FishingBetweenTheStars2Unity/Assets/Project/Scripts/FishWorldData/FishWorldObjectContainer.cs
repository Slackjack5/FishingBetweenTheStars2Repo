
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class FishWorldObjectContainer : UdonSharpBehaviour
{
    [Header("Required GameObjects")]
    public VRC_Pickup fishPickup;
    public InventoryTab inventoryTab;
    [UdonSynced] private bool isActive;
    [UdonSynced] string EXUR_Tag;
    [UdonSynced] int EXUR_LastUsedTime;

    public void EXUR_Reinitialize()
    {
        isActive = true;
        transform.GetChild(0).gameObject.GetComponent<FishWorldObject>().Start();
        transform.GetChild(0).gameObject.SetActive(isActive);
        transform.GetChild(0).GetComponent<FishWorldObject>().SetObjectById(inventoryTab.GetFishIdInWorld());
        transform.GetChild(0).position = inventoryTab.gameObject.transform.position - inventoryTab.gameObject.transform.forward * 0.1f;
        transform.GetChild(0).rotation = inventoryTab.gameObject.transform.rotation;
        fishPickup.pickupable = true;
    }
    
    public void EXUR_Finalize()
    {
        fishPickup.pickupable = false;
        isActive = false;
        transform.GetChild(0).gameObject.SetActive(isActive);
    }

    public void EXUR_RetrievedFromUsing()
    {
        EXUR_Finalize();
    }

    public override void OnDeserialization()
    {
        transform.GetChild(0).gameObject.SetActive(isActive);
    }
}
