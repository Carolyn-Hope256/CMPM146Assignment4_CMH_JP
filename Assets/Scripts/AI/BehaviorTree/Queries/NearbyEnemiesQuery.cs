public class NearbyEnemiesQuery : BehaviorTree
{
    int count;
    float distance;
    bool iso;
    string type;
    bool aggro;

    public override Result Run()
    {
        var nearby = GameManager.Instance.GetEnemiesInRange(agent.transform.position, distance, type, aggro);
        /*if (type != null)
        {
            nearby = GameManager.Instance.GetEnemiesInRange(agent.transform.position, distance, type);
        }*/
        if(nearby == null)
        {
            return Result.FAILURE;
        }

        bool populated = nearby.Count >= count;

        if (iso)
        {
            populated = !populated;
        }

        if (populated)
        {
            return Result.SUCCESS;
        }
        return Result.FAILURE;
    }

    /*public NearbyEnemiesQuery(int count, float distance, bool isolated) : base()
    {
        this.count = count; 
        this.distance = distance;
        this.iso = isolated;
    }*/

    public NearbyEnemiesQuery(int count, float distance, bool isolated, string? filter = null, bool rallied = false) : base()
    {
        this.count = count;
        this.distance = distance;
        this.iso = isolated;
        this.type = filter;
        this.aggro = rallied;

    }

    public override BehaviorTree Copy()
    {
        return new NearbyEnemiesQuery(count, distance, iso);
    }
}
