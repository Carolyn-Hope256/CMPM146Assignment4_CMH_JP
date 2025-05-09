using System.Globalization;
using UnityEngine;

public class PermaBuff : BehaviorTree
{
    public override Result Run()
    {
        var target = GameManager.Instance.GetPBuffTarget(agent.gameObject);
        if (target != null)
        {
            Debug.Log("Found buff target " +  target.name);
        }

        EnemyAction act = agent.GetAction("permabuff");
        if (act == null || target == null) return Result.FAILURE;
        Debug.Log("Target is valid");

        bool success = act.Do(target.transform);
        return (success ? Result.SUCCESS : Result.FAILURE);
    }

    public PermaBuff() : base()
    {

    }

    public override BehaviorTree Copy()
    {
        return new PermaBuff();
    }
}
