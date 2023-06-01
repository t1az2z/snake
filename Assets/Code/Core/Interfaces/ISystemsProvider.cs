using Leopotam.Ecs;

namespace Snake.Core.Interfaces
{
    public interface ISystemsProvider
    {
        EcsSystems GetSystems(EcsWorld world, EcsSystems endFrame, EcsSystems mainSystems);
    }
}