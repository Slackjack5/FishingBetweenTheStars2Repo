
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class SwingIndicatorController : UdonSharpBehaviour
{
    private bool hasCollidedWithLine;
    void OnTriggerEnter(Collider collider)
    {
        if(Networking.GetOwner(collider.gameObject) == Networking.LocalPlayer && collider.gameObject.name == "line") 
        {
            hasCollidedWithLine = true;
        }
    }

    public bool GetCollidedWithLine()
    {
        return hasCollidedWithLine;
    }
}
