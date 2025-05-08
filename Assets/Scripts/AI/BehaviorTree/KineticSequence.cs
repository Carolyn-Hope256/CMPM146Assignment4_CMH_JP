using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class KineticSequence : InteriorNode 
{
    public override Result Run()
    {
        if (current_child >= children.Count)
        {
            current_child = 0;
            return Result.SUCCESS;
        }

        Result res = children[current_child].Run();

        if (res == Result.SUCCESS || res == Result.FAILURE)
        {
            current_child++;
        }
        return Result.IN_PROGRESS;
    }

    public KineticSequence(IEnumerable<BehaviorTree> children) : base (children)
    {
    }

    public override BehaviorTree Copy()
    {
        return new Sequence(CopyChildren());
    }
}
