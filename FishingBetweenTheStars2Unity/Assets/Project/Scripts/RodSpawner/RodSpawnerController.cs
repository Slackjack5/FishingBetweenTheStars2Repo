
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class RodSpawnerController : UdonSharpBehaviour
{
    public Manager manager;
    public float baseHeight;
  public GameObject resetPoint;

  private void Start()
  {
    transform.position = resetPoint.transform.position;
    transform.rotation = resetPoint.transform.rotation;
  }
  public override void OnPickup()
    {

    }

    public override void OnDrop()
    {
      GameObject existingObject = manager.AcquireGameObjectWithTag("" + Networking.LocalPlayer.playerId);
      if (existingObject != null)
      {
        existingObject.GetComponent<RodContainerController>().ResetRod();
        existingObject.GetComponent<RodContainerController>().EXUR_Reinitialize();
      }
      else
      {
        manager.AcquireObjectForEachPlayer();
        existingObject = manager.AcquireGameObjectWithTag("" + Networking.LocalPlayer.playerId);
      }
    //Vector3 boneRotations = Networking.LocalPlayer.GetBoneRotation(HumanBodyBones.Hips).eulerAngles;
    //Vector3 playerForward = new Vector3(Mathf.Cos(boneRotations.y), 0, Mathf.Sin(boneRotations.y));
      existingObject.transform.GetChild(0).position = gameObject.transform.position;//Networking.LocalPlayer.GetBonePosition(HumanBodyBones.Hips);
      existingObject.transform.GetChild(0).rotation = gameObject.transform.rotation; //Networking.LocalPlayer.GetBoneRotation(HumanBodyBones.Hips);
    gameObject.transform.position = resetPoint.transform.position;
    gameObject.transform.rotation = new Quaternion(0,0,0,0);
    //existingObject.transform.GetChild(0).position += existingObject.transform.GetChild(0).forward;
  }
  }
