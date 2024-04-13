using MBT;
using System.Collections.Generic;
using UnityEngine;

[MBTNode("Engin2/Assign Works")]
[AddComponentMenu("")]
public class AssignWorks : Leaf
{
    public override NodeResult Execute()
    {
        return NodeResult.success;
    }

}
