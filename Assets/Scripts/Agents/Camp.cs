using UnityEngine;

public class Camp : MonoBehaviour
{
    /**
     * Added by Mao
     * Global camp id
     */
    private static int s_globalCampId = 0;

    /**
     * added by Mao
     * identical camp id
     */
    public int m_campId { get; private set; }

    void Start()
    {
        lock(this)
        {
            m_campId = s_globalCampId;
            s_globalCampId++;
            TeamOrchestrator._Instance.Camps.Add(this.m_campId, this);
            Debug.Log("New camp added : " + m_campId);
        }
    }

    private void OnDestroy()
    {
        TeamOrchestrator._Instance.Camps.Remove(this.m_campId);
    }
}
