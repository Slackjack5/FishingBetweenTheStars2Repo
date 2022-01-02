
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

public class SettingsPanelController : UdonSharpBehaviour
{
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
    [Header("Required prefabs")]
    public GameObject fishingRod;
    public GameObject currentFishingRod;
    public FishingGameController fishingGameController;
    void FixedUpdate()
    {
        maxSpeedText.text = ""+maxSpeedSlider.value;
        maxFallSpeedText.text = ""+maxFallSpeedSlider.value;
        accelText.text = ""+accelSlider.value;
        gravityText.text = ""+gravitySlider.value;
        bounceText.text = ""+bounceSlider.value;
        boundAdjustmentText.text = ""+boundAdjustmentSlider.value;
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
