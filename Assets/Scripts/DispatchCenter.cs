using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispatchCenter : MonoBehaviour
{

    public static DispatchCenter _Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        if (_Instance == null || _Instance == this)
        {
            _Instance = this;
            return;
        }
        Destroy(this);
    }


    /**
     * dictionary of collectible and workers who are working on the specified collectible
     * collectible : worker  -->  1 : n
     * key : collectible id
     * value : worker id list
     */
    private Dictionary<int, List<int>> m_collectible_workers = new Dictionary<int, List<int>>();

    /**
     * dictionary of worker and the collectible that it works on
     * worker : collectible --> n : 1
     * key : worker id
     * value : collectible id
     */
    private Dictionary<int, int> m_worker_collectible = new Dictionary<int, int>();

    private Dictionary<int, List<int>> m_camp_workers = new Dictionary<int, List<int>>();

    private Dictionary<int, int> m_worker_camp = new Dictionary<int, int>();

    public void AssignWorkerToCamp(int workerId, int campId)
    {
        lock (this)
        {
            if (campId < 0)
            {
                m_worker_camp[workerId] = campId;
                return;
            }
            if (m_camp_workers.ContainsKey(campId))
            {
                List<int> workers = m_camp_workers[campId];
                if (!workers.Contains(workerId))
                {
                    workers.Add(workerId);
                }
            }
            else
            {
                List<int> workers = new List<int>();
                workers.Add(workerId);
                m_camp_workers[campId] = workers;
            }

            if (!m_worker_camp.ContainsKey(workerId) ||
                m_worker_camp[workerId] == -1)
            {
                m_worker_camp[workerId] = campId;
            }
        }
    }

    public List<int> GetAssignedWorkerOfCamp(int campId)
    {
        if (m_camp_workers.ContainsKey(campId))
        {
            List<int> ret = m_camp_workers[campId];
            return ret;
        }
        return null;
    }

    public int GetAssignedCamp(int workerId)
    {
        if (m_worker_camp.ContainsKey(workerId))
        {
            return m_worker_camp[workerId];
        }
        return -1;
    }

    public void AssignWorkerToCollectible(int workerId, int collectibleId)
    {
        lock(this)
        {
            if(collectibleId < 0)
            {
                m_worker_collectible[workerId] = collectibleId;
                return;
            }
            if(m_collectible_workers.ContainsKey(collectibleId))
            {
                List<int> workers = m_collectible_workers[collectibleId];
                if(!workers.Contains(workerId))
                {
                    workers.Add(workerId);
                }
            }
            else
            {
                List<int> workers = new List<int>();
                workers.Add(workerId);
                m_collectible_workers[collectibleId] = workers;
            }

            if (!m_worker_collectible.ContainsKey(workerId) ||
                m_worker_collectible[workerId] == -1)
            {
                m_worker_collectible[workerId] = collectibleId;
            }
        }
    }

    public List<int> GetAssignedWorkerOfCollectible(int collectibleId)
    {
        if(m_collectible_workers.ContainsKey(collectibleId))
        {
            List<int> ret = m_collectible_workers[collectibleId];
            return ret;
        }
        return null;
    }

    public int GetAssignedCollectible(int workerId)
    {
        if(m_worker_collectible.ContainsKey(workerId))
        {
            return m_worker_collectible[workerId];
        }
        return -1;
    }

    public void ReleaseCollectible(int collectibleId)
    {
        if(!m_collectible_workers.ContainsKey(collectibleId))
        {
            return;
        }
        List<int> workers = new List<int>(m_collectible_workers[collectibleId]);
        if(workers != null)
        {
            foreach(int workerId in workers)
            {
                ReleaseWorker(workerId);
            }
        }
    }

    public void ReleaseWorker(int workerId)
    {
        lock (this)
        {
            int collectibleId = -1;
            int campId = -1;
            if(m_worker_collectible.ContainsKey(workerId) && m_worker_camp.ContainsKey(workerId))
            {
                collectibleId = m_worker_collectible[workerId];
                campId = m_worker_camp[workerId];
            }
            if(collectibleId < 0 && campId < 0)
            {
                return;
            }
            m_worker_collectible.Remove(workerId);
            m_worker_camp.Remove(workerId);
            if (m_collectible_workers.ContainsKey(collectibleId))
            {
                List<int> workers = m_collectible_workers[collectibleId];
                if (workers.Contains(workerId))
                {
                    workers.Remove(workerId);
                }
                if(workers.Count == 0)
                {
                    m_collectible_workers.Remove(collectibleId);
                }
            }

            if (m_camp_workers.ContainsKey(campId))
            {
                List<int> workers = m_camp_workers[campId];
                if (workers.Contains(workerId))
                {
                    workers.Remove(workerId);
                }
                if (workers.Count == 0)
                {
                    m_camp_workers.Remove(campId);
                }
            }
            TeamOrchestrator._Instance.WorkersList[workerId].SetWorkerState(Worker.STATE_SCOUTING);
        }
    }

    public bool IsWorkerBusy(int workerId)
    {
        return m_worker_collectible.ContainsKey(workerId) && m_worker_collectible[workerId] >= 0 &&
            m_worker_camp.ContainsKey(workerId) && m_worker_camp[workerId] >= 0;
    }

    public void AssignNewWorks(Worker worker, Camp camp, Collectible collectible)
    {
        Debug.Log("Worker " + worker.m_workerId + " assigned to camp: " + camp.m_campId + " resource: " + collectible.m_collectibleId);
        AssignWorkerToCamp(worker.m_workerId, camp.m_campId);
        AssignWorkerToCollectible(worker.m_workerId, collectible.m_collectibleId);
        worker.SetWorkerState(Worker.STATE_WAITING);
    }

    public List<Collectible> GetUnassignedCollectibles()
    {

        List<Collectible> ret = new List<Collectible>();
        foreach(var c in TeamOrchestrator._Instance.KnownCollectibles)
        {
            if(!m_collectible_workers.ContainsKey(c.Key) ||
                m_collectible_workers[c.Key] == null ||
                m_collectible_workers[c.Key].Count == 0)
            {
                ret.Add(c.Value);
            }
        }
        return ret;
    }

}
