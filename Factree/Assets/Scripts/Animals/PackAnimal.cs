using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackAnimal : MovingAnimal
{
    public int carryingCapacity;

    public GarbageTileType lookingFor;

    public float busyTime = 2f;

    private ResourceItem carrying;

    protected override IEnumerator DoBehaviour()
    {
        if (grid.cityGrid == null) yield return null; // Wait until the map is generated

        var home = grid.cityGrid.GetGridObject(startingPosition.x, startingPosition.y).PlantTile;

        focus = RandomResourceOfType(lookingFor);


        if (!home.CheckUpkeep())//<-- TODO: change to event based
        {
            yield return DoMoveToGridPosition(startingPosition);
        }
        else
        {
            yield return DoMoveToGridPosition(focus, AnimalState.Busy);


            
            if (GetGarbageTypeAt(focus) == lookingFor)
            {
                GarbageSO resourceTile = grid.cityGrid.GetGridObject(focus.x, focus.y).Resource;

                if (resourceTile.resource.count > 0)
                {

                    currentState = AnimalState.Busy;

                    yield return new WaitForSeconds(busyTime);

                    //TODO: subtract from garbage on tile. Cant do it from GarbageSO. This requires we have a system that stores recources on tiles.
                    
                    carrying = new ResourceItem(resourceTile.resource.resourceType, carryingCapacity);//TODO: limit resources gathered to what was available on the tile

                    if (carrying.count > 0)
                    {
                        yield return DoMoveToGridPosition(startingPosition);

                        ResourceManager.Instance.AddResourceAmountByType(carrying.resourceType, carrying.count);

                        carrying.count = 0;
                    }
                }
            }
        }
        
        currentState = AnimalState.Idle;
    }
}
