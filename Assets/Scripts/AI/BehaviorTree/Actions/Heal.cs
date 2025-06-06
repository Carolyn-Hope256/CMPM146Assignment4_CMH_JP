using UnityEngine;

public class Heal : BehaviorTree
{
    public override Result Run()
    {
        var target = GameManager.Instance.GetBestHealTarget(agent.gameObject);
        EnemyAction act = agent.GetAction("heal");
        if (act == null || target == null) return Result.FAILURE;

        bool success = act.Do(target.transform);
        return (success ? Result.SUCCESS : Result.FAILURE);
        
    }

    public Heal() : base()
    {
        
    }

    public override BehaviorTree Copy()
    {
        return new Heal();
    }
}
