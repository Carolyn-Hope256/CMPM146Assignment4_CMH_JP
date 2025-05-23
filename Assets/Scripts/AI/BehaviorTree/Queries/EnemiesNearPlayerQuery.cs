public class EnemiesNearPlayerQuery : BehaviorTree
{
    int count;
    float distance;
    bool iso;

    public override Result Run()
    {
        var nearby = GameManager.Instance.GetEnemiesInRange(GameManager.Instance.player.transform.position, distance);
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

    public EnemiesNearPlayerQuery(int count, float distance, bool isolated) : base()
    {
        this.count = count; 
        this.distance = distance;
        this.iso = isolated;
    }

    public override BehaviorTree Copy()
    {
        return new EnemiesNearPlayerQuery(count, distance, iso);
    }
}
