
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class HookController : UdonSharpBehaviour
{
    public FishingGameController gameController;
    private LineController line;
    void Start()
    {
        line = GetComponentInParent<LineController>();
    }

    void FixedUpdate()
    {
        if(transform.position.y < -5)
        {
            line.ResetLine();
            line.SetCast(false);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if(line.GetCast() && !line.GetWater())
        {
            if(collider.gameObject.name == "Water") 
            {
                line.SetInWater(true);
            }
            else
            {
                line.ResetLine();
                line.SetCast(false);
            }
        }
        else if(!line.GetCast())
        {
            if(collider.gameObject.name == "FishWorldObject" && Networking.GetOwner(collider.gameObject).playerId == Networking.LocalPlayer.playerId)
            {
                collider.GetComponent<FishWorldObject>().UsedAsBait();
                gameController.AddFishOnLine(collider.GetComponent<FishWorldObject>().GetFishData());
                collider.transform.parent.GetComponent<FishWorldObjectContainer>().EXUR_Finalize();
            }
        }
    }
}
