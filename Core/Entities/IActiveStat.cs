namespace AdventuresUnknownSDK.Core.Entities
{
    public interface IActiveStat
    {
        bool StatsChanged { get; set; }
        void Initialize(Entity activeStat);
    }
}