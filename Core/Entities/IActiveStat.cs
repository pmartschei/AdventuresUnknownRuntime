namespace AdventuresUnknownSDK.Core.Entities
{
    public interface IActiveStat
    { 
        void Register(Entity entity);
        void Unregister(Entity entity);
    }
}