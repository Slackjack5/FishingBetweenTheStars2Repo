
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class InventorySlot : UdonSharpBehaviour
{
 public void selected()
  {
    GameObject temp = gameObject.transform.parent.gameObject;
    temp.GetComponent<InventoryTab>().slotObjectSelected = gameObject;
  }
}
