using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeAnimal : MovingAnimal
{
    public GameObject grassGrower;
    public float behaviourRadius;

    protected override IEnumerator DoBehaviour()
    {
        if (grid.cityGrid == null) yield return null; // Wait until the map is generated

        var beeTree = grid.cityGrid.GetGridObject(startingPosition.x, startingPosition.y).PlantTile;

        focus = RandomTileOfType(BaseTileType.Soil);

        if (!beeTree.CheckUpkeep())//<-- TODO: change to event based
        {
            yield return DoMoveToGridPosition(startingPosition);
        }
        else
        {
            yield return DoMoveToGridPosition(focus);


            if (GetTileTypeAt(focus) == BaseTileType.Soil)
            {
                currentState = AnimalState.Busy;

                bool isGrassGrown = false;
                Instantiate(grassGrower, objectGrid.CellToWorld(focus), Quaternion.identity, null)
                    .GetComponent<DestroySelfTimer>().onDestroy += () =>
                    {
                        isGrassGrown = true;
                    };

                while(!isGrassGrown)
                {
                    yield return new WaitForSeconds(0.5f);


                    yield return DoMoveToWorldPosition(objectGrid.CellToWorld(focus) + 
                        new Vector3(
                            Random.Range(-behaviourRadius, behaviourRadius),
                            Random.Range(-behaviourRadius, behaviourRadius)
                        ), 
                        AnimalState.Busy);

                }
            }

            //yield return new WaitForSeconds(grassGrower.GetComponent<DestroySelfTimer>().delay);

            yield return DoMoveToGridPosition(startingPosition);

        }

    }
}
