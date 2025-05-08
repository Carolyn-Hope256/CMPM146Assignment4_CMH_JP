using UnityEngine;

public class PlayerDistanceQuery : BehaviorTree
{
    float range;
    bool inside;

    public override Result Run()
    {
        float distance = (agent.transform.position - GameManager.Instance.player.transform.position).magnitude;
        bool within = distance < range;

        if (!inside)
        {
            within = !within;
        }
        if (within)
        {
            return Result.SUCCESS;
        }
        return Result.FAILURE;
    }

    public PlayerDistanceQuery(float range, bool inside) : base()
    {
        this.range = range;
        this.inside = inside;

    }

    public override BehaviorTree Copy()
    {
        return new PlayerDistanceQuery(range, inside);
    }
}
