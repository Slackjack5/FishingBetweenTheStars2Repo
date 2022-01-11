
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class HookController : UdonSharpBehaviour
{
    public FishingGameController gameController;
    public GameObject fishSprite;
    private LineController line;
    private GameObject currentFishSprite;
    void Start()
    {
        line = GetComponentInParent<LineController>();
    }

    void FixedUpdate()
    {
        if(transform.position.y < -2 && line.GetCast())
        {
            line.SetInWater(true);
        }

        /*if(transform.position.y < -5 && line.GetCast())
        {
            line.ResetLine();
            line.SetCast(false);
        }*/

        if(line.GetCast() && !line.GetWater() && currentFishSprite != null)
        {
            currentFishSprite.SetActive(false);
        }
        else if(!line.GetCast() && !line.GetWater() && currentFishSprite != null)
        {
            currentFishSprite.SetActive(true);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        /*if(line.GetCast() && !line.GetWater())
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
        }*/
        if(!line.GetCast())
        {
            if(collider.gameObject.name == "FishWorldObject" && Networking.GetOwner(collider.gameObject).playerId == Networking.LocalPlayer.playerId)
            {
                collider.GetComponent<FishWorldObject>().UsedAsBait();
                gameController.AddFishOnLine(collider.GetComponent<FishWorldObject>().GetFishData());
                currentFishSprite = VRCInstantiate(fishSprite);
                currentFishSprite.transform.SetParent(transform);
                currentFishSprite.transform.localPosition = new Vector3(0, -0.001f, 0);
                currentFishSprite.GetComponent<SpriteRenderer>().sprite = collider.GetComponent<FishWorldObject>().GetFishData().getFishSprite();
                collider.transform.parent.GetComponent<FishWorldObjectContainer>().EXUR_Finalize();
            }
        }
    }

    /*void OnCollisionEnter(Collision collision)
    {
        if(!line.GetWater())
        {
            line.ResetLine();
            line.SetCast(false);
        }
    }*/

    public GameObject GetFishSprite()
    {
        return currentFishSprite;
    }

    public void SetFishSprite(GameObject fishSprite)
    {
        currentFishSprite = fishSprite;
    }
}
