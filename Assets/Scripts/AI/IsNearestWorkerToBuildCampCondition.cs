using MBT;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Engin2/Is Nearest Worker To Build Camp")]
public class IsNearestWorkerToBuildCampCondition : Condition
{

    [SerializeField]
    private TransformReference m_agentTransform;

    public override bool Check()
    {
        Worker worker = m_agentTransform.Value.GetComponent<Worker>();
        return worker.GetWorkerState() == Worker.STATE_BUILDING;
    }

}
