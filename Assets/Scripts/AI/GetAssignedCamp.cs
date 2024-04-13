using MBT;
using UnityEngine;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Engin2/Get Assigned Camp")]
    public class GetAssignedCamp: Leaf
    {
        [Space]
        [SerializeField]
        private Vector2Reference m_targetPosition2D = new Vector2Reference();
        [SerializeField]
        private TransformReference m_workerTransform;// = new TransformReference();

        public override NodeResult Execute()
        {
            if (TeamOrchestrator._Instance.Camps.Count == 0)
            {
                return NodeResult.failure;
            }

            Worker worker = m_workerTransform.Value.GetComponent<Worker>();

            int campId = DispatchCenter._Instance.GetAssignedCamp(worker.m_workerId);
            if(campId < 0)
            {
                return NodeResult.failure;
            }
            Camp camp = TeamOrchestrator._Instance.Camps[campId];
            m_targetPosition2D.Value = 
                new Vector2(camp.transform.position.x, camp.transform.position.y);

            return NodeResult.success;
        }
    }
}
