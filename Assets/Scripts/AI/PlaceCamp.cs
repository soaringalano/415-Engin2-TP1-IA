using MBT;
using UnityEngine;

[MBTNode("Engin2/Place Camp")]
[AddComponentMenu("")]
public class PlaceCamp : Leaf
{
    [SerializeField]
    private GameObject m_campPrefab;

    [SerializeField]
    private IntReference m_campToAssign = new IntReference();

    [SerializeField]
    private TransformReference m_workerTransform;

    public override NodeResult Execute()
    {
        Camp camp = Instantiate(m_campPrefab, transform.position, Quaternion.identity).GetComponent<Camp>();
        TeamOrchestrator._Instance.OnCampPlaced();
        m_campToAssign.Value = camp.m_campId;

        Worker worker = m_workerTransform.Value.GetComponent<Worker>();
        if(worker != null)
        {
            worker.SetWorkerState(Worker.STATE_SCOUTING);
        }

        return NodeResult.success;

    }
}
