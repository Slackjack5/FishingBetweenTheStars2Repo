
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Player : UdonSharpBehaviour
{
    private float maxSpeed; // max speed the player can move the area to catch the fish
    private float accel; // acceleration to max speed
    private float gravity; // how fast the area falls
    private float bounce; // what percentage of momentum is maintained when area bounces at bottom of UI
    private float speed; // player's current speed
    private float position; // player's current position
    public void CreatePlayer(float maxSpeed, float accel, float gravity, float bounce)
    {
        this.maxSpeed = maxSpeed;
        this.accel = accel;
        this.bounce = bounce;
        this.gravity = gravity;
        speed = 0;
        position = 0;
    }

    // necessary to tell player to be in bounds at the start of the game
    public void SetPosition(float position)
    {
        this.position = position;
    }

    public float GetPosition()
    {
        return position;
    }

    public void Move(float boardSize, float bounds, bool isReeling)
    {
        if(position + speed <= bounds && !isReeling)
        {
            speed = -speed * bounce; // bounce off the floor
            position += speed;
        }
        else if(position + speed >= boardSize - bounds && isReeling)
        {
            speed = 0;
            position = boardSize - bounds;
        }
        else if(isReeling)
        {
            speed = Mathf.Min(maxSpeed, speed + accel);
            position += speed;
        }
        else if(!isReeling)
        {
            speed = Mathf.Max(-maxSpeed, speed - gravity);
            position += speed;
        }
    }
}
