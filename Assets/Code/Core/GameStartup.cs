using System.Collections.Generic;
using Core;
using Leopotam.Ecs;
using Snake.Core.GameFSM;
using Snake.Core.GameFSM.States;
using Snake.Core.Interfaces;
using t1az2z.Tools.FDebug;
using UnityEngine;

namespace Snake.Core
{
    public class GameStartup : MonoBehaviour
    {
        [SerializeField] private List<Object> UpdateSystemProviders;
        [SerializeField] private List<Object> FixedSystemProviders;
        [SerializeField] private List<Object> LateSystemProviders;
        
        private EcsWorld _world;
        private EcsSystems _updateSystems;
        private EcsSystems _fixedUpdateSystems;
        private EcsSystems _lateUpdateSystems;

        private GameFSM.GameFSM _gameFsm;
        
        private void Awake()
        {
            _world = new EcsWorld();
            
            _updateSystems = CreateEcsSystems( "Update Systems", UpdateSystemProviders);
            _fixedUpdateSystems = CreateEcsSystems("Fixed Update Systems", FixedSystemProviders);
            _lateUpdateSystems = CreateEcsSystems("Late Update Systems", LateSystemProviders);
            
#if UNITY_EDITOR
            Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(_world);
#endif

            
            _updateSystems.Init();
            _fixedUpdateSystems.Init();
            _lateUpdateSystems.Init();
        }
        
        private EcsSystems CreateEcsSystems(string systemCollectionName, List<Object> systemProviders)
        {
            EcsSystems ecsSystems = new EcsSystems(_world, systemCollectionName);

#if UNITY_EDITOR
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(ecsSystems);
#endif

            EcsSystems endFrame = new EcsSystems(_world, systemCollectionName + "EndFrame");

            foreach (Object systemProvider in systemProviders)
            {
                if (systemProvider is ISystemsProvider provider)
                {
                    ecsSystems.Add(provider.GetSystems(_world, endFrame, ecsSystems));
                }
                else
                {
                    FDebug.Log($"#Object {systemProvider.name} is not ISystemProvider, skipping.#", FColor.Red);
                }
            }

            ecsSystems.Add(endFrame);

            return ecsSystems;
        }
        private void Update()
        {
            _updateSystems.Run();
        }

        private void FixedUpdate()
        {
            _fixedUpdateSystems.Run();
        }

        private void LateUpdate()
        {
            _lateUpdateSystems.Run();
        }
    }
}