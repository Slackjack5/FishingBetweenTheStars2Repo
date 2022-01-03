
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;


public class RodPool : UdonSharpBehaviour
{
    private float poolSize; // size of RodPool, determined by number of children of the pool
    void Start()
    {
        poolSize = transform.childCount;
    }

    // returns a rod from the pool, or null if there are no inactive objects left
    public GameObject GetRod()
    {
        for(int i = 0; i < poolSize; i++)
        {
            GameObject currentObject = transform.GetChild(i).gameObject;
            if(!currentObject.GetComponent<RodContainerController>().isRodActive())
            {
                currentObject.transform.SetPositionAndRotation(transform.position, transform.rotation);
                currentObject.GetComponent<RodContainerController>().SetRodActive(true);
                return currentObject;
            }
        }

        return null;
    }

    public void ReturnToPool(GameObject obj)
    {
        if(obj == null) return;
        obj.GetComponent<RodContainerController>().ResetRod();
    }
}
