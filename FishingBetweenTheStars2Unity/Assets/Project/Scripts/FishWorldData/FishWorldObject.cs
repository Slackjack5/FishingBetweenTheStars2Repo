
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

    public void UsedAsCash()
    {
      usedAsBait = true;
    }

  public override void OnDrop()
    {
        if(!usedAsBait)
        {
            inventory.AddToBag(spriteId);
            transform.parent.GetComponent<FishWorldObjectContainer>().EXUR_Finalize();
        }
        else if (!usedAsCash)
        {
          inventory.AddMoney(spriteId);
          transform.parent.GetComponent<FishWorldObjectContainer>().EXUR_Finalize();
          Debug.Log("Sold Fish");
        }
    }

    public override void OnDeserialization()
    {
        GetComponent<SpriteRenderer>().sprite = dictionary.getFishData(spriteId).getFishSprite();
    }
}
