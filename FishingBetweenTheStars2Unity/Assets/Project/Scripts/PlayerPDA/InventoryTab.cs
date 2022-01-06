
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

public class InventoryTab : UdonSharpBehaviour
{
  public bool[] isFull;
  public GameObject[] inventorySlots;
  public FishData[] Fish;
    void Start()
    {
        
    }

  private void FixedUpdate()
  {
    isFull[0]=true;
    Fish[0]=GameObject.Find("FishDictionary").GetComponent<FishDictionary>().getFishData(6);
    
  }
}
