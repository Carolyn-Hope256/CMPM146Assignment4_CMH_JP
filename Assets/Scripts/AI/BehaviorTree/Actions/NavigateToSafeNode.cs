using System;
using System.Linq;
using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEngine;

public class NavigateToSafeNode : BehaviorTree
{
    Transform target;
    float arrived_distance;
    float distance;
    bool in_progress;
    Vector3 start_point;
    AIWaypoint currentWaypoint;

    public override Result Run()
    {
       
        if (!in_progress)
        {
            in_progress = true;
            start_point = agent.transform.position;
        }
        // if ((start_point - GameManager.Instance.player.transform.position).magnitude > 50){
        //     agent.GetComponent<Unit>().movement = new Vector2(0, 0);
        //     in_progress = false;
        //     return Result.SUCCESS;
        // }
        
        if (currentWaypoint == null){
            target = AIWaypointManager.Instance.GetClosest(start_point).transform;
        }else{
            var waypoint = AIWaypointManager.Instance.  get_direction_to_safest_waypoint(currentWaypoint);
            if (waypoint == null){
                agent.GetComponent<Unit>().movement = new Vector2(0, 0);
                in_progress = false;
                
                return Result.SUCCESS;
            }
            target = waypoint.transform;
        }
        
        
        
        

        //make movement vector
        Vector3 direction = target.position - agent.transform.position;

        if ((direction.magnitude < arrived_distance) || (agent.transform.position - start_point).magnitude >= distance)
        {
            agent.GetComponent<Unit>().movement = new Vector2(0, 0);
            in_progress = false;
            if (direction.magnitude < arrived_distance){
                currentWaypoint = AIWaypointManager.Instance.GetClosest(start_point);
            }
            return Result.SUCCESS;
        }
        else
        {
            agent.GetComponent<Unit>().movement = direction.normalized;
            return Result.IN_PROGRESS;  
        }
    }

    public NavigateToSafeNode( Vector3 start_point, float distance, float arrived_distance) : base()
    {
        this.start_point = start_point;
        this.arrived_distance = arrived_distance;
        this.distance = distance;
        this.in_progress = false;
        this.currentWaypoint = null;
    }

    public override BehaviorTree Copy()
    {
        return new NavigateToSafeNode(start_point, distance, arrived_distance);
    }
}

