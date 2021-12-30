
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class LeverController : UdonSharpBehaviour
{
    private GameObject handleController;
    private GameObject leverHandle;
    private Rigidbody lever;
    public float magnitude; // force multiplier on the handle
    public float maxDistToTurn; // maximum distance from handle allowed to turn the reel
    void Start()
    {
        lever = gameObject.GetComponent<Rigidbody>();
        leverHandle = gameObject.transform.GetChild(0).gameObject;
        handleController = gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject; // get the second child
    }

    void FixedUpdate()
    {
        if(Vector3.Distance(leverHandle.transform.position, handleController.transform.position) < maxDistToTurn) 
        {
            lever.AddTorque((leverHandle.transform.position - handleController.transform.position) * magnitude);
        }
    }

    void OnDrawGizmos()
    {
        if(leverHandle != null) {
            Gizmos.DrawSphere(leverHandle.transform.position, 1f);
            Gizmos.DrawSphere(handleController.transform.position, 1f);
        }
    }
}
