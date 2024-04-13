using MBT;
using System.Collections.Generic;
using UnityEngine;

[MBTNode("Engin2/Not All Workers Ready")]
[AddComponentMenu("")]
public class NotAllWorkersReadyCondition : Condition
{

    [SerializeField]
    private TransformReference m_workerTransform = new TransformReference();

    public override bool Check()
    {
        Worker worker = m_workerTransform.Value.GetComponent<Worker>();


        if (worker != null)
        {
            int collectibleId = DispatchCenter._Instance.GetAssignedCollectible(worker.m_workerId);
            int campId = DispatchCenter._Instance.GetAssignedCamp(worker.m_workerId);
            if(campId < 0 || collectibleId < 0)
            {
                return false;
            }
            Camp camp = TeamOrchestrator._Instance.Camps[campId];
            if (collectibleId >= 0)
            {
                List<int> workers = DispatchCenter._Instance.GetAssignedWorkerOfCollectible(collectibleId);
                if (workers.Count > 0)
                {

                    bool ret = false;
                    foreach(int workerId in workers)
                    {
                        Worker w = TeamOrchestrator._Instance.WorkersList[workerId];
                        if (w != null)
                        {
                            if(w.transform.position != camp.transform.position)
                            {
                                ret |= true;
                            }
                            else
                            {
                                ret |= false;
                            }
                        }
                    }
                    return ret;
                }
            }
        }

        return false;
    }

}
