
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class FIshingPDA : UdonSharpBehaviour
{
  private VRCPlayerApi localPlayer;
  public GameObject headOrb;
  public GameObject handCollider;
  void Start()
    {
    localPlayer = Networking.LocalPlayer;
    }

  private void LateUpdate()
  {
    //Update our Head Position
    Vector3 headPosition = localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;
    Quaternion headRotation = localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).rotation;
    Vector3 handPosition = localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.RightHand).position;
    Quaternion handRotation = localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.RightHand).rotation;
    headOrb.transform.position = headPosition;
    headOrb.transform.rotation = headRotation;
    handCollider.transform.position = handPosition;
    handCollider.transform.rotation = handRotation;
  }


}
