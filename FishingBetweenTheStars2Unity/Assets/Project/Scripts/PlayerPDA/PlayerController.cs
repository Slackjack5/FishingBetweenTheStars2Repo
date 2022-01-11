
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;

public class PlayerController : UdonSharpBehaviour
{
  public int playerCash;
  public GameObject GoldCounter;

  void Start()
  {
    playerCash = 10000;
  }

  private void FixedUpdate()
  {
    GoldCounter.GetComponent<TMPro.TextMeshProUGUI>().text = "Gold: " + playerCash;
  }

  public int GetCash()
  {
    return playerCash;
  }

  public void ChangeCash(int cash)
  {
    playerCash += cash;
  }

}
