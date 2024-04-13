using MBT;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Engin2/Has Found Collectible")]
public class HasFoundCollectibleCondition : Condition
{

    [SerializeField]
    private IntReference m_collectibleToAssign = new IntReference();

    public override bool Check()
    {
        if(TeamOrchestrator._Instance.KnownCollectibles.Count == 0)
        {
            return false;
        }
        List<Collectible> cs = DispatchCenter._Instance.GetUnassignedCollectibles();
        if(cs != null && cs.Count > 0 )
        {
            Debug.Log("Found collectible amount: " +  cs.Count);
            m_collectibleToAssign.Value = cs[0].m_collectibleId;
            Debug.Log("Assign collectible : " + cs[0].m_collectibleId);
            return true;
        }
        else
        {
            m_collectibleToAssign.Value = -1;
        }
        return false;
    }
}
