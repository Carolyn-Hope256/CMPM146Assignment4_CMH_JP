using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Constraints;


public class AIWaypointManager
{
    private List<AIWaypoint> waypoints;

    private static AIWaypointManager theInstance;
    public static AIWaypointManager Instance
    {
        get
        {
            if (theInstance == null)
                theInstance = new AIWaypointManager();
            return theInstance;
        }
    }

    private AIWaypointManager()
    {
        waypoints = new List<AIWaypoint>();
    }

    public void AddWaypoint(AIWaypoint wp)
    {
        waypoints.Add(wp);
    }

    public AIWaypoint GetClosest(Vector3 point)
    {
        return waypoints.Aggregate((a, b) => (a.position - point).sqrMagnitude < (b.position - point).sqrMagnitude ? a : b);
    }

    public AIWaypoint GetClosestByType(Vector3 point, AIWaypoint.Type type)
    {
        var of_type = waypoints.FindAll((a) => a.type == type);
        if (of_type.Count == 0) return null;
        return of_type.Aggregate((a, b) => (a.position - point).sqrMagnitude < (b.position - point).sqrMagnitude ? a : b);
    }

    public AIWaypoint Get(int i)
    {
        if (waypoints.Count <= i) return null;
        return waypoints[i];
    }
    public AIWaypoint get_direction_to_safest_waypoint(AIWaypoint waypoint){
        // var max = 0f;
        // var index = -1;
        // for (var i = 0; i < waypoints.Count; i++){
        //     var temp = (waypoints[i].transform.position - GameManager.Instance.player.transform.position).magnitude;
        //     if(temp > max){
        //         max = temp;
        //         index = i;
        //     }
        // }
        
        var safestwaypoint = GetClosestByType(waypoint.transform.position, 0);//waypoints[index];
        Debug.Log("Looking for direction to waypoint " + safestwaypoint.gameObject.name + ", currently at waypoint " + waypoint.gameObject.name);
        if (safestwaypoint == waypoint){
            Debug.Log("Arrived at safest waypoint!");
            return null;
        }
        var num = 0f;
        var tempwaypoint = waypoint;
        while(tempwaypoint != safestwaypoint){
            num += (tempwaypoint.transform.position - tempwaypoint.adjacentWaypoints[1].transform.position).magnitude;
            tempwaypoint = tempwaypoint.adjacentWaypoints[1];
            // if ((tempwaypoint.transform.position - GameManager.Instance.player.transform.position).magnitude < 30){
            //     return waypoint.adjacentWaypoints[0];
            // }
        }
        var num2 = 0f;
        tempwaypoint = waypoint;
        while(tempwaypoint != safestwaypoint){
            num2 += (tempwaypoint.transform.position - tempwaypoint.adjacentWaypoints[0].transform.position).magnitude;
            tempwaypoint = tempwaypoint.adjacentWaypoints[0];
            // if ((tempwaypoint.transform.position - GameManager.Instance.player.transform.position).magnitude < 30){
            //     return waypoint.adjacentWaypoints[1];
            // }
            
        }

        if (num < num2){
            //go clockwise
            return waypoint.adjacentWaypoints[1];
        }else{
            //go counter clockwise
            return waypoint.adjacentWaypoints[0];
        }
    }
}
