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

    public override Result Run()
    {
        //var t;
        var t = GameManager.Instance.GetClosestOtherEnemy(agent.gameObject);
        if (filter != null)
        {
            t = GameManager.Instance.GetClosestOfType(agent.gameObject, filter);
        }
        

        if(this.target == null && t != null)
        {
            this.target = t.transform;
        }
        if (target == null)
        {
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

        /*var nearby = GameManager.Instance.GetEnemiesInRange(agent.transform.position, 50);
        float closest = 51;
        float dis;
        for (int e = 0; e < nearby.Count; e++) {
            dis = (nearby[e].transform.position - agent.transform.position).magnitude;
            if (dis < closest)
            {
                this.target = nearby[e].transform;
                closest = dis;
            }
        }*/

        
        /*if (t != null)
        { 
            this.target = t;
        }*/
    }

    public GoTowardsNearestEnemy(float distance, float arrived_distance, bool back, string type) : base()
    {
        this.arrived_distance = arrived_distance;
        this.distance = distance;
        this.backstep = back;
        this.filter = type;
        this.in_progress = false;
        
    }

    public override BehaviorTree Copy()
    {
        return new GoTowardsNearestEnemy(distance, arrived_distance, backstep);
    }
}

