using MBT;
using UnityEngine;
using UnityEngine.UIElements;


/**
 * This part is optimized by Mao.
 * 
 * The position of the new camp aims to optimize the efficiency of the collection of resources,
 * so it depends on the following factors:
 * 1. Distances between current position and surrounding resources, here I choose square means of the distances,
 *    this factor determines how many seconds it takes a worker to carry resources from collectible position to camp position, etc.
 * 2. Distances between current position and other camps: this is to avoid continuous creating camps.
 * 3. Owned resources amount: the amount of owned resources should be greater than the amount needed to build a camp, but should not affect too much
 *    the final score.
 * 4. Total amount of workers: there should be a balance point between the amount of camps and amount of workers;
 * 
 * Now, all factor aims to one goal: maximize the final score. 
 * We first begin from average distance and standard variance of the distances between current position and surrounding resources.
 * 
 */ 

//[AddComponentMenu("")]
//[MBTNode(name = "Engin2/Can Place Camp")]
//Deprecated, no more needed
public class CanPlaceCamp : Condition
{

    public override bool Check()
    {
        if(TeamOrchestrator._Instance.KnownCollectibles.Count == 0 )
        {
            return false;
        }

        bool canPlace = TeamOrchestrator._Instance.CanPlaceObject(new Vector2(transform.position.x, transform.position.y));

        if(TeamOrchestrator._Instance.Camps.Count == 0 )
        {
            canPlace = false;// No camp, then need generate by a bit of calcul.
        }
        else
        {
            // if already have a camp, then need some calcul. for now return false
            // TODO
            canPlace &= false;
        }

        return canPlace;
    }

    //Get nearest camp position to avoid build another camp side by side.
    /*private ObjectInfo GetNearestCampInfo()
    {
        Vector3 position;
        float distance;
        if (TeamOrchestrator._Instance.Camps.Count == 0)
        {
            //On n'a pas trouvé de camp. On retourne faux
            position = new Vector3(0, 0, 0);
            return null;
        }
        else
        {
            position = TeamOrchestrator._Instance.Camps[0].transform.position;
        }

        distance = Vector3.Distance(position, m_workerTransform.Value.position);

        foreach (var camp in TeamOrchestrator._Instance.Camps)
        {
            float tempDist = Vector3.Distance(camp.transform.position, m_workerTransform.Value.position);
            if ( distance > tempDist)
            {
                distance = tempDist;
                position = camp.transform.position;
            }
        }
        ObjectInfo campInfo = new ObjectInfo();
        campInfo.m_distance = distance;
        campInfo.m_position = position;
        return campInfo;
    }

    private class ObjectInfo
    {

        public float m_distance;

        public Vector3 m_position;

    }*/

}