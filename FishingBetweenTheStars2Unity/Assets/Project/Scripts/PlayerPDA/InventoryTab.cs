
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;
using TMPro;

public class InventoryTab : UdonSharpBehaviour
{
  public GameObject[] inventorySlots;
  public int[] FishId;
  public FishDictionary fishDictionary;
  public GameObject slotObjectSelected;
  private int slotIdSelected;
  public Sprite unknownFish;
  public Sprite emptySlot;
  public Sprite emptyBaitSlot;
  public Sprite[] SlotRarities;
  public Sprite[] FishSlotRarities;
  public Manager fishPool; // pool of fish to spawn in when items removed from inventory to put on hook
  private int[] slotItemQuantity;
  public int Tab;
  private bool[] isFull;


  private void Start()
  {
    isFull = new bool[14];
    slotItemQuantity=new int[14];
  }

  public void AddToBag(int Id) //Type AddtoBag and Give a fish ID, Will Automatically be tossed into the Inventory
  {
    slotIdSelected = -1;
    int startVal = 0;
    int endVal = 10;
    if(Id >= 10)
    {
      startVal = 10;
      endVal = 13;
    }
    for (int i = startVal; i < endVal; i++)
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
        //Change Slot Color to Rarity

        if(startVal == 10)
        {
          if(fishDictionary.getFishData(FishId[i]).getFishRarity() == 0)
          {
            inventorySlots[i].GetComponent<Image>().sprite = FishSlotRarities[0];
          } 
          else if (fishDictionary.getFishData(FishId[i]).getFishRarity() == 1)
          {
            inventorySlots[i].GetComponent<Image>().sprite = FishSlotRarities[1];
          }
          else if (fishDictionary.getFishData(FishId[i]).getFishRarity() == 2)
          {
            inventorySlots[i].GetComponent<Image>().sprite = FishSlotRarities[2];
          }
        }
        else
        {
          if(fishDictionary.getFishData(FishId[i]).getFishRarity()==0)
          {
            inventorySlots[i].GetComponent<Image>().sprite = SlotRarities[0];
          }
          else if (fishDictionary.getFishData(FishId[i]).getFishRarity() == 1)
          {
            inventorySlots[i].GetComponent<Image>().sprite = SlotRarities[2];
          }
          else if (fishDictionary.getFishData(FishId[i]).getFishRarity() == 2)
          {
            inventorySlots[i].GetComponent<Image>().sprite = SlotRarities[3];
          }
          else if (fishDictionary.getFishData(FishId[i]).getFishRarity() == 3)
          {
            inventorySlots[i].GetComponent<Image>().sprite = SlotRarities[4];
          }
        }
        break;
      }

    }
  }

  public void AddWorms(int Id, int quantity) //Type AddtoWorms and Give a fish ID, Will Automatically be tossed into the Inventory tab 2
  {
    // check for if any slot has the worm type in it already
    for (int i = 10; i < inventorySlots.Length - 1; i++)
    {
      if (FishId[i] == Id && slotItemQuantity[i] < 10)
      {
        slotItemQuantity[i] += quantity;
        GameObject Child = inventorySlots[i].transform.GetChild(1).gameObject;
        Child.GetComponent<TMPro.TextMeshProUGUI>().text = slotItemQuantity[i].ToString();
        if(slotItemQuantity[i] <= 0)
        {
          RemoveItem(i);
        }
        return;
      }
    }
    if(!(quantity < 1))
    {
      // then check for empty slots
      for (int i = 10; i < inventorySlots.Length - 1; i++)
      {
        if (isFull[i] == false)
        {
          slotItemQuantity[i] += quantity;
          AddToBag(Id);
          GameObject Child = inventorySlots[i].transform.GetChild(1).gameObject;
          Child.GetComponent<TMPro.TextMeshProUGUI>().text = slotItemQuantity[i].ToString();
          return;
        }
      }
    }
  }

  

    public void ItemSelected()
  {
    for (int i = 0; i < inventorySlots.Length - 1; i++)  //Run Through All our Slots
    {
      if(inventorySlots[i]==slotObjectSelected) //When we find the gameobject that matches the game object in the slot, run the code
      {
        if(slotIdSelected == i && FishId[i] != 0)
        {
          // spawn fish object
          GameObject fishWorldObject = fishPool.AcquireGameObjectWithTag(""+Networking.LocalPlayer.playerId);
          if (fishWorldObject != null)
          {
            if(!fishWorldObject.transform.GetChild(0).GetComponent<FishWorldObject>().GetPickedUp())
            {
              fishWorldObject.transform.GetChild(0).GetComponent<FishWorldObject>().OnDrop();
            }
            fishWorldObject.GetComponent<FishWorldObjectContainer>().EXUR_Reinitialize();
          }
          else
          {
            fishPool.AcquireObjectForEachPlayer();
            fishWorldObject = fishPool.AcquireGameObjectWithTag("" + Networking.LocalPlayer.playerId);
          }
          fishWorldObject.GetComponentInChildren<FishWorldObject>().SetObjectById(FishId[i]);
          fishWorldObject.transform.GetChild(0).position = gameObject.transform.position - gameObject.transform.forward * 0.1f;
          fishWorldObject.transform.GetChild(0).rotation = gameObject.transform.rotation;
          RemoveItem(i);
          return;
        }
        else
        {
          slotIdSelected = i;
          UpdateMainPanel(FishId[slotIdSelected]); //Update our main Panel
          break;
        }
      }
    }
  }

  public void RemoveItem(int slot) //Type Remove and Give a the slot you want to clear
  {
    //If on Tab 1 (Inventory Tab)
    if(slot < 10)
    {
      inventorySlots[slot].GetComponent<Image>().sprite = SlotRarities[0];     //Change Slot Back to Grey
      isFull[slot] = false;    //Show that the slot is now Open
      FishId[slot] = 0;     //Make slot data our Empty Slot
      GameObject Child = inventorySlots[slot].transform.GetChild(0).gameObject;
      Child.GetComponent<Image>().sprite = emptySlot;
    }
    else if(slot >= 10) //If on Tab 2 (Worm Tab
    {
      if(slotItemQuantity[slot]>0)
      {
        slotItemQuantity[slot]--;
        GameObject Child = inventorySlots[slot].transform.GetChild(1).gameObject;
        Child.GetComponent<TMPro.TextMeshProUGUI>().text = slotItemQuantity[slot].ToString();
        if(slotItemQuantity[slot] == 0)
        {
          inventorySlots[slot].GetComponent<Image>().sprite = SlotRarities[0];     //Change Slot Back to Grey
          isFull[slot] = false;    //Show that the slot is now Open
          FishId[slot] = 0;     //Make slot data our Empty Slot
          GameObject Child2 = inventorySlots[slot].transform.GetChild(0).gameObject;
          Child2.GetComponent<Image>().sprite = emptySlot;
          inventorySlots[slot].GetComponent<Image>().sprite = emptyBaitSlot;
        }
      }
      else
      {
        inventorySlots[slot].GetComponent<Image>().sprite = emptyBaitSlot;   //Change Slot Back to Grey
        isFull[slot] = false;    //Show that the slot is now Open
        FishId[slot] = 0;     //Make slot data our Empty Slot
        GameObject Child = inventorySlots[slot].transform.GetChild(0).gameObject;
        Child.GetComponent<Image>().sprite = emptySlot;
      }
    }
  }

  public void AddMoney(int id)
  {
    GameObject Parent = transform.parent.gameObject;
    Parent.GetComponent<PlayerController>().playerCash += fishDictionary.getFishData(id).getFishValue();
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

    //Change Slot Color and Tag to Rarity
    Child = inventorySlots[inventorySlots.Length - 1].transform.GetChild(7).gameObject;
    if (fishDictionary.getFishData(FishId[inventorySlots.Length - 1]).getFishRarity() == 0)
    {
      inventorySlots[inventorySlots.Length - 1].GetComponent<Image>().sprite = SlotRarities[0];
      Child.GetComponent<TMPro.TextMeshProUGUI>().text = "Common";
    }
    else if (fishDictionary.getFishData(FishId[inventorySlots.Length - 1]).getFishRarity() == 1)
    {
      inventorySlots[inventorySlots.Length - 1].GetComponent<Image>().sprite = SlotRarities[2];
      Child.GetComponent<TMPro.TextMeshProUGUI>().text = "Uncommon";
    }
    else if (fishDictionary.getFishData(FishId[inventorySlots.Length - 1]).getFishRarity() == 2)
    {
      inventorySlots[inventorySlots.Length - 1].GetComponent<Image>().sprite = SlotRarities[3];
      Child.GetComponent<TMPro.TextMeshProUGUI>().text = "Rare";
    }
    else if (fishDictionary.getFishData(FishId[inventorySlots.Length - 1]).getFishRarity() == 3)
    {
      inventorySlots[inventorySlots.Length - 1].GetComponent<Image>().sprite = SlotRarities[4];
      Child.GetComponent<TMPro.TextMeshProUGUI>().text = "Legendary";
    }
    else if (fishDictionary.getFishData(FishId[inventorySlots.Length - 1]).getFishRarity() == 5)
    {
      inventorySlots[inventorySlots.Length - 1].GetComponent<Image>().sprite = SlotRarities[0];
      Child.GetComponent<TMPro.TextMeshProUGUI>().text = ""; //Empty Slot
    }
  }

  public void ClosePanel()
  {
    transform.parent.position = new Vector3(999999, 999999, 999999);
  }
}
