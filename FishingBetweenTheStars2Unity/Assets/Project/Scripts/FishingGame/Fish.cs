
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Fish : UdonSharpBehaviour
{
    private int fishType; // 0 mixed, 1 sinker, 2 floater, 3 dart
    private float difficulty; // percentage val from 0 to 1
    private const float DEFAULT_CATCH_RATE = 0.0015f;
    private float[] mixed = {0.03f, 0.03f, 0.002f};
    private float[] sinker = {0.015f, 0.03f, 0.002f};
    private float[] floater = {0.03f, 0.015f, 0.002f};
    private float[] dart = {0.03f, 0.03f, 0.01f};
    private float catchRate; // catch percentage per tick when fish is in catch range or escape rate when not
    private float upAccel; // rate at which fish accelerates upwards
    private float downAccel; // rate at which fish accelerates downwards
    private float erraticness; // how erratic the fish's movement is (1 percent is 1 percent chance to change targets per tick)
    private float target; // current target the fish is moving towards
    private float position; // current position
    private float percentageCaught; // how caught the fish is
    private const int TICKS_TO_MIN = 1000; // how many game ticks it should take to get min distance
    private const int TICKS_TO_MAX = 5000; // how many game ticks it should take to get max distance
    private float maxDist; // max dist ticks to go to target
    private float minDist; // min dist ticks to go to target
    private float timeStep;

    public void CreateFish(int fishType, float difficulty, float timeStep)
    {
        this.timeStep = timeStep;
        if(fishType == 0)
        {
            CreateFishPriv(mixed[0] * difficulty * timeStep, mixed[1] * difficulty * timeStep, mixed[2] * difficulty * timeStep);
        }
        else if(fishType == 1)
        {
            CreateFishPriv(sinker[0] * difficulty * timeStep, sinker[1] * difficulty * timeStep, sinker[2] * difficulty * timeStep);
        }
        else if(fishType == 2)
        {
            CreateFishPriv(floater[0] * difficulty * timeStep, floater[1] * difficulty * timeStep, floater[2] * difficulty * timeStep);
        }
        else if(fishType == 3)
        {
            CreateFishPriv(dart[0] * difficulty * timeStep, dart[1] * difficulty * timeStep, dart[2] * difficulty * timeStep);
        }
    }
    void CreateFishPriv(float upAccel, float downAccel, float erraticness)
    {
        maxDist = Mathf.Max(upAccel, downAccel) * TICKS_TO_MAX;
        minDist = Mathf.Max(upAccel, downAccel) * TICKS_TO_MIN;
        this.upAccel = upAccel;
        this.downAccel = downAccel;
        this.erraticness = erraticness;
        target = 0;
        position = 0;
        percentageCaught = 0.15f;
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
        percentageCaught = Mathf.Min(1, percentageCaught + DEFAULT_CATCH_RATE * timeStep);
    }

    public void AddEscape()
    {
        percentageCaught = Mathf.Max(0, percentageCaught - DEFAULT_CATCH_RATE * timeStep);
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
        float actualMaxDist = Mathf.Max(maxDist, boardSize - bounds);
        target = Mathf.Max(Mathf.Min(Random.value * (actualMaxDist * 2) + position - actualMaxDist, boardSize - bounds), bounds);
        if(target < position && Mathf.Abs(target - position) < minDist && target != bounds)
        {
            target -= minDist;
        }
        if(target > position && Mathf.Abs(target - position) < minDist && target != boardSize - bounds)
        {
            target += minDist;
        }
    }

    public void SetTarget(float target)
    {
        this.target = target;
    }

    public void Move(float boardSize, float bounds)
    {
        if(Mathf.Abs(position - target) < 10f)
        {
            PickNextTarget(boardSize, bounds);
            return;
        }
        else if(Random.value < erraticness)
        {
            Debug.Log("erratic movement triggered");
            PickNextTarget(boardSize, bounds);
            return;
        }
        if(target >= position)
        {
            position = Mathf.Lerp(position, target, upAccel);
        }
        else if(target < position)
        {
            position = Mathf.Lerp(position, target, upAccel);
        }
    }
}
