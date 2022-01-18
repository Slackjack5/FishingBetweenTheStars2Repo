
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
    [Header("Required prefabs")]
    public GameObject rodPoolGameObject;
    public Manager rodManager;
    
    void Start()
    {
        OnMaxSpeedChanged();
        OnMaxFallSpeedChanged();
        OnAccelChanged();
        OnGravityChanged();
        OnBounceChanged();
        OnBoundsChanged();
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
    }

    public void CreateRod()
    {
        /*GameObject existingObject = rodManager.AcquireGameObjectWithTag(""+Networking.LocalPlayer.playerId);
        if(existingObject != null)
        {
            existingObject.GetComponent<RodContainerController>().ResetRod();
            existingObject.GetComponent<RodContainerController>().EXUR_Reinitialize();
            existingObject.transform.GetChild(0).position = rodManager.gameObject.transform.position;
        }
        else
        {
           rodManager.AcquireObjectForEachPlayer(); 
        }*/
    }
}
