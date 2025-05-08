using UnityEngine;

public class GoTowards : BehaviorTree
{
    Transform target;
    float arrived_distance;
    float distance;
    bool backstep;
    bool in_progress;
    Vector3 start_point;

    public override Result Run()
    {
        if (!in_progress)
        {
            in_progress = true;
            start_point = agent.transform.position;
        }
        Vector3 direction = target.position - agent.transform.position;

        if (backstep)
        {
            direction *= -1;
        }

        if ((direction.magnitude < arrived_distance) || (agent.transform.position - start_point).magnitude >= distance)
        {
            agent.GetComponent<Unit>().movement = new Vector2(0, 0);
            in_progress = false;
            return Result.SUCCESS;
        }
        else
        {
            agent.GetComponent<Unit>().movement = direction.normalized;
            return Result.IN_PROGRESS;  
        }
    }

    public GoTowards(Transform target, float distance, float arrived_distance, bool back) : base()
    {
        this.target = target;
        this.arrived_distance = arrived_distance;
        this.distance = distance;
        this.backstep = back;
        this.in_progress = false;
    }

    public override BehaviorTree Copy()
    {
        return new GoTowards(target, distance, arrived_distance, backstep);
    }
}

