using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/CollectResourceAbility")]
public class CollectResourceAbility : ScriptableObject
{
    public GarbageSO requiredGarbage;
    public float radius;
    public float cooldownTime;
    public List<ResourceItem> startResourceChange;
    public List<ResourceItem> perSecondResourceChange;
    public int garbageSubtracted;
    public int totalSeconds;


    public void DoAbility(int x, int y, int targetX, int targetY)
    {
        Debug.Log("Do Ability");
        GridManagement gm = FindObjectOfType<GridManagement>();
        GarbageSO objectAtLocation = gm.cityGrid.GetGridObject(targetX, targetY).Resource;
        Debug.Log(objectAtLocation);
        Debug.Log(objectAtLocation);
        if (objectAtLocation == requiredGarbage)
        {
            if(Vector2.Distance(new Vector2(x,y), new Vector2(targetX, targetY)) <= radius)
            {

                foreach (ResourceItem r in startResourceChange)
                {
                    ResourceManager.Instance.AddResourceAmountByType(r.resourceType, r.count);
                }

                foreach (ResourceItem r in perSecondResourceChange)
                {
                    Debug.Log("Resource per sec");
                    ResourceManager.Instance.AddTimeResourceChange(r.resourceType, r.count, totalSeconds);
                }

                objectAtLocation.SubtractResource(garbageSubtracted);
            }
            
        }  
    }
}
