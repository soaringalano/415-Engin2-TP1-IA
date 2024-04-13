using MBT;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode("Engin2/Wait My Turn")]
public class WaitMyTurn : Leaf
{


    [SerializeField]
    private TransformReference m_workerTransform;

    public override NodeResult Execute()
    {
        Worker worker = m_workerTransform.Value.GetComponent<Worker>();
        if(worker != null)
        {
            if(worker.m_myWaitTime > 0)
            {
                worker.m_myWaitTime -= Time.fixedDeltaTime;
                return NodeResult.success;
            }
            else
            {
                worker.SetWorkerState(Worker.STATE_WORKING);
                return NodeResult.failure;
            }
        }
        return NodeResult.failure;
    }

}
