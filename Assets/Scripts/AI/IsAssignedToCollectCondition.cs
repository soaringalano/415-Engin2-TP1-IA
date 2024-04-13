using MBT;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Engin2/Is Assigned To Collect")]
public class IsAssignedToCollectCondition : Condition
{

    [SerializeField]
    private TransformReference m_workerTransform;// = new TransformReference();

    public override bool Check()
    {
        Worker worker = m_workerTransform.Value.GetComponent<Worker>();
        if(worker != null)
        {
            return DispatchCenter._Instance.IsWorkerBusy(worker.m_workerId);
        }

        return false;
    }

}
