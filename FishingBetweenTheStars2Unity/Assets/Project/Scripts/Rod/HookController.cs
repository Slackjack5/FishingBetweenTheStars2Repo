
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class HookController : UdonSharpBehaviour
{
    private Rigidbody hookRB;
    private LineController line;
    void Start()
    {
        hookRB = GetComponent<Rigidbody>();
        line = GetComponentInParent<LineController>();
    }
    void OnCollisionEnter(Collision collision)
    {
        if(line.GetCast())
        {
            if(collision.gameObject.name == "Water") 
            {
                hookRB.constraints = RigidbodyConstraints.FreezeAll;
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
