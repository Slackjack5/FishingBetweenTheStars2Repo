
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class SellingStation : UdonSharpBehaviour
{
    void Start()
    {
        
    }

  private void OnTriggerEnter(Collider collider)
  {
    if (collider.gameObject.name == "FishWorldObject" && Networking.GetOwner(collider.gameObject).playerId == Networking.LocalPlayer.playerId)
    {
      collider.GetComponent<FishWorldObject>().UsedAsCash();
    }
  }
}
