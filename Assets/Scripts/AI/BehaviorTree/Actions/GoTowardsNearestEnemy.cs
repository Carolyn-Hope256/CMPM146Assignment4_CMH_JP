using System.Globalization;
using UnityEngine;

public class GoTowardsNearestEnemy : BehaviorTree
{
    Transform target;
    float arrived_distance;
    float distance;
    bool backstep;
    string filter;
    bool in_progress;
    Vector3 start_point;
    float time;
    public override Result Run()
    {
        //var t;
        var t = GameManager.Instance.GetClosestOtherEnemy(agent.gameObject);
        if (filter != null)
        {
            t = GameManager.Instance.GetClosestOfType(agent.gameObject, filter);
        }

        if (Time.time - time > 5)
        {
            Debug.Log("Stuck, exiting move");
            time = Time.time;
            return Result.SUCCESS;
        }

        if (this.target == null && t != null)
        {
            this.target = t.transform;
        }
        if (target == null)
        {
            time = Time.time;
            return Result.FAILURE;
        }
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
            time = Time.time;
            return Result.SUCCESS;
        }
        else
        {
            agent.GetComponent<Unit>().movement = direction.normalized;
            return Result.IN_PROGRESS;  
        }
    }

    public GoTowardsNearestEnemy(float distance, float arrived_distance, bool back) : base()
    {
        this.arrived_distance = arrived_distance;
        this.distance = distance;
        this.backstep = back;
        this.in_progress = false;
        this.time = Time.time;
    }

    public GoTowardsNearestEnemy(float distance, float arrived_distance, bool back, string type) : base()
    {
        this.arrived_distance = arrived_distance;
        this.distance = distance;
        this.backstep = back;
        this.in_progress = false;
        this.time = Time.time;
        this.filter = type;
        
        
    }

    public override BehaviorTree Copy()
    {
        return new GoTowardsNearestEnemy(distance, arrived_distance, backstep);
    }
}

