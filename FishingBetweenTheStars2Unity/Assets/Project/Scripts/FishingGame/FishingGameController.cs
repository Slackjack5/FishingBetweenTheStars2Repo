
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

[DefaultExecutionOrder(2)]
public class FishingGameController : UdonSharpBehaviour
{
    private const float DEFAULT_TIME_STEP = 0.02f;
    private float timeStepRatio;
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
    public float swingRadius; // radius in which the swing sphere can spawn
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
    public GameObject newRod; // rod prefab for swinging
    public GameObject swingIndicator; // swing indicator prefab

    private bool gameActive; // is the game active?
    private Fish fish; // the fish being caught in this game
    private Player player; // the player in the game
    private LineController line; // getting data from the fishing rod
    private const float boardSize = 200; // how big the board is
    private float bounds; // the size of the bounds of the player, determined by boardSize, UIboardSize, and playerUI size
    private bool hasSwungRod;
    private bool swingIndicatorSpawned;
    private GameObject currentSwingIndicator;
    private Transform initialPosOfRod; // initialPos of rod when swing event occured

    float gameToUICoords(float gameCoords)
    {
        return -(gameCoords * backgroundUI.rect.width/boardSize - backgroundUI.rect.width/2);
    }

    // needs to be public so sliders can reinstantiate stuff
    public void Start()
    {
        Destroy(currentSwingIndicator);
        timeStepRatio = Time.fixedDeltaTime/DEFAULT_TIME_STEP;
        bounds = boardSize/(backgroundUI.rect.width/playerUI.rect.width) * boundAdjustment;
        fish = GetComponentInChildren<Fish>();
        fish.CreateFish(catchRate * timeStepRatio, escapeRate * timeStepRatio, fishMaxSpeed * timeStepRatio, fishAccel * timeStepRatio, minDistToTarget);
        fish.SetPosition(bounds);
        fish.SetTarget(bounds);
        player = GetComponentInChildren<Player>();
        player.CreatePlayer(maxSpeed * timeStepRatio, maxFallSpeed * timeStepRatio, accel * timeStepRatio, gravity * timeStepRatio, bounce);
        player.SetPosition(bounds);
        line = linePrefab.GetComponent<LineController>();
        gameActive = false;
        swingIndicatorSpawned = false;
        hasSwungRod = false;
        canvas.SetActive(gameActive);
    }

    void Update()
    {
        if(currentSwingIndicator != null)
        {
            currentSwingIndicator.GetComponent<LineRenderer>().SetPosition(0, currentSwingIndicator.transform.position);
            currentSwingIndicator.GetComponent<LineRenderer>().SetPosition(1, line.transform.position);
        }
    }    
    void FixedUpdate()
    {
        if(line.GetWater())
        {
            if(!gameActive)
            {
                gameActive = true;
                canvas.SetActive(gameActive);
            }
            if(fish.GetPercentageCaught() > 0.2 && hasSwungRod == false)
            {
                if(swingIndicatorSpawned == false)
                {
                    currentSwingIndicator = VRCInstantiate(swingIndicator);
                    currentSwingIndicator.transform.position = line.transform.position + new Vector3(Random.value * swingRadius + swingRadius/5, Random.value * swingRadius + swingRadius/5, Random.value * swingRadius + swingRadius/5);
                    swingIndicatorSpawned = true;
                }
                if(currentSwingIndicator.GetComponent<SwingIndicatorController>().GetCollidedWithLine())
                {
                    hasSwungRod = true;
                    Destroy(currentSwingIndicator);
                }
            }
            else
            {
                fish.Move(boardSize, bounds);
                player.Move(boardSize, bounds, line.GetReeling());
                if(Mathf.Abs(fish.GetPosition() - player.GetPosition()) < bounds)
                {
                    fish.AddCaught();
                }
                else
                {
                    fish.AddEscape();
                }
                if(fish.GetPercentageCaught() == 1)
                {
                    Start();
                    line.ResetLine();
                    return;
                }
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

        if(swingIndicatorSpawned && !hasSwungRod)
        {
            progressUI.transform.GetChild(3).gameObject.SetActive(true);
        }
        else
        {
            progressUI.transform.GetChild(3).gameObject.SetActive(false);
        }
    }

    public float GetPercentageCaught()
    {
        return fish.GetPercentageCaught();
    }
}
