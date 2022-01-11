
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class InventorySlot : UdonSharpBehaviour
{
  public InventoryTab inventoryTab;
 public void selected()
  {
    inventoryTab.slotObjectSelected = gameObject;
  }
}
