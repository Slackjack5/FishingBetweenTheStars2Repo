
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class HeadOrb : UdonSharpBehaviour
{
  public GameObject PlayerPDA;
  private VRCPlayerApi localPlayer;
  public GameObject ResetPoint;
  private Vector3 originalPosition;

  private void Start()
  {
    //Set our Orb to Behind our Head
    transform.position = ResetPoint.transform.position;
    transform.rotation = ResetPoint.transform.rotation;
  }


  public virtual void OnDrop() 
  {
    //When we let go reset the orb and teleport our UI
    localPlayer = Networking.LocalPlayer;
    PlayerPDA.transform.position = gameObject.transform.position;
    PlayerPDA.transform.rotation = localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).rotation;
    transform.position = ResetPoint.transform.position;
    transform.rotation = ResetPoint.transform.rotation;
  }



}
