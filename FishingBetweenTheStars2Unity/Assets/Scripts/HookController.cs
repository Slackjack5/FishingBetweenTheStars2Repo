
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class HookController : UdonSharpBehaviour
{
    private Rigidbody rigidbody;
    private LineController line;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        line = GetComponentInParent<LineController>();
    }
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "CrimsonLayer1") 
        {
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            line.SetInWater(true);
        }
        else
        {
            line.ResetLine();
            line.SetCast(false);
        }
    }
}
