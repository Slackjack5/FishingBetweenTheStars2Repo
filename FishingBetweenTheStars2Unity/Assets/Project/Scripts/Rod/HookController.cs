
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class HookController : UdonSharpBehaviour
{
    private LineController line;
    void Start()
    {
        line = GetComponentInParent<LineController>();
    }

    void FixedUpdate()
    {
        if(transform.position.y < -10)
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
    }
}
