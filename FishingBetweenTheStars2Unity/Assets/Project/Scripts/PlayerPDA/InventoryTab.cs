
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;
using TMPro;

public class InventoryTab : UdonSharpBehaviour
{
  public bool[] isFull;
  public GameObject[] inventorySlots;
  public int[] FishId;
  public FishDictionary fishDictionary;
  public GameObject slotObjectSelected;
  private int slotIdSelected;
  public Sprite unknownFish;

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
        if(fishDictionary.getFishData(FishId[i]).getFishSprite()!=null)
        {
          Child.GetComponent<Image>().sprite = fishDictionary.getFishData(FishId[i]).getFishSprite();
        }
        else
        {
          Child.GetComponent<Image>().sprite = unknownFish;
        }
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
    //Change Main Image

    //Get The Child Image of our Slot
    GameObject Child = inventorySlots[inventorySlots.Length-1].transform.GetChild(0).gameObject;
    //Put Fish Id in that Slot
    FishId[inventorySlots.Length-1] = Id;
    //Change Sprite in that Slot
    if(fishDictionary.getFishData(FishId[inventorySlots.Length - 1]).getFishSprite()!=null)
    {
      Child.GetComponent<Image>().sprite = fishDictionary.getFishData(FishId[inventorySlots.Length - 1]).getFishSprite();
    }
    else
    {
      Child.GetComponent<Image>().sprite = unknownFish;
    }

    //Say the Slot is Full
   
    isFull[inventorySlots.Length-1] = true;
    
   //Change Fish Name 
   Child = inventorySlots[inventorySlots.Length - 1].transform.GetChild(1).gameObject;
   Child.GetComponent<TMPro.TextMeshProUGUI>().text = fishDictionary.getFishData(FishId[inventorySlots.Length - 1]).getFishName();
   //Change Fish Description
   Child = inventorySlots[inventorySlots.Length - 1].transform.GetChild(2).gameObject;
   Child.GetComponent<TMPro.TextMeshProUGUI>().text = fishDictionary.getFishData(FishId[inventorySlots.Length - 1]).getFishDescription();
   //Change Value Number
   Child = inventorySlots[inventorySlots.Length - 1].transform.GetChild(3).gameObject;
   Child.GetComponent<TMPro.TextMeshProUGUI>().text = fishDictionary.getFishData(FishId[inventorySlots.Length - 1]).getFishValue().ToString();
   //Change Power Number
   Child = inventorySlots[inventorySlots.Length - 1].transform.GetChild(4).gameObject;
   Child.GetComponent<TMPro.TextMeshProUGUI>().text = fishDictionary.getFishData(FishId[inventorySlots.Length - 1]).getFishPower().ToString();
   //Change Favorite Food Image
   Child = inventorySlots[inventorySlots.Length - 1].transform.GetChild(5).gameObject;
   if (fishDictionary.getFishData(FishId[inventorySlots.Length - 1]).getFavoriteFish()==9999)
   {
     Child.GetComponent<Image>().sprite = unknownFish;
   }
   else
   {
     Child.GetComponent<Image>().sprite = fishDictionary.getFishData(fishDictionary.getFishData(FishId[inventorySlots.Length - 1]).getFavoriteFish()).getFishSprite();
   }
   //Change Tier Number
   Child = inventorySlots[inventorySlots.Length - 1].transform.GetChild(6).gameObject;
   Child.GetComponent<TMPro.TextMeshProUGUI>().text = fishDictionary.getFishData(FishId[inventorySlots.Length - 1]).getFishTier().ToString();
   
   
  }
}
