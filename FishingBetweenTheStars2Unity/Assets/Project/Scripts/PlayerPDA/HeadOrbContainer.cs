
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class HeadOrbContainer : UdonSharpBehaviour
{
  private void OnTriggerEnter(Collider other)
  {
    //When our hand enters the trigger, turn on our Grabbable HeadOrb
    transform.GetChild(0).gameObject.SetActive(true);
  }

  private void OnTriggerExit(Collider other)
  {
    //When our hand exits the trigger, turn off our Grabbable HeadOrb
    transform.GetChild(0).gameObject.SetActive(false);
  }

}
