
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

public class FishingGameController : UdonSharpBehaviour
{
    [Header("Fish variables")]
    public float catchRate; // catch percentage per tick when fish is in catch range
    public float escapeRate; // catch percentage decrease per tick when fish is not in catch range
    public float fishMaxSpeed; // how fast the fish can move at max speed
    public float fishAccel; // rate at which fish accelerates
    public float minDistToTarget; // how close the fish needs to be to its target to pick a new target
    [Header("Player variables")]
    public float maxFallSpeed;
    public float maxSpeed; // max speed the player can move the area to catch the fish
    public float accel; // acceleration to max speed
    public float gravity; // how fast the area falls
    public float bounce; // what percentage of momentum is maintained when area bounces at bottom of UI
    public float boundAdjustment; // multiplier on bounds of the area inside the player (hitbox adjustment)
    [Header("UI elements")]
    public RectTransform playerUI;
    public RectTransform fishUI;
    public RectTransform backgroundUI;
    public Slider progressUI;
    [Header("Requisite prefabs")]
    public GameObject playerPrefab;
    public GameObject fishPrefab;
    public GameObject linePrefab;
    public GameObject canvas;

    private bool gameActive; // is the game active?
    private Fish fish; // the fish being caught in this game
    private Player player; // the player in the game
    private LineController line; // getting data from the fishing rod
    private const float boardSize = 200; // how big the board is
    private float bounds; // the size of the bounds of the player, determined by boardSize, UIboardSize, and playerUI size
    private int gameTicks; // how many game ticks have happened

    float gameToUICoords(float gameCoords)
    {
        return -(gameCoords * backgroundUI.rect.width/boardSize - backgroundUI.rect.width/2);
    }

    // needs to be public so sliders can reinstantiate stuff
    public void Start()
    {
        bounds = boardSize/(backgroundUI.rect.width/playerUI.rect.width) * boundAdjustment;
        fish = GetComponentInChildren<Fish>();
        fish.CreateFish(catchRate, escapeRate, fishMaxSpeed, fishAccel, minDistToTarget);
        fish.SetPosition(bounds);
        fish.SetTarget(bounds);
        player = GetComponentInChildren<Player>();
        player.CreatePlayer(maxSpeed, maxFallSpeed, accel, gravity, bounce);
        player.SetPosition(bounds);
        line = linePrefab.GetComponent<LineController>();
        canvas.SetActive(false);
        gameTicks = 0;
    }

    void FixedUpdate()
    {
        if(line.GetWater())
        {
            if(!canvas.activeSelf)
            {
                canvas.SetActive(true);
            }
            fish.Move(boardSize, bounds);
            player.Move(boardSize, bounds, line.GetReeling());
            gameTicks++;
            if(Mathf.Abs(fish.GetPosition() - player.GetPosition()) < bounds)
            {
                fish.AddCaught();
            }
            else
            {
                fish.AddEscape();
            }
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        Vector3 fishPos = fishUI.anchoredPosition3D;
        fishPos.x = gameToUICoords(fish.GetPosition());
        fishUI.anchoredPosition3D = fishPos;

        Vector3 playerPos = playerUI.anchoredPosition3D;
        playerPos.x = gameToUICoords(player.GetPosition());
        playerUI.anchoredPosition3D = playerPos;

        progressUI.value = fish.GetCaught();
    }

    public int GetGameTicks()
    {
        return gameTicks;
    }

}
