using MBT;
using System.Linq;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Engin2/Get nearest camp condition")]
public class GetNearestCamp : Condition
{
    [SerializeField]
    private TransformReference m_workerTransform;// = new TransformReference();
    [SerializeField]
    private Vector2Reference m_nearestCampVec2 = new Vector2Reference();
    [SerializeField]
    private IntReference m_campToAssign = new IntReference();


    [SerializeField]
    public float m_maxDistance = 10.0f;

    public override bool Check()
    {
        if (TeamOrchestrator._Instance.Camps.Count == 0)
        {
            //On n'a pas trouvé de camp. On retourne faux
            return false;
        }

        Camp nearestCamp = TeamOrchestrator._Instance.Camps.Values.First();
        float nearestDistance = Vector3.Distance(nearestCamp.transform.position, m_workerTransform.Value.position);

        foreach (var camp in TeamOrchestrator._Instance.Camps)
        {
            float distance = Vector3.Distance(camp.Value.transform.position, m_workerTransform.Value.position);
            if (nearestDistance > distance) 
            {
                nearestCamp = camp.Value;
                nearestDistance = distance;
            }
        }

        if(nearestDistance > m_maxDistance)
        {
            m_campToAssign.Value = -1;
            return false;
        }

        //Ceci est le camp le plus près. On update sa valeur dans le blackboard et retourne true
        m_nearestCampVec2.Value = new Vector2(nearestCamp.transform.position.x, nearestCamp.transform.position.y);
        m_campToAssign.Value = nearestCamp.m_campId;
        return true;
    }
}
