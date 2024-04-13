using MBT;
using UnityEngine;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Engin2/Get Assigned Collectible")]
    public class GetAssignedCollectible : Leaf
    {
        [Space]
        [SerializeField]
        public Vector2Reference m_assignedCollectible = new Vector2Reference();
        
        [SerializeField]
        private TransformReference m_workerTransform;// = new TransformReference();

        public override NodeResult Execute()
        {
            if (TeamOrchestrator._Instance.KnownCollectibles.Count == 0)
            {
                return NodeResult.failure;
            }

            Worker worker = m_workerTransform.Value.GetComponent<Worker>();

            int collectibleId = DispatchCenter._Instance.GetAssignedCollectible(worker.m_workerId);

            if (collectibleId >= 0)
            {
                Collectible collectible = TeamOrchestrator._Instance.KnownCollectibles[collectibleId];
                m_assignedCollectible.Value =
                    new Vector2(collectible.transform.position.x, collectible.transform.position.y);
                return NodeResult.success;
            }
            return NodeResult.failure;
        }
    }
}
