
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class FIshingPDA : UdonSharpBehaviour
{
  private VRCPlayerApi localPlayer;
  public GameObject headOrb;
  public GameObject rightHandCollider;
  public GameObject leftHandCollider;

  public int currentTab;
  public int maxTab;
  void Start()
    {
    localPlayer = Networking.LocalPlayer;
    }

  private void LateUpdate()
  {
    //Update our Head Position
    Vector3 headPosition = localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;
    Quaternion headRotation = localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).rotation;
    Vector3 rightHandPosition = localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.RightHand).position;
    Quaternion rightHandRotation = localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.RightHand).rotation;
    Vector3 leftHandPosition = localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.LeftHand).position;
    Quaternion leftHandRotation = localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.LeftHand).rotation;
    headOrb.transform.position = headPosition;
    headOrb.transform.rotation = headRotation;
    rightHandCollider.transform.position = rightHandPosition;
    rightHandCollider.transform.rotation = rightHandRotation;
    leftHandCollider.transform.position = leftHandPosition;
    leftHandCollider.transform.rotation = leftHandRotation;
    
  }

  private void FixedUpdate()
  {
    
  }

  public void NextTab()
  {
    if (currentTab < maxTab)
    {
      currentTab += 1;
      UpdateTab();
    }
    else
    {
      currentTab = 0;
      UpdateTab();
    }
  }

  public void UpdateTab()
  {
    Debug.Log("Updating Tab");
    if(currentTab==0)
    {
      GameObject Child = gameObject.transform.GetChild(0).gameObject;
      Child.SetActive(true);
      //Set other children as inactive
      Child = gameObject.transform.GetChild(1).gameObject;
      Child.SetActive(false);
    }
    else if(currentTab == 1)
    {
      GameObject Child = gameObject.transform.GetChild(1).gameObject;
      Child.SetActive(true);
      //Set other children as inactive
      Child = gameObject.transform.GetChild(0).gameObject;
      Child.SetActive(false);
    }
  }

}
