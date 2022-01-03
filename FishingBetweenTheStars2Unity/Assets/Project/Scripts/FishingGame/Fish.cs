
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Fish : UdonSharpBehaviour
{
    private float catchRate; // catch percentage per tick when fish is in catch range
    private float escapeRate; // catch percentage decrease per tick when fish is not in catch range
    private float accel; // rate at which fish accelerates
    private float minDistToTarget; // how close the fish needs to be to its target to pick a new target
    private float target; // current target the fish is moving towards
    private float position; // current position
    private float percentageCaught; // how caught the fish is
    private const int MAX_DIST = 100; // max dist ticks to go to target
    private const int MIN_DIST = 30; // min dist ticks to go to target
    public void CreateFish(float catchRate, float escapeRate, float maxSpeed, float accel, float minDistToTarget)
    {
        this.catchRate = catchRate;
        this.escapeRate = escapeRate;
        this.accel = accel;
        this.minDistToTarget = minDistToTarget;
        target = 0;
        position = 0;
        percentageCaught = 0;
    }

    // necessary to tell fish to be in bounds at the start of the game
    public void SetPosition(float position)
    {
        this.position = position;
    }

    public float GetPosition()
    {
        return position;
    }

    public void AddCaught()
    {
        percentageCaught = Mathf.Min(1, percentageCaught + catchRate);
    }

    public void AddEscape()
    {
        percentageCaught = Mathf.Max(0, percentageCaught - escapeRate);
    }

    public float GetPercentageCaught()
    {
        return percentageCaught;
    }

    public float GetCaught()
    {
        return percentageCaught;
    }

    public void PickNextTarget(float boardSize, float bounds)
    {
        target = Random.value * ((float)boardSize - (float)bounds) + (float)bounds;
        // pick a target within a certain range of the fish based on its speed
        // this will be a constant because it probably should be
        while(Mathf.Abs(position - target) > MAX_DIST || Mathf.Abs(position - target) < MIN_DIST)
        {
            target = Random.value * ((float)boardSize - (float)bounds) + (float)bounds;
        }
    }

    public void SetTarget(float target)
    {
        this.target = target;
    }

    public void Move(float boardSize, float bounds)
    {
        if(Mathf.Abs(position - target) < minDistToTarget)
        {
            PickNextTarget(boardSize, bounds);
            return;
        }
        position = Mathf.Lerp(position, target, accel);
    }
}
