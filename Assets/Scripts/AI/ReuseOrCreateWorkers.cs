using MBT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[MBTNode("Engin2/Reuse Or Create Workers")]
[AddComponentMenu("")]
public class ReuseOrCreateWorkers : Leaf
{

    private const int m_maxWorkerCount = 40;

    [SerializeField]
    private int m_minWorkerPerCollective = 2;

    [SerializeField]
    private int m_minScouters = 3;

    [SerializeField]
    public GameObject m_workerPrefab;

    [SerializeField]
    public IntReference m_collectibleToAssign;

    [SerializeField]
    public IntReference m_campToAssign;

    public override NodeResult Execute()
    {

        List<Worker> usableWorker = new List<Worker>();

        foreach(var item in TeamOrchestrator._Instance.WorkersList)
        {
            Worker worker = item.Value;
            if(!DispatchCenter._Instance.IsWorkerBusy(worker.m_workerId))
            {
                usableWorker.Add(worker);
            }
        }

        if(m_campToAssign == null || m_campToAssign.Value < 0)
        {
            Debug.LogError("Camp to assign value error");
            return NodeResult.failure;
        }
        Camp camp = TeamOrchestrator._Instance.Camps[m_campToAssign.Value];
        Collectible collectible = TeamOrchestrator._Instance.KnownCollectibles[m_collectibleToAssign.Value];

        List<Worker> workerList;

        if(usableWorker.Count <= m_minScouters)
        {
            //no usable worker, must create new workers
            workerList = CreateWorkers(m_minWorkerPerCollective, m_collectibleToAssign.Value, camp);
        }
        else if(usableWorker.Count < m_minScouters + m_minWorkerPerCollective)
        {
            workerList =
                CreateWorkers(m_minWorkerPerCollective + m_minScouters - usableWorker.Count,
                              m_collectibleToAssign.Value, camp);
            List<Worker> sublist = PickWorkers(usableWorker, m_minWorkerPerCollective - workerList.Count, camp);
            if(sublist != null)
            {
                foreach(Worker w in  sublist)
                {
                    workerList.Add(w);
                }
            }
        }
        else
        {
            workerList = PickWorkers(usableWorker, m_minWorkerPerCollective, camp);
        }
        if(workerList == null)
        {
            Debug.LogError("Create worker failed, something is wrong");
            return NodeResult.failure;
        }
        int counter = 0;
        foreach(Worker worker in workerList)
        {
            worker.m_myWaitTime = counter++ * 5.01f;
            DispatchCenter._Instance.AssignNewWorks(worker, camp, collectible);
        }
        return NodeResult.success;
    }

    public List<Worker> CreateWorkers(int workerNumber, int collectibleId, Camp camp)
    {
        if(workerNumber <= 0 || TeamOrchestrator._Instance.WorkersList.Count >= m_maxWorkerCount)
        {
            return null;
        }

        List<Worker> ret = new List<Worker>();
        for(int i=0;i<workerNumber;i++)
        {
            if (TeamOrchestrator._Instance.WorkersList.Count >= m_maxWorkerCount)
                break;
            GameObject go = Instantiate(m_workerPrefab, Vector3.zero, Quaternion.identity);
            go.transform.SetParent(GameObject.Find("_TeamOrchestrator").transform);
            Worker worker = go.GetComponent<Worker>();
            worker.SetWorkerState(Worker.STATE_WAITING);
            worker.m_isInScouting = false;
            ret.Add(worker);
            //TeamOrchestrator._Instance.WorkersList.Add(worker.m_workerId, worker);
            TeamOrchestrator._Instance.OnWorkerCreated();
        }
        return ret;
    }

    public List<Worker> PickWorkers(List<Worker> usableWorkers, int workerNumber, Camp camp)
    {
        if (workerNumber <= 0)
            return null;
        /*SortedList sortedList = new SortedList();
        foreach(Worker worker in usableWorkers)
        {
            Vector3 pos = worker.transform.position;
            float distance = Vector3.Distance(pos, camp.transform.position);
            sortedList.Add(distance, worker);
        }
        List<Worker> ret = new List<Worker>();
        foreach(DictionaryEntry entry in sortedList)
        {
            if(ret.Count >= workerNumber)
            {
                break;
            }
            Worker w = (Worker)entry.Value;
            w.m_isInScouting = false;
            w.SetWorkerState(Worker.STATE_WAITING);
            ret.Add(w);
        }*/
        List<Worker> ret = new List<Worker>();
        for(int i=workerNumber-1;i>=0;i--)
        {
            ret.Add(usableWorkers[i]);
        }
        return ret;
    }

}
