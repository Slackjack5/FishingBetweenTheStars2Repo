
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
    void OnCollisionEnter(Collision collision)
    {
        if(line.GetCast() && !line.GetWater())
        {
            if(collision.gameObject.name == "Water") 
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
