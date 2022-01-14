
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;
using TMPro;

public class InventoryTab : UdonSharpBehaviour
{
  [Header("Inventory slot GameObjects")]
  public FishSlot[] fishSlots;
  public WormSlot[] wormSlots;
  public Sprite[] mainPanelRarities;
  public GameObject mainPanel;
  [Header("Required Objects")]
  public FishDictionary fishDictionary;  
  public Sprite unknownFish;
  public Manager fishPool; // pool of fish to spawn in when items removed from inventory to put on hook
  private GameObject slotObjectSelected;

  void Start()
  {
    for(int i = 0; i < 6; i++)
    {
      AddToBag(fishDictionary.rollFish(0).getFishId());
    }

    for(int i = 0; i < 2; i++)
    {
      AddWorms(fishDictionary.WORM_OFFSET, 5);
    }
  }

  public void AddToBag(int id) //Type AddtoBag and Give a fish ID, Will Automatically be tossed into the Inventory
  {
    foreach(FishSlot fishSlot in fishSlots)
    {
      if(fishSlot.SetFish(id)) return;
    }
  }

  public void AddWorms(int id, int quantity) //Type AddtoWorms and Give a fish ID, Will Automatically be tossed into the Inventory tab 2
  {
    foreach(WormSlot wormSlot in wormSlots)
    {
      if(wormSlot.ChangeWorms(quantity, id)) return;
    }
  }

  private void SpawnFishObject(int id)
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
    fishWorldObject.GetComponentInChildren<FishWorldObject>().SetObjectById(id);
    fishWorldObject.transform.GetChild(0).position = gameObject.transform.position - gameObject.transform.forward * 0.1f;
    fishWorldObject.transform.GetChild(0).rotation = gameObject.transform.rotation;
  }

  public void FishItemSelected(FishSlot selected)
  {
    for(int i = 0; i < fishSlots.Length; i++)
    {
      if(fishSlots[i] == selected)
      {
        if(slotObjectSelected == selected.gameObject && selected.GetId() >= 0)
        {
          SpawnFishObject(fishSlots[i].GetId());
          RemoveFish(i);
          return;
        }
        else
        {
          slotObjectSelected = selected.gameObject;
          UpdateMainPanel(selected.gameObject);
        }
      }
    }
  }

  public void WormItemSelected(WormSlot selected)
  {
    for(int i = 0; i < wormSlots.Length; i++)
    {
      if(wormSlots[i] == selected)
      {
        if(slotObjectSelected == selected.gameObject && selected.GetId() >= 0)
        {
          SpawnFishObject(wormSlots[i].GetId());
          AddWorms(wormSlots[i].GetId(), -1);
          return;
        }
        else
        {
          slotObjectSelected = selected.gameObject;
          UpdateMainPanel(selected.gameObject);
        }
      }
    }
  }

  // removes a fish given a fishSlotId
  public void RemoveFish(int fishSlotId)
  {
    fishSlots[fishSlotId].SetFish(-1);
  }

  public void RemoveWorm(int wormSlotId)
  {
    wormSlots[wormSlotId].ChangeWorms(-1, wormSlotId);
  }

  public void UpdateMainPanel(GameObject obj)
  {
    // get the id of the fish or worm
    int id = obj.GetComponent<WormSlot>() != null ? obj.GetComponent<WormSlot>().GetId() : obj.GetComponent<FishSlot>().GetId();

    GameObject child;
    child = mainPanel.transform.GetChild(0).gameObject;

    child.GetComponent<Image>().sprite = id >= 0 ? fishDictionary.getFishData(id).getFishSprite() : unknownFish;
    //Change Sprite in that Slot

    //Change Fish Name 
    child = mainPanel.transform.GetChild(1).gameObject;
    child.GetComponent<TMPro.TextMeshProUGUI>().text = fishDictionary.getFishData(id).getFishName();
    //Change Fish Description
    child = mainPanel.transform.GetChild(2).gameObject;
    child.GetComponent<TMPro.TextMeshProUGUI>().text = fishDictionary.getFishData(id).getFishDescription();
    //Change Value Number
    child = mainPanel.transform.GetChild(3).gameObject;
    child.GetComponent<TMPro.TextMeshProUGUI>().text = fishDictionary.getFishData(id).getFishValue().ToString();
    //Change Power Number
    child = mainPanel.transform.GetChild(4).gameObject;
    child.GetComponent<TMPro.TextMeshProUGUI>().text = fishDictionary.getFishData(id).getFishPower().ToString();
    //Change Favorite Food Image
    child = mainPanel.transform.GetChild(5).gameObject;
    child.GetComponent<Image>().sprite = fishDictionary.getFishData(id).getFavoriteFish() == 9999 ? unknownFish : fishDictionary.getFishData(fishDictionary.getFishData(id).getFavoriteFish()).getFishSprite();
    //Change Tier Number
    child = mainPanel.transform.GetChild(6).gameObject;
    child.GetComponent<TMPro.TextMeshProUGUI>().text = fishDictionary.getFishData(id).getFishTier().ToString();

    //Change Slot Color and Tag to Rarity
    child = mainPanel.transform.GetChild(7).gameObject;
    int rarity = fishDictionary.getFishData(id).getFishRarity();
    if(rarity > 4)
    {
      rarity = 4;
    }
    mainPanel.GetComponent<Image>().sprite = mainPanelRarities[rarity];

    TMPro.TextMeshProUGUI gui = child.GetComponent<TMPro.TextMeshProUGUI>();
    string rarityMessage;
    switch(rarity)
    {
      case 0: rarityMessage = "Common"; break;
      case 1: rarityMessage = "Uncommon"; break;
      case 2: rarityMessage = "Rare"; break;
      case 3: rarityMessage = "Legendary"; break;
      default: rarityMessage = ""; break;
    }
    gui.text = rarityMessage;
  }

  public void ClosePanel()
  {
    transform.parent.position = new Vector3(999999, 999999, 999999);
  }
}
