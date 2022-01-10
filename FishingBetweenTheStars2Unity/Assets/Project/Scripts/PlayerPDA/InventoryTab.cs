
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
  public Sprite emptySlot;
  public Sprite[] SlotRarities;
  public Manager fishPool; // pool of fish to spawn in when items removed from inventory to put on hook


  private void Start()
  {
    for (int i = 0; i < (inventorySlots.Length / 2) - 1; i++)
    {
      if (isFull[i] == false)
      {
        AddToBag(Random.Range(1, 24));
      }
    }

  }
  private void FixedUpdate()
  {
    
    /*for (int i = 0; i < (inventorySlots.Length / 2) - 1; i++)
    {
      if (isFull[i] == false)
      {
        AddToBag(Random.Range(0, 24));
      }
    }*/
    
  }
  public void AddToBag(int Id) //Type AddtoBag and Give a fish ID, Will Automatically be tossed into the Inventory
  {
    slotIdSelected = -1;
    for (int i = 0; i < inventorySlots.Length - 1; i++)
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
        if(slotIdSelected == i && FishId[i] != 0)
        {
          // spawn fish object
          GameObject fishWorldObject = fishPool.AcquireGameObjectWithTag(""+Networking.LocalPlayer.playerId);
          if (fishWorldObject != null)
          {
            fishWorldObject.GetComponent<FishWorldObjectContainer>().EXUR_Reinitialize();
          }
          else
          {
            fishPool.AcquireObjectForEachPlayer();
            fishWorldObject = fishPool.AcquireGameObjectWithTag("" + Networking.LocalPlayer.playerId);
          }
          fishWorldObject.GetComponentInChildren<FishWorldObject>().SetObjectById(FishId[i]);
          fishWorldObject.transform.GetChild(0).position = gameObject.transform.position;
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
    inventorySlots[slot].GetComponent<Image>().sprite = SlotRarities[0];     //Change Slot Back to Grey
    isFull[slot] = false;    //Show that the slot is now Open
    FishId[slot] = 0;     //Make slot data our Empty Slot
    GameObject Child = inventorySlots[slot].transform.GetChild(0).gameObject;
    Child.GetComponent<Image>().sprite = emptySlot;
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
    transform.position = new Vector3(999999, 999999, 999999);
  }
}
