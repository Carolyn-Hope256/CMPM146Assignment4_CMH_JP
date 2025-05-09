using UnityEngine;

public class MeatShieldQuery : BehaviorTree
{
    bool guard;

    public override Result Run()
    {
        GameObject friend = GameManager.Instance.GetClosestOfType(agent.gameObject, "zombie");
        if (friend == null)
        {
            return Result.FAILURE;
        }

        float distance = (agent.transform.position - GameManager.Instance.player.transform.position).magnitude;
        float friendDist = (friend.transform.position - GameManager.Instance.player.transform.position).magnitude;

        bool shielded = distance > friendDist;

        if (!guard)
        {
            shielded = !shielded;
        }
        if (shielded)
        {
            return Result.SUCCESS;
        }
        return Result.FAILURE;
    }

    public MeatShieldQuery(bool guarded) : base()
    {
        this.guard = guarded;
    }

    public override BehaviorTree Copy()
    {
        return new MeatShieldQuery(guard);
    }
}
