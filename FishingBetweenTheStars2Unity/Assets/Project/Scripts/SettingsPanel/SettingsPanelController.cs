
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
    public Slider maxFallSpeedSlider;
    public Text maxFallSpeedText;
    public Slider accelSlider;
    public Text accelText;
    public Slider gravitySlider;
    public Text gravityText;
    public Slider bounceSlider;
    public Text bounceText;
    public Slider boundAdjustmentSlider;
    public Text boundAdjustmentText;
    public Text fixedUpdateRate;
    public Text fishGameTicks;
    [Header("Required prefabs")]
    public GameObject fishingRod;
    public GameObject currentFishingRod;
    public FishingGameController fishingGameController;
    void FixedUpdate()
    {
        timeStepRatio = Time.fixedDeltaTime/DEFAULT_TIME_STEP;
        maxSpeedText.text = ""+maxSpeedSlider.value;
        maxFallSpeedText.text = ""+maxFallSpeedSlider.value;
        accelText.text = ""+accelSlider.value;
        gravityText.text = ""+gravitySlider.value;
        bounceText.text = ""+bounceSlider.value;
        boundAdjustmentText.text = ""+boundAdjustmentSlider.value;
        if(fishingGameController != null) 
        {
            fishGameTicks.text = ""+timeStepRatio;
            fixedUpdateRate.text = ""+Time.fixedDeltaTime / timeStepRatio;
        }
    }

    // overriding onpickup because buttons are really stupid
    public void CreateRod()
    {
        Destroy(currentFishingRod);
        currentFishingRod = VRCInstantiate(fishingRod);
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
