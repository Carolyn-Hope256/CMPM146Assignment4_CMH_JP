using UnityEngine;
using System.Globalization;
public class AttackModeQuery : BehaviorTree
{

    public override Result Run()
    {
        if (GameManager.Instance.AttackGroup.ContainsKey(agent.gameObject.GetInstanceID()))
        {
            return Result.SUCCESS;
        }

        return Result.FAILURE;
    }

    public AttackModeQuery() : base()
    {
       
    }

    public override BehaviorTree Copy()
    {
        return new AttackModeQuery();
    }
}
