
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
    public bool simulateGame; // whether or not to simulate the actual game or use the settings panel
    [Header("Fish variables")]
    public int fishType; // type of fish
    public float difficulty; // difficulty from 0 to 1
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
    public FishDictionary fishDictionary; // fish dictionary script
    public InventoryTab inventory;

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
    private FishData fishData;
    private FishData fishOnLine;
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
        fish.CreateFish(fishType, difficulty, timeStepRatio);
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
                if(simulateGame)
                {
                    if(Random.value > 0.995)
                    {
                        if(fishOnLine != null)
                        {
                            fishData = fishDictionary.rollFish(fishDictionary.getTierFromPower(fishOnLine.getFishValue()));
                        }
                        else
                        {
                            fishData = fishDictionary.rollFish(0);
                        }
                        fish.CreateFish(fishData.getFishType(), fishData.getFishDifficulty(), timeStepRatio);
                        gameActive = true;
                        canvas.SetActive(gameActive);
                    }
                }
                else
                {
                    gameActive = true;
                    canvas.SetActive(gameActive);
                }
            }
            else
            {
                if(fish.GetPercentageCaught() > 0.5 && hasSwungRod == false)
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
                        inventory.AddToBag(fishData.getFishId() + 1);
                        line.ResetLine();
                        fishOnLine = null;
                        Start();
                        return;
                    }
                    if(fish.GetPercentageCaught() == 0)
                    {
                        line.ResetLine();
                        fishOnLine = null;
                        Start();
                        return;
                    }
                }
                UpdateUI();
            }
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

    public FishData GetFishData()
    {
        return fishData;
    }

    public void AddFishOnLine(FishData fish)
    {
        fishOnLine = fish;
    }
}
