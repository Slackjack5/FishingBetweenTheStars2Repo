
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;

public class PlayerController : UdonSharpBehaviour
{
  public int playerCash;
  public GameObject GoldCounter;

  private void FixedUpdate()
  {
    GoldCounter.GetComponent<TMPro.TextMeshProUGUI>().text = "Gold: " + playerCash;
  }

}
