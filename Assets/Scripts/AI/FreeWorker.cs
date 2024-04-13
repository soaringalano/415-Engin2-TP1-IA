using MBT;
using System.Collections.Generic;
using UnityEngine;

[MBTNode("Engin2/Free Worker")]
[AddComponentMenu("")]
public class FreeWorker : Leaf
{

    [SerializeField]
    private TransformReference m_workerTransform;

    public override NodeResult Execute()
    {
        Worker worker = m_workerTransform.Value.GetComponent<Worker>();
        if(worker != null)
        {
            DispatchCenter._Instance.ReleaseWorker(worker.m_workerId);
            return NodeResult.success;
        }
        return NodeResult.success;
    }

}
