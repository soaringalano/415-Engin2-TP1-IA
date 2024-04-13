using MBT;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Engin2/Need Waiting")]
public class NeedWaiting : Condition
{

    [SerializeField]
    private TransformReference m_workerTransform;

    public override bool Check()
    {
        Worker worker = m_workerTransform.Value.GetComponent<Worker>();
        if (worker != null)
        {
            return worker.NeedWait();
        }
        return false;
    }
}
