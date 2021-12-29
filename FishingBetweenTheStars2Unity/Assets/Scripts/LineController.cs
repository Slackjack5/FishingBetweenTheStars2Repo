
using UdonSharp;
using UnityEngine;
using System.Collections.Generic;
using VRC.SDKBase;
using VRC.Udon;

public class LineController : UdonSharpBehaviour
{
    private LineRenderer lineRenderer;
    private Rigidbody hookRigidbody;
    private Vector3 predictedVelocity;
    private Vector3 prevPosition;
    private Vector3[] prevVelocities;
    private GameObject hookParent;
    private int prevVelCount;
    public GameObject hook;
    void Start()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        hookRigidbody = hook.GetComponent<Rigidbody>();
        lineRenderer.forceRenderingOff = true;
        prevVelocities = new Vector3[10];
        prevPosition = transform.position;
        prevVelCount = 0;
    }

    void FixedUpdate()
    {
        if(prevVelCount < 9) 
        {
            prevVelocities[prevVelCount] = (transform.position - prevPosition);
            prevVelCount++;
        }
        else
        {
            Vector3 temp;
            temp = prevVelocities[prevVelocities.Length - 2];
            prevVelocities[prevVelocities.Length - 2] = prevVelocities[prevVelocities.Length - 1];
            for(int i = prevVelocities.Length - 2; i >= 1; i--) {
                Vector3 temp2 = prevVelocities[i - 1];
                prevVelocities[i - 1] = temp;
                temp = temp2;
            }
            prevVelocities[prevVelocities.Length - 1] = transform.position - prevPosition;
        }
        Vector3 sum = new Vector3();
        for(int i = 0; i < prevVelCount; i++)
        {
            sum += prevVelocities[i];
        }
        predictedVelocity = sum / (float)prevVelCount;
        prevPosition = transform.position;
    }

    public void CastLine()
    {
        hookRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        hookRigidbody.AddForce(predictedVelocity);
        hookParent = hook.transform.parent.gameObject; // store the parent for later when we want to reattach the hook
        hook.transform.SetParent(null); // we don't want the hook to move with respect to the rod anymore
        lineRenderer.forceRenderingOff = false;
    }
}
