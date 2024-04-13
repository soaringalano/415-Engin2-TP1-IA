using MBT;
using System.Linq;
using UnityEngine;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Engin2/Get closest collectible")]
    public class GetClosestCollectible : Leaf
    {
        [Space]
        [SerializeField]
        private Vector2Reference m_closestCollectiblePos = new Vector2Reference();
        [SerializeField]
        private TransformReference m_workerTransform = new TransformReference();
        [SerializeField]
        private IntReference m_collectibleToAssign = new IntReference();

        public override NodeResult Execute()
        {
            if (TeamOrchestrator._Instance.KnownCollectibles.Count == 0)
            {
                //On n'a pas trouvé de collectible. On retourne sans avoir updaté
                return NodeResult.failure;
            }

            Collectible closestCollectible = TeamOrchestrator._Instance.KnownCollectibles.Values.First();
            float closestDistance = Vector3.Distance(closestCollectible.transform.position, m_workerTransform.Value.position);

            foreach (var collectible in TeamOrchestrator._Instance.KnownCollectibles)
            {
                Collectible c = collectible.Value;
                float distance = Vector3.Distance(c.transform.position, m_workerTransform.Value.position);
                if (closestDistance > distance)
                {
                    closestCollectible = c;
                    closestDistance = distance;
                }
            }

            //Ceci est le camp le plus près. On update sa valeur dans le blackboard et retourne true
            m_closestCollectiblePos.Value =
                new Vector2(closestCollectible.transform.position.x, closestCollectible.transform.position.y);
            m_collectibleToAssign.Value = closestCollectible.m_collectibleId;
            return NodeResult.success;
        }
    }
}

[AddComponentMenu("")]
[MBTNode("Example/Set Random Position", 500)]
public class SetRandomPosition : Leaf
{
    public Bounds bounds;
    public Vector3Reference blackboardVariable = new Vector3Reference(VarRefMode.DisableConstant);

    public override NodeResult Execute()
    {
        // Random values per component inside bounds
        blackboardVariable.Value = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
        return NodeResult.success;
    }
}