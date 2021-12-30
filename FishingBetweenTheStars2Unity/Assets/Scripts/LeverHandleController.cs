
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class LeverHandleController : UdonSharpBehaviour
{
    void OnDrop()
    {
        transform.localPosition = new Vector3(0, 0, 0);
    }
}
