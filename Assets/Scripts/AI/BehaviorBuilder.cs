using UnityEngine;

public class BehaviorBuilder
{
    public static BehaviorTree MakeTree(EnemyController agent)
    {
        BehaviorTree result = null;
        if (agent.monster == "warlock")
        {
            result = new KineticSequence(new BehaviorTree[] {
                                        //new MoveToPlayer(10),
                                        new Sequence(new BehaviorTree[] {
                                            new PlayerDistanceQuery(18, false),
                                            new GoTowards(GameManager.Instance.player.transform, 1, .05f, false)
                                        }),

                                        new Sequence(new BehaviorTree[] {
                                            new PlayerDistanceQuery(16, true),
                                            new GoTowards(GameManager.Instance.player.transform, 1, .05f, true)
                                        }),

                                        new PermaBuff(),
                                        new Heal(),
                                        new Buff(),
                                        new Attack()
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
                                            new NearbyEnemiesQuery(2, 5, true), //if alone
                                            new GoTowardsNearestEnemy(1, 1, false), //look for friends
                                            new PlayerDistanceQuery(14, true), //if alone and the player is too close
                                            new GoTowards(GameManager.Instance.player.transform, 1, .05f, true), //run
                                        }),
                                        new Sequence(new BehaviorTree[] {
                                            new NearbyEnemiesQuery(5, 5, false), //if not alone
                                            new MoveToPlayer(agent.GetAction("attack").range),//charge!
                                            new Attack()
                                        }),

                                     });
        }
        else
        {
            result = new Sequence(new BehaviorTree[] {
                                       new MoveToPlayer(agent.GetAction("attack").range),
                                       new Attack()
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
