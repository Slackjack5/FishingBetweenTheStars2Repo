
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

public class SettingsPanelController : UdonSharpBehaviour
{
    private const float DEFAULT_TIME_STEP = 0.02f;
    private float timeStepRatio;
    [Header("UI elements")]
    public Slider maxSpeedSlider;
    public Text maxSpeedText;
    [UdonSynced] private float maxSpeedValue;
    public Slider maxFallSpeedSlider;
    public Text maxFallSpeedText;
    [UdonSynced] private float maxFallSpeedValue;
    public Slider accelSlider;
    public Text accelText;
    [UdonSynced] private float accelValue;
    public Slider gravitySlider;
    public Text gravityText;
    [UdonSynced] private float gravityValue;
    public Slider bounceSlider;
    public Text bounceText;
    [UdonSynced] private float bounceValue;
    public Slider boundAdjustmentSlider;
    public Text boundAdjustmentText;
    [UdonSynced] private float boundAdjustmentValue;
    public Text fixedUpdateRate;
    public Text fishGameTicks;
    [Header("Required prefabs")]
    public GameObject rodPoolGameObject;
    private GameObject currentFishingRod;
    private FishingGameController fishingGameController;
    private RodPool rodPool;
    
    void Start()
    {
        timeStepRatio = Time.fixedDeltaTime/DEFAULT_TIME_STEP;
        rodPool = rodPoolGameObject.GetComponent<RodPool>();
        OnMaxSpeedChanged();
        OnMaxFallSpeedChanged();
        OnAccelChanged();
        OnGravityChanged();
        OnBounceChanged();
        OnBoundsChanged();
    }
    void FixedUpdate()
    {
        if(fishingGameController != null) 
        {
            fishGameTicks.text = ""+timeStepRatio;
            fixedUpdateRate.text = ""+Time.fixedDeltaTime / timeStepRatio;
        }
    }

    public void OnMaxSpeedChanged()
    {
        maxSpeedText.text = ""+maxSpeedSlider.value;
        maxSpeedValue = maxSpeedSlider.value;
    }

    public void OnMaxFallSpeedChanged()
    {
        maxFallSpeedText.text = ""+maxFallSpeedSlider.value;
        maxFallSpeedValue = maxFallSpeedSlider.value;
    }

    public void OnAccelChanged()
    {
        accelText.text = ""+accelSlider.value;
        accelValue = accelSlider.value;
    }

    public void OnGravityChanged()
    {
        gravityText.text = ""+gravitySlider.value;
        gravityValue = gravitySlider.value;
    }

    public void OnBounceChanged()
    {
        bounceText.text = ""+bounceSlider.value;
        bounceValue = bounceSlider.value;
    }

    public void OnBoundsChanged()
    {
        boundAdjustmentText.text = ""+boundAdjustmentSlider.value;
        boundAdjustmentValue = boundAdjustmentSlider.value;
    }

    public override void OnDeserialization()
    {
        maxSpeedText.text = ""+maxSpeedValue;
        maxSpeedSlider.value = maxSpeedValue;
        maxFallSpeedText.text = ""+maxFallSpeedValue;
        maxFallSpeedSlider.value = maxFallSpeedValue;
        accelText.text = ""+accelValue;
        accelSlider.value = accelValue;
        gravityText.text = ""+gravityValue;
        gravitySlider.value = gravityValue;
        bounceText.text = ""+bounceValue;
        bounceSlider.value = bounceValue;
        boundAdjustmentText.text = ""+boundAdjustmentValue;
        boundAdjustmentSlider.value = boundAdjustmentValue;
        fishingGameController = currentFishingRod.GetComponentInChildren<FishingGameController>();
        fishingGameController.maxSpeed = maxSpeedSlider.value;
        fishingGameController.maxFallSpeed = maxFallSpeedSlider.value;
        fishingGameController.accel = accelSlider.value;
        fishingGameController.gravity = gravitySlider.value;
        fishingGameController.bounce = bounceSlider.value;
        fishingGameController.boundAdjustment = boundAdjustmentSlider.value;
        fishingGameController.Start();
    }

    // overriding onpickup because buttons are really stupid
    public void CreateRod()
    {
        rodPool.ReturnToPool(currentFishingRod);
        currentFishingRod = rodPool.GetRod();
        if(currentFishingRod != null)
        {
            fishingGameController = currentFishingRod.GetComponentInChildren<FishingGameController>();
            fishingGameController.maxSpeed = maxSpeedSlider.value;
            fishingGameController.maxFallSpeed = maxFallSpeedSlider.value;
            fishingGameController.accel = accelSlider.value;
            fishingGameController.gravity = gravitySlider.value;
            fishingGameController.bounce = bounceSlider.value;
            fishingGameController.boundAdjustment = boundAdjustmentSlider.value;
            fishingGameController.Start();
        }
    }
}
