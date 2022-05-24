
using TypePack;
using TypePack.Core.Attributes;

namespace Assets.GameSaving.Serialization
{

    public class TypePackObjects : global::TypePack.Core.Schema.SchemaKind
    {

        [TypePackType(typeof(global::TeamZ.GameSaving.States.MonoBehaviourState), true)]

        public const ushort TeamZ_GameSaving_States_MonoBehaviourState = 256;

        [TypePackType(typeof(global::TeamZ.GameSaving.Interfaces.State), true)]

        public const ushort TeamZ_GameSaving_Interfaces_State = 257;

        [TypePackType(typeof(global::TeamZ.GameSaving.States.Charaters.LizardState))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.Charaters.LizardState.Health))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.Charaters.LizardState.Armor))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.Charaters.LizardState.PunchDamage))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.Charaters.LizardState.KickDamage))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.Charaters.LizardState.Name))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.Charaters.LizardState.RunSpeed))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.Charaters.LizardState.CreepSpeed))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.Charaters.LizardState.StrikeSpeed))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.Charaters.LizardState.JumpSpeed))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.Charaters.LizardState.JumpForce))]
        public const ushort TeamZ_GameSaving_States_Charaters_LizardState = 258;

        [TypePackType(typeof(global::TeamZ.GameSaving.States.Charaters.HedgehogState))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.Charaters.HedgehogState.Health))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.Charaters.HedgehogState.Armor))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.Charaters.HedgehogState.PunchDamage))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.Charaters.HedgehogState.KickDamage))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.Charaters.HedgehogState.Name))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.Charaters.HedgehogState.RunSpeed))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.Charaters.HedgehogState.CreepSpeed))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.Charaters.HedgehogState.StrikeSpeed))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.Charaters.HedgehogState.JumpSpeed))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.Charaters.HedgehogState.JumpForce))]
        public const ushort TeamZ_GameSaving_States_Charaters_HedgehogState = 259;

        [TypePackType(typeof(global::TeamZ.GameSaving.States.CameraState))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.CameraState.PlayerId))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.CameraState.Position))]
        public const ushort TeamZ_GameSaving_States_CameraState = 260;

        [TypePackType(typeof(global::TeamZ.GameSaving.States.CharacterControllerState))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.CharacterControllerState.CurrentDirection))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.CharacterControllerState.IsClimbed))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.CharacterControllerState.IsKeyUpWasPressed))]
        public const ushort TeamZ_GameSaving_States_CharacterControllerState = 261;

        [TypePackType(typeof(global::TeamZ.GameSaving.States.EntityState))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.EntityState.Id))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.EntityState.AssetGuid))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.EntityState.Scale))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.EntityState.Rotation))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.EntityState.Position))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.EntityState.LevelId))]
        public const ushort TeamZ_GameSaving_States_EntityState = 262;

        [TypePackType(typeof(global::TeamZ.GameSaving.States.GameObjectState))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.GameObjectState.Entity))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.GameObjectState.MonoBehaviousStates))]
        public const ushort TeamZ_GameSaving_States_GameObjectState = 263;

        [TypePackType(typeof(global::TeamZ.GameSaving.States.GameState))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.GameState.LevelId))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.GameState.GameObjectsStates))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.GameState.VisitedLevels))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.GameState.PlayerServiceState))]
        public const ushort TeamZ_GameSaving_States_GameState = 264;

        [TypePackType(typeof(global::TeamZ.GameSaving.States.LevelObjectState))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.LevelObjectState.Strength))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.LevelObjectState.IsDestructible))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.LevelObjectState.IsOnlyMovable))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.LevelObjectState.HighlightingColor))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.LevelObjectState.IsAlreadyExploded))]
        public const ushort TeamZ_GameSaving_States_LevelObjectState = 265;

        [TypePackType(typeof(global::TeamZ.GameSaving.States.LootBoxState))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.LootBoxState.IsOpen))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.LootBoxState.IsAvailable))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.LootBoxState.IsNotEmpty))]
        public const ushort TeamZ_GameSaving_States_LootBoxState = 266;

        [TypePackType(typeof(global::TeamZ.Assets.GameSaving.States.RenderState))]
        [TypePackMember(nameof(global::TeamZ.Assets.GameSaving.States.RenderState.SortingLayerName))]
        [TypePackMember(nameof(global::TeamZ.Assets.GameSaving.States.RenderState.SortingLayerOrder))]
        public const ushort TeamZ_Assets_GameSaving_States_RenderState = 267;

        [TypePackType(typeof(global::TeamZ.Assets.UI.Speech.SpeechBubbleState))]
        [TypePackMember(nameof(global::TeamZ.Assets.UI.Speech.SpeechBubbleState.Messages))]
        [TypePackMember(nameof(global::TeamZ.Assets.UI.Speech.SpeechBubbleState.Duration))]
        [TypePackMember(nameof(global::TeamZ.Assets.UI.Speech.SpeechBubbleState.Character))]
        public const ushort TeamZ_Assets_UI_Speech_SpeechBubbleState = 268;

        [TypePackType(typeof(global::TeamZ.Code.Game.Players.PlayerServiceState))]
        [TypePackMember(nameof(global::TeamZ.Code.Game.Players.PlayerServiceState.FirstPlayerEntityId))]
        [TypePackMember(nameof(global::TeamZ.Code.Game.Players.PlayerServiceState.SecondPlayerEntityId))]
        public const ushort TeamZ_Code_Game_Players_PlayerServiceState = 269;

        [TypePackType(typeof(global::UnityEngine.Vector2))]
        [TypePackMember(nameof(global::UnityEngine.Vector2.x))]
        [TypePackMember(nameof(global::UnityEngine.Vector2.y))]
        public const ushort UnityEngine_Vector2 = 270;

        [TypePackType(typeof(global::UnityEngine.Vector3))]
        [TypePackMember(nameof(global::UnityEngine.Vector3.x))]
        [TypePackMember(nameof(global::UnityEngine.Vector3.y))]
        [TypePackMember(nameof(global::UnityEngine.Vector3.z))]
        public const ushort UnityEngine_Vector3 = 271;

        [TypePackType(typeof(global::UnityEngine.Quaternion))]
        [TypePackMember(nameof(global::UnityEngine.Quaternion.x))]
        [TypePackMember(nameof(global::UnityEngine.Quaternion.y))]
        [TypePackMember(nameof(global::UnityEngine.Quaternion.z))]
        [TypePackMember(nameof(global::UnityEngine.Quaternion.w))]
        public const ushort UnityEngine_Quaternion = 272;

        [TypePackType(typeof(global::UnityEngine.Color))]
        [TypePackMember(nameof(global::UnityEngine.Color.r))]
        [TypePackMember(nameof(global::UnityEngine.Color.g))]
        [TypePackMember(nameof(global::UnityEngine.Color.b))]
        [TypePackMember(nameof(global::UnityEngine.Color.a))]
        public const ushort UnityEngine_Color = 273;

        [TypePackType(typeof(global::TeamZ.Code.Game.Cutscenes.CutsceneActivatorState))]
        [TypePackMember(nameof(global::TeamZ.Code.Game.Cutscenes.CutsceneActivatorState.AssetGuid))]
        public const ushort TeamZ_Code_Game_Cutscenes_CutsceneActivatorState = 274;

        [TypePackType(typeof(global::TeamZ.GameSaving.States.ActivatorState))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.ActivatorState.IsActive))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.ActivatorState.Name))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.ActivatorState.IsActivated))]
        public const ushort TeamZ_GameSaving_States_ActivatorState = 275;

        [TypePackType(typeof(global::TeamZ.GameSaving.States.PortalState))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.PortalState.IsActive))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.PortalState.SceneName))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.PortalState.Location))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.PortalState.Name))]
        public const ushort TeamZ_GameSaving_States_PortalState = 276;

        [TypePackType(typeof(global::TeamZ.GameSaving.States.DoorState))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.DoorState.Name))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.DoorState.Type))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.DoorState.State))]
        public const ushort TeamZ_GameSaving_States_DoorState = 277;

        [TypePackType(typeof(global::TeamZ.GameSaving.States.GeneratorState))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.GeneratorState.Name))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.GeneratorState.IsActive))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.GeneratorState.IsActivated))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.GeneratorState.DoorId))]
        [TypePackMember(nameof(global::TeamZ.GameSaving.States.GeneratorState.PortalId))]
        public const ushort TeamZ_GameSaving_States_GeneratorState = 278;
    }
}
