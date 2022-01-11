
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class BuyingPanelController : UdonSharpBehaviour
{
    public InventoryTab inventory;
    public PlayerController player;
    public Manager manager;
    public void BuyRegularWorm()
    {
        manager.AcquireObjectForEachPlayer();
        if(player.GetCash() >= 5)
        {
            inventory.AddWorms(35, 1);
            player.ChangeCash(-5);
        }
    }
    
    public void BuyCosmicWorm()
    {
        if(player.GetCash() >= 35)
        {
            inventory.AddWorms(36, 1);
            player.ChangeCash(-35);
        }
    }

    public void BuyVoidStarfish()
    {
        if(player.GetCash() >= 75)
        {
            inventory.AddWorms(37, 1);
            player.ChangeCash(-75);
        }
    }
}

