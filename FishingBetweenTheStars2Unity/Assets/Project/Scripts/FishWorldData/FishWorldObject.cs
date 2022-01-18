
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class FishWorldObject : UdonSharpBehaviour
{
    public FishDictionary dictionary;
    public InventoryTab inventory;
    public PlayerController player;
    private FishData fishData;
    private bool usedAsBait;
    private bool usedAsCash;
    private bool fishPickedUp;
    [UdonSynced] private int spriteId;

    public void Start()
    {
        fishPickedUp = false;
    }

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
    public override void OnPickup()
    {
        fishPickedUp = true;
    }

    public bool GetPickedUp()
    {
        return fishPickedUp;
    }

  public override void OnDrop() //When we drop the fish, if its being used for cash sell it, if not use it as bait?
   {
        if(!usedAsBait && !usedAsCash)
        {
            if(spriteId >= dictionary.WORM_OFFSET)
            {
                inventory.AddWorms(spriteId, 1);
                transform.parent.GetComponent<Handler>().ReleaseObject();
            }
            else
            {
                inventory.AddToBag(spriteId);
                transform.parent.GetComponent<Handler>().ReleaseObject();
            }
        }
        else if (usedAsCash)
        {
            player.ChangeCash(dictionary.getFishData(spriteId).getFishValue());
            if(spriteId >= dictionary.WORM_OFFSET)
            {
                transform.parent.GetComponent<Handler>().ReleaseObject();
                usedAsCash = false;
            }
            else
            {
                transform.parent.GetComponent<Handler>().ReleaseObject();
                usedAsCash = false;
            }
        }
        else
        {
          transform.parent.GetComponent<Handler>().ReleaseObject();
        }


  }

    public override void OnDeserialization()
    {
        GetComponent<SpriteRenderer>().sprite = dictionary.getFishData(spriteId).getFishSprite();
    }
}
