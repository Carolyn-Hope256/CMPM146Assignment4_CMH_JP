using UnityEngine;
using System.Globalization;
public class AttackMode : BehaviorTree
{

    public override Result Run()
    {
        if (!GameManager.Instance.AttackGroup.ContainsKey(agent.gameObject.GetInstanceID()))
        {
            GameManager.Instance.AttackGroup.Add(agent.gameObject.GetInstanceID(), true);
        }

        return Result.SUCCESS;
    }

    public AttackMode() : base()
    {
       
    }

    public override BehaviorTree Copy()
    {
        return new AttackMode();
    }
}
