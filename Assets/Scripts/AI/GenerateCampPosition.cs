using MBT;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

[MBTNode("Engin2/Generate Camp Position")]
[AddComponentMenu("")]
public class GenerateCampPosition: Leaf
{
    [SerializeField]
    private float m_speed = 0.1f;

    [SerializeField]
    private float m_time = 5.0f;

    [SerializeField]
    private float m_minDistance = 10.0f;

    public FloatReference m_movementRange = new FloatReference(VarRefMode.DisableConstant);
    public Vector2Reference m_targetPosition2D = new Vector2Reference(VarRefMode.DisableConstant);

    [SerializeField]
    private TransformReference m_agentTransform;// = new TransformReference();

    public override void OnEnter()
    {
        // ONLY FOR THE FIRST TIME OF PLACING A CAMP
        if(TeamOrchestrator._Instance.Camps.Count == 0)
        {
            var pos = CalculateFirstCampPosition();
            m_targetPosition2D.Value = pos + new Vector2(m_agentTransform.Value.position.x, m_agentTransform.Value.position.y);

        }
        else
        {
            var pos = CalculateMoreCampPosition();
            m_targetPosition2D.Value = pos + new Vector2(m_agentTransform.Value.position.x, m_agentTransform.Value.position.y);
        }
    }

    public override NodeResult Execute()
    {
        //Debug.Log("On GeneratedPointAroundSelf execute");
        return NodeResult.success;
    }

    // known that there's no camp yet
    private Vector2 CalculateFirstCampPosition()
    {
        float num = TeamOrchestrator._Instance.KnownCollectibles.Count;
        if (num == 1)// if resources found == 1
        {
            Collectible closestCollectible = TeamOrchestrator._Instance.KnownCollectibles.Values.First();
            Vector3 position = closestCollectible.transform.position;
            float distance = Vector2.Distance(new Vector2(position.x, position.y), Vector2.zero);
            float sinus = position.y / distance;
            float cosin = position.x / distance;
            float offset = m_minDistance; //m_speed * m_time;
            Vector2 newpos = new Vector2(position.x, position.y);
            newpos -= new Vector2(offset * cosin, offset * sinus);
            return newpos;
        }
        // TODO if resources found > 1
        else 
        {
            /*Vector2 position = new Vector2(transform.position.x, transform.position.y);
            float sum = 0.0f;
            float squaresum = 0.0f;
            foreach (var coll in TeamOrchestrator._Instance.KnownCollectibles)
            {
                Vector2 collPos = new Vector2(coll.transform.position.x, coll.transform.position.y);
                float dist = Vector2.Distance(position, collPos);
                sum += dist;
                squaresum += Mathf.Pow(dist, 2.0f);
            }
            float average = sum / num;
            float variance = squaresum / num - Mathf.Pow(sum / num, 2.0f);
            float standardvariance = Mathf.Pow(variance, 0.5f);
            ......
            return pos;*/
            var pos = Random.insideUnitCircle * m_minDistance;
            m_targetPosition2D.Value = pos + new Vector2(m_agentTransform.Value.position.x, m_agentTransform.Value.position.y);
            return transform.position;

        }


    }

    private Vector2 CalculateMoreCampPosition()
    {
        //TODO
        return Vector2.zero;
    }

}
