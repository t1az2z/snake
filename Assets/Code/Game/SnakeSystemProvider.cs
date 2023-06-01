using Leopotam.Ecs;
using Snake.Core.GameFSM;
using Snake.Core.Interfaces;
using Snake.Game.Systems;
using UnityEngine;

namespace Snake.Game
{
    [CreateAssetMenu(fileName = "SnakeSystemProvider", menuName = "Snake/SystemProvider")]
    public class SnakeSystemProvider : ScriptableObject, ISystemsProvider
    {
        public EcsSystems GetSystems(EcsWorld world, EcsSystems endFrame, EcsSystems mainSystems)
        {
            var systems = new EcsSystems(world, "Snake Systems");

            var gameFsm = new GameFsm();
            gameFsm.Init();
            
            systems
                .Add(new DebugKeysSystem())
                
                .Inject(gameFsm);

            return systems;
        }
    }
}