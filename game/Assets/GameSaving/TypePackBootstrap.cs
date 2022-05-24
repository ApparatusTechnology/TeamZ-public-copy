using TeamZ.Assets.GameSaving.States;
using TeamZ.Assets.UI.Speech;
using TeamZ.Code.Game.Cutscenes;
using TeamZ.Code.Game.Players;
using TeamZ.GameSaving.Interfaces;
using TeamZ.GameSaving.States;
using TeamZ.GameSaving.States.Charaters;
using TypePack.Core;

namespace TeamZ
{
    public class TypePackBootstrap
    {
        public TypePackBootstrap()
        {
            TypePackConfig.Init(config =>
            {
                config.AddUnion<MonoBehaviourState>();

                config.AddObject<GameState>();
                config.AddObject<EntityState>();
                config.AddObject<LizardState>();
                config.AddObject<HedgehogState>();
                config.AddObject<CharacterControllerState>();
                config.AddObject<GameObjectState>();
                config.AddObject<CameraState>();
                config.AddObject<LevelObjectState>();
                config.AddObject<ActivatorState>();
                config.AddObject<LootBoxState>();
                config.AddObject<RenderState>();
                config.AddObject<SpeechBubbleState>();
                config.AddObject<CutsceneActivatorState>();
                config.AddObject<PortalState>();
                config.AddObject<DoorState>();
                config.AddObject<GeneratorState>();

                config.AddUnion<State>();
                config.AddObject<PlayerServiceState>();
                
                config.AddObject<UnityEngine.Vector2>(o => o.x, o => o.y);
                config.AddObject<UnityEngine.Vector3>(o => o.x, o => o.y, o => o.z);
                config.AddObject<UnityEngine.Quaternion>(o => o.x, o => o.y, o => o.z, o => o.w);
                config.AddObject<UnityEngine.Color>(o => o.r, o => o.g, o => o.b, o => o.a);

                config.SetOutputNamespace("Assets.GameSaving.Serialization");
            });
        }
    }
}