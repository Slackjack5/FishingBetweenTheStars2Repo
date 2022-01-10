
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class FishWorldObject : UdonSharpBehaviour
{
    public FishDictionary dictionary;
    public InventoryTab inventory;
    private FishData fishData;
    private bool usedAsBait;
    private bool usedAsCash;
    [UdonSynced] private int spriteId;

    void FixedUpdate()
    {
        if(Vector3.Distance(Networking.LocalPlayer.GetPosition(), transform.position) > 5f)
        {
            inventory.AddToBag(spriteId);
            transform.parent.GetComponent<FishWorldObjectContainer>().EXUR_Finalize();
        }
    }
    public void SetObjectById(int id)
    {
        spriteId = id;
        GetComponent<SpriteRenderer>().sprite = dictionary.getFishData(id).getFishSprite();
        Debug.Log(spriteId);
        Debug.Log(GetComponent<SpriteRenderer>().sprite);
        fishData = dictionary.getFishData(id);
    }

    public FishData GetFishData()
    {
        return fishData;
    }
    
    public void UsedAsBait()
    {
        usedAsBait = true;
    }

    public void UsedAsCash() //If inside the Sell Machine, Show that its being used for Cash
    {
      if(!usedAsCash)
      {
      usedAsCash = true;
      }
      else { usedAsCash = false; }
      
    }

  public override void OnDrop() //When we drop the fish, if its being used for cash sell it, if not use it as bait?
   {
        if(!usedAsBait && !usedAsCash)
        {
            inventory.AddToBag(spriteId);
            transform.parent.GetComponent<FishWorldObjectContainer>().EXUR_Finalize();
        }
        else if (usedAsCash)
        {
            inventory.AddMoney(spriteId);
            transform.parent.GetComponent<FishWorldObjectContainer>().EXUR_Finalize();
            usedAsCash = false;
            Debug.Log("Sold Fish");
        }
        else
        {
          inventory.AddToBag(spriteId);
          transform.parent.GetComponent<FishWorldObjectContainer>().EXUR_Finalize();
        }


  }

    public override void OnDeserialization()
    {
        GetComponent<SpriteRenderer>().sprite = dictionary.getFishData(spriteId).getFishSprite();
    }
}
