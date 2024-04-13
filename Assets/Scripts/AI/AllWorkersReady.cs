using MBT;
using System.Collections.Generic;
using UnityEngine;

[MBTNode("Engin2/All Workers Ready")]
[AddComponentMenu("")]
public class AllWorkersReady : Leaf
{
    //Added by Mao
    //return fail to end loop so that workers can begin collecting resources
    public override NodeResult Execute()
    {
        return NodeResult.failure;
    }
}
