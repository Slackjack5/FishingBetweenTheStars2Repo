
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class VelocityEstimator : UdonSharpBehaviour
{
    private const float DEFAULT_TIME_STEP = 0.02f;
    private float timeStepRatio;
    private Vector3[] prevVelocities;
    private int prevVelCount;
    private Vector3 predictedVelocity;
    private Vector3 prevPosition; // previous position the hook was at to do casting velocity calculations
    void Start()
    {
        timeStepRatio = Time.fixedDeltaTime/DEFAULT_TIME_STEP;
        prevVelocities = new Vector3[(int)(10/timeStepRatio)];
        prevPosition = transform.position;
        prevVelCount = 0;
        predictedVelocity = new Vector3();
    }
    void FixedUpdate()
    {
        if(prevVelCount < prevVelocities.Length - 1) 
        {
            prevVelocities[prevVelCount] = (transform.position - prevPosition);
            prevVelCount++;
        }
        else
        {
            Vector3 temp;
            temp = prevVelocities[prevVelocities.Length - 2];
            prevVelocities[prevVelocities.Length - 2] = prevVelocities[prevVelocities.Length - 1];
            for(int i = prevVelocities.Length - 2; i >= 1; i--) 
            {
                Vector3 temp2 = prevVelocities[i - 1];
                prevVelocities[i - 1] = temp;
                temp = temp2;
            }
            prevVelocities[prevVelocities.Length - 1] = (transform.position - prevPosition);
        }
        Vector3 sum = new Vector3();
        for(int i = 0; i < prevVelCount; i++)
        {
            sum += prevVelocities[i];
        }
        predictedVelocity = sum / ((float)prevVelCount * timeStepRatio);
        prevPosition = transform.position;
    }

    public Vector3 GetPredictedVelocity()
    {
        return predictedVelocity;
    }
}
