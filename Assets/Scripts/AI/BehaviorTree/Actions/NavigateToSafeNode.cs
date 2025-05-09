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
         if((start_point - GameManager.Instance.player.transform.position).magnitude > 30){
            return Result.SUCCESS;
        }
        
        if (currentWaypoint == null){
            //move in direction of nearest waypoint
            if (target == null){
                target = AIWaypointManager.Instance.GetClosest(start_point).transform;
            }
        }else{
            //move to the more optimal node
            var temp = currentWaypoint.adjacentWaypoints;
            var dist1 = (temp[0].transform.position - GameManager.Instance.player.transform.position).magnitude;
            var dist2 = (temp[1].transform.position - GameManager.Instance.player.transform.position).magnitude;
            if (dist1 < dist2){
                if (Math.Abs(dist1 - dist2) <= 0.1){
                    target = currentWaypoint.transform;
                }
                else{
                    target = temp[1].transform;
                }
            }else{
                target = temp[0].transform;
            }
        }


        var waypoint = AIWaypointManager.Instance.get_safest_waypoint();




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

