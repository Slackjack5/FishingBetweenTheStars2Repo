
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;
using TMPro;

[DefaultExecutionOrder(4)]
public class WormSlot : UdonSharpBehaviour
{
    [Header("Required GameObjects")]
    public InventoryTab inventoryTab;
    public FishDictionary fishDictionary;
    public Sprite[] slotRarities;
    private int wormId = -1;
    private int quantity = 0;
    private const int MAX_WORMS_PER_SLOT = 10;

    // if the slot has worms in it already, or the id is invalid, then return false. otherwise return true
    public bool ChangeWorms(int newWorms, int id)
    {
        if(id != wormId && quantity > 0)
        {
            return false;
        }

        if(id < fishDictionary.WORM_OFFSET)
        {
            return false;
        }

        if(id != wormId)
        {
            transform.GetChild(0).gameObject.GetComponent<Image>().sprite = fishDictionary.getFishData(id).getFishSprite();
            GetComponent<Image>().sprite = slotRarities[fishDictionary.getFishData(id).getFishRarity()];
            if(quantity + newWorms > MAX_WORMS_PER_SLOT) return false;
            quantity += newWorms;
            wormId = id;
        }
        else
        {
            if(quantity + newWorms > MAX_WORMS_PER_SLOT) return false;
            quantity += newWorms;
        }

        if(quantity <= 0)
        {
            transform.GetChild(0).gameObject.GetComponent<Image>().sprite = fishDictionary.getFishData(-1).getFishSprite();
            GetComponent<Image>().sprite = slotRarities[3];
            wormId = -1;
        }

        transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = ""+quantity;

        return true;
    }

    public int GetId()
    {
        if(wormId < 0)
        {
            return -1;
        }
        return wormId;
    }

    public void Selected()
    {
        inventoryTab.WormItemSelected(this);
    }
}
