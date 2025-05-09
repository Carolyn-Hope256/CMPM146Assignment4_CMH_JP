using Unity.VisualScripting;
using UnityEngine;
using System.Globalization;

public class BehaviorBuilder
{
    public static BehaviorTree MakeTree(EnemyController agent)
    {
        BehaviorTree result = null;
        if (agent.monster == "warlock")
        {
            result = new KineticSequence(new BehaviorTree[] {
                                        
                                        new Sequence(new BehaviorTree[] {
                                            new NearbyEnemiesQuery(2, 30, false),//If there are any friends in the general area
                                            new GoTowardsNearestEnemy(1, 2, false, "zombie"),//meet up with them
                                            new MeatShieldQuery(false),//if warlock is closer to the player than their friend is
                                            new GoTowards(GameManager.Instance.player.transform, 1, 0.5f, true)//move away from player

                                        }),

                                        // new PermaBuff(),
                                        // new Heal(),
                                        // new Buff(),
                                        // new Attack()
                                     });
        }
        /*else if (agent.monster == "zombie")
        {
            result = new Sequence(new BehaviorTree[] {
                                       new MoveToPlayer(agent.GetAction("attack").range),
                                       new Attack()
                                     });
        }*/
        else if (agent.monster == "zombie")
        {
            result = new KineticSequence(new BehaviorTree[] {
                                        //new MoveToPlayer(10);
                                        new Sequence(new BehaviorTree[] {
                                            new NearbyEnemiesQuery(5, 5, true), //if alone
                                            new GoTo(AIWaypointManager.Instance.GetClosestByType(agent.transform.position,0).transform, 2f), //look for friends
                                            new PlayerDistanceQuery(14, true), //if alone and the player is too close
                                            new GoTowards(GameManager.Instance.player.transform, 1, .05f, true), //run
                                            // new GoTowardsNearestEnemy(1, 1, false), //look for friends
                                            
                                        }),
                                        new Sequence(new BehaviorTree[] {
                                            new NearbyEnemiesQuery(5, 6, false), //if not alone
                                            new MoveToPlayer(agent.GetAction("attack").range),//charge!
                                            new Attack()
                                        }),

                                     });
        }
        else
        {
            result = new Sequence(new BehaviorTree[] {
                                    new NavigateToSafeNode( agent.transform.position, 1, 1f)
                                    //    new MoveToPlayer(agent.GetAction("attack").range),
                                    //    new Attack()
                                     });
        }

        // do not change/remove: each node should be given a reference to the agent
        foreach (var n in result.AllNodes())
        {
            n.SetAgent(agent);
        }
        return result;
    }
}
