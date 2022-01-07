
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
  public GameObject slotObjectSelected;
  private int slotIdSelected;

  private void FixedUpdate()
  {
    if (fishDictionary.getFishData(0)!=null) 
    {
      AddToBag(6);
    }

  }

  public void AddToBag(int Id)
  {
    for (int i = 0; i < inventorySlots.Length-1; i++)
    {
      if (isFull[i]==false)
      {
        //Get The Child Image of our Slot
        GameObject Child = inventorySlots[i].transform.GetChild(0).gameObject;
        //Put Fish Id in that Slot
        FishId[i] = Id;
        //Change Sprite in that Slot
        Child.GetComponent<Image>().sprite = fishDictionary.getFishData(FishId[i]).getFishSprite();
        //Say the Slot is Full
        isFull[i] = true;
        break;
      }

    }
  }

  public void ItemSelected()
  {
    for (int i = 0; i < inventorySlots.Length - 1; i++)  //Run Through All our Slots
    {
      if(inventorySlots[i]==slotObjectSelected) //When we find the gameobject that matches the game object in the slot, run the code
      {
        slotIdSelected = i;
        UpdateMainPanel(FishId[slotIdSelected]); //Update our main Panel
        break;
      }
    }
  }

  public void UpdateMainPanel(int Id)
  {
    //Get The Child Image of our Slot
    GameObject Child = inventorySlots[inventorySlots.Length-1].transform.GetChild(0).gameObject;
    //Put Fish Id in that Slot
    FishId[inventorySlots.Length-1] = Id;
    //Change Sprite in that Slot
    Child.GetComponent<Image>().sprite = fishDictionary.getFishData(FishId[inventorySlots.Length-1]).getFishSprite();
    //Say the Slot is Full
    isFull[inventorySlots.Length-1] = true;
  }
}
