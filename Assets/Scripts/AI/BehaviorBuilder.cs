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
                                        //If you can use your support spells, you should!

                                        new Heal(),
                                        new PermaBuff(),
                                        new Buff(),
                                        
                                        new Sequence(new BehaviorTree[] {//warlocks can get in the way of other enemies, so give them a smidge of space
                                            new NearbyEnemiesQuery(2, 1.5f, false),
                                            new GoTowardsNearestEnemy(.5f, 0.1f, true)

                                        }),
                                        new Selector(new BehaviorTree[] {
                                            new Sequence(new BehaviorTree[] {
                                                new NearbyEnemiesQuery(1, 15, false, "zombie"),//If there are any friends in the general area
                                                new MeatShieldQuery(false),//if warlock is closer to the player than their friend is
                                                new GoTowards(GameManager.Instance.player.transform, 0.5f, 0.5f, true),//move away from player
                                            }),
                                            new Sequence(new BehaviorTree[] {
                                                new NearbyEnemiesQuery(1, 15, false, "zombie"),//If there are any friends in the general area
                                                new GoTowardsNearestEnemy(1, 2, false, "zombie"),//meet up with them
                                            }),
                                            new NavigateToSafeNode( agent.transform.position, 2, 8f)
                                        }),

                                        new Attack()//warlock attack is awful and we don't care about it

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
            result = new Selector(new BehaviorTree[] {

                                        /*new Sequence(new BehaviorTree[] {
                                            new NearbyEnemiesQuery(5, 5, true), //if alone
                                            new GoTo(AIWaypointManager.Instance.GetClosestByType(agent.transform.position,0).transform, 2f), //look for friends
                                            new PlayerDistanceQuery(14, true), //if alone and the player is too close
                                            new GoTowards(GameManager.Instance.player.transform, 1, .05f, true), //run
                                        }),*/
                                            
                                        // }),
                                        new Sequence(new BehaviorTree[] {
                                            new AttackModeQuery(), //if you've been given the attack order
                                            new NearbyEnemiesQuery(8, 5, false), //And it's so crowded you don't care about healers
                                            new GoTowards(GameManager.Instance.player.transform, 6, agent.GetAction("attack").range, false), //attack!
                                            new Attack()
                                        }),
                                        new Sequence(new BehaviorTree[] {
                                            new AttackModeQuery(), //if you've been given the attack order
                                            new NearbyEnemiesQuery(1, 5, true, "warlock"), // but theres no healers nearby
                                            //new GoTowards(GameManager.Instance.player.transform, .5f, agent.GetAction("attack").range, true),//Regroup!
                                            new GoTowardsNearestEnemy(0.5f, 4, false, "warlock")
                                        }),
                                        new Sequence(new BehaviorTree[] {
                                            new AttackModeQuery(), //if you've been given the attack order
                                            
                                            new GoTowards(GameManager.Instance.player.transform, 6, agent.GetAction("attack").range, false), //Attack!
                                            new Attack()
                                        }),
                                        new Sequence(new BehaviorTree[] {
                                            new NearbyEnemiesQuery(1, 8, false, "zombie", true), //if your neighbors are partying
                                            new AttackMode()//join!
                                            
                                        }),
                                        new Sequence(new BehaviorTree[] {
                                            new EnemiesNearPlayerQuery(5, 6, false), //if there's already a party
                                            new AttackMode()//join!
                                            
                                        }),
                                        new Sequence(new BehaviorTree[] {
                                             new NearbyEnemiesQuery(14, 14, false), //if not alone
                                             new NearbyEnemiesQuery(3, 14, false, "warlock"), //and there's adequate healers
                                             new AttackMode() //Charge!
                                        }),
                                        new NavigateToSafeNode( agent.transform.position, 2, 5f)//Otherwise, rally!

                                     });
        }
        else
        {
            result = new Selector(new BehaviorTree[] {
                                    new Sequence(new BehaviorTree[] {
                                        new AttackModeQuery(),
                                        new EnemiesNearPlayerQuery(4, 4, false), //if there's a crowd
                                        new GoTowards(GameManager.Instance.player.transform, 6, agent.GetAction("attack").range, false),//rush!
                                        new Attack()

                                    }),

                                    new Sequence(new BehaviorTree[] {
                                        new AttackModeQuery(),
                                        new EnemiesNearPlayerQuery(6, 3, true),
                                        new PlayerDistanceQuery(9, true), //if you're too close to the fire...
                                        new GoTowards(GameManager.Instance.player.transform, 3, .1f, true),//back off!
                                    }),

                                    new Sequence(new BehaviorTree[] {
                                        new AttackModeQuery(), //if a party's starting...
                                        new GoTowards(GameManager.Instance.player.transform, 6, 10, false),//get ready!
                                    }),



                                    new Sequence(new BehaviorTree[] {
                                            new EnemiesNearPlayerQuery(6, 8, false), //if there's already a party
                                            new AttackMode()//join!
                                            
                                    }),

                                    new Sequence(new BehaviorTree[] {//skeletons can get in the way of other enemies, so give them a smidge of space
                                            new NearbyEnemiesQuery(2, 3f, false),
                                            new GoTowardsNearestEnemy(.5f, 0.1f, true)

                                    }),

                                    new NavigateToSafeNode( agent.transform.position, 10, 3f)
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
