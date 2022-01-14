
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

public class FishSlot : UdonSharpBehaviour
{
    [Header("Required GameObjects")]
    public InventoryTab inventoryTab;
    public FishDictionary fishDictionary;
    public Sprite[] slotRarities;
    private int fishId;
    private bool isFull;
    void Start()
    {
        fishId = -1;
        isFull = false;
    }
    public bool SetFish(int id)
    {
        if(id < 0)
        {
            isFull = false;
            transform.GetChild(0).gameObject.GetComponent<Image>().sprite = fishDictionary.getFishData(-1).getFishSprite();
            GetComponent<Image>().sprite = slotRarities[4];
        }
        else if(id >= 0 && !isFull)
        {
            int rarity = fishDictionary.getFishData(id).getFishRarity();
            if(rarity > 4)
            {
                rarity = 4;
            }
            isFull = true;
            transform.GetChild(0).gameObject.GetComponent<Image>().sprite = fishDictionary.getFishData(id).getFishSprite();
            GetComponent<Image>().sprite = slotRarities[rarity];
        }
        else if(id >= 0 && isFull)
        {
            return false;
        }

        fishId = id;
        return true;
    }

    public int GetId()
    {
        if(fishId < 0)
        {
            return -1;
        }
        return fishId;
    }

    public void Selected()
    {
        inventoryTab.FishItemSelected(this);
    }
}
