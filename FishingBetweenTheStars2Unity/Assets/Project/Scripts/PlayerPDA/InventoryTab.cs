
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

public class InventoryTab : UdonSharpBehaviour
{
  public bool[] isFull;
  public GameObject[] inventorySlots;
  public int[] FishId;
  public FishDictionary fishDictionary;

  private void FixedUpdate()
  {
    if (fishDictionary.getFishData(0)!=null) 
    {
      //Show that the Invetory slot is Full
      isFull[0] = true;
      //Put The Data of The Fish Taking That Slot
      FishId[0] = 6;
      //Put an Image of that Fish into the Slot
      GameObject Child = inventorySlots[0].transform.GetChild(0).gameObject;
      Child.GetComponent<Image>().sprite = fishDictionary.getFishData(FishId[0]).getFishSprite();
    }

  }

  public void AddToBag(int Id)
  {

  }

  public void ItemSelected()
  {
    Debug.Log("I Shit Myself");
  }
}
