namespace AdventuresUnknownSDK.Core.Entities
{
    public interface IActiveStat
    {
        bool StatsChanged { get; set; }
        Stat[] Stats { get; }
        Stat[] RawStats { get; }
        Stat GetStat(string modTypeIdentifier);
    }
}