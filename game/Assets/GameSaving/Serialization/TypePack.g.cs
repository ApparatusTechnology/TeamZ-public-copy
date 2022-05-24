
using System;
using TypePack.Core.Writers;
using TypePack.Utils;

namespace Assets.GameSaving.Serialization
{

    [global::System.CodeDom.Compiler.GeneratedCode("TypePack", "0.6.1.0")]
    public class TypePackSerializer
    {
        public static class SerializationDictionary<TValue>
        {
            static SerializationDictionary()
            {
                Reader = BaseSerializationDictionary.Dictionary<TValue>.Reader;
                Writer = BaseSerializationDictionary.Dictionary<TValue>.Writer;
            }

            public static Func<global::FastStream.FastReader, TValue> Reader;
            public static Action<global::FastStream.FastMemoryWriter, TValue> Writer;
        }

        global::System.Buffers.ArrayPool<byte> pool;

        static TypePackSerializer()
        {
            BaseSerializationDictionary.Init();

            SerializationDictionary<global::TeamZ.GameSaving.States.MonoBehaviourState>.Writer = TypePackWrite;
            SerializationDictionary<global::TeamZ.GameSaving.States.MonoBehaviourState>.Reader = TypePackRead_TeamZ_GameSaving_States_MonoBehaviourState;

            SerializationDictionary<global::TeamZ.GameSaving.Interfaces.State>.Writer = TypePackWrite;
            SerializationDictionary<global::TeamZ.GameSaving.Interfaces.State>.Reader = TypePackRead_TeamZ_GameSaving_Interfaces_State;

            SerializationDictionary<global::TeamZ.GameSaving.States.Charaters.LizardState>.Writer = TypePackWrite;
            SerializationDictionary<global::TeamZ.GameSaving.States.Charaters.LizardState>.Reader = TypePackRead_TeamZ_GameSaving_States_Charaters_LizardState;

            SerializationDictionary<global::TeamZ.GameSaving.States.Charaters.HedgehogState>.Writer = TypePackWrite;
            SerializationDictionary<global::TeamZ.GameSaving.States.Charaters.HedgehogState>.Reader = TypePackRead_TeamZ_GameSaving_States_Charaters_HedgehogState;

            SerializationDictionary<global::TeamZ.GameSaving.States.CameraState>.Writer = TypePackWrite;
            SerializationDictionary<global::TeamZ.GameSaving.States.CameraState>.Reader = TypePackRead_TeamZ_GameSaving_States_CameraState;

            SerializationDictionary<global::TeamZ.GameSaving.States.CharacterControllerState>.Writer = TypePackWrite;
            SerializationDictionary<global::TeamZ.GameSaving.States.CharacterControllerState>.Reader = TypePackRead_TeamZ_GameSaving_States_CharacterControllerState;

            SerializationDictionary<global::TeamZ.GameSaving.States.EntityState>.Writer = TypePackWrite;
            SerializationDictionary<global::TeamZ.GameSaving.States.EntityState>.Reader = TypePackRead_TeamZ_GameSaving_States_EntityState;

            SerializationDictionary<global::TeamZ.GameSaving.States.GameObjectState>.Writer = TypePackWrite;
            SerializationDictionary<global::TeamZ.GameSaving.States.GameObjectState>.Reader = TypePackRead_TeamZ_GameSaving_States_GameObjectState;

            SerializationDictionary<global::TeamZ.GameSaving.States.GameState>.Writer = TypePackWrite;
            SerializationDictionary<global::TeamZ.GameSaving.States.GameState>.Reader = TypePackRead_TeamZ_GameSaving_States_GameState;

            SerializationDictionary<global::TeamZ.GameSaving.States.LevelObjectState>.Writer = TypePackWrite;
            SerializationDictionary<global::TeamZ.GameSaving.States.LevelObjectState>.Reader = TypePackRead_TeamZ_GameSaving_States_LevelObjectState;

            SerializationDictionary<global::TeamZ.GameSaving.States.LootBoxState>.Writer = TypePackWrite;
            SerializationDictionary<global::TeamZ.GameSaving.States.LootBoxState>.Reader = TypePackRead_TeamZ_GameSaving_States_LootBoxState;

            SerializationDictionary<global::TeamZ.Assets.GameSaving.States.RenderState>.Writer = TypePackWrite;
            SerializationDictionary<global::TeamZ.Assets.GameSaving.States.RenderState>.Reader = TypePackRead_TeamZ_Assets_GameSaving_States_RenderState;

            SerializationDictionary<global::TeamZ.Assets.UI.Speech.SpeechBubbleState>.Writer = TypePackWrite;
            SerializationDictionary<global::TeamZ.Assets.UI.Speech.SpeechBubbleState>.Reader = TypePackRead_TeamZ_Assets_UI_Speech_SpeechBubbleState;

            SerializationDictionary<global::TeamZ.Code.Game.Players.PlayerServiceState>.Writer = TypePackWrite;
            SerializationDictionary<global::TeamZ.Code.Game.Players.PlayerServiceState>.Reader = TypePackRead_TeamZ_Code_Game_Players_PlayerServiceState;

            SerializationDictionary<global::UnityEngine.Vector2>.Writer = TypePackWrite;
            SerializationDictionary<global::UnityEngine.Vector2>.Reader = TypePackRead_UnityEngine_Vector2;

            SerializationDictionary<global::UnityEngine.Vector3>.Writer = TypePackWrite;
            SerializationDictionary<global::UnityEngine.Vector3>.Reader = TypePackRead_UnityEngine_Vector3;

            SerializationDictionary<global::UnityEngine.Quaternion>.Writer = TypePackWrite;
            SerializationDictionary<global::UnityEngine.Quaternion>.Reader = TypePackRead_UnityEngine_Quaternion;

            SerializationDictionary<global::UnityEngine.Color>.Writer = TypePackWrite;
            SerializationDictionary<global::UnityEngine.Color>.Reader = TypePackRead_UnityEngine_Color;

            SerializationDictionary<global::TeamZ.Code.Game.Cutscenes.CutsceneActivatorState>.Writer = TypePackWrite;
            SerializationDictionary<global::TeamZ.Code.Game.Cutscenes.CutsceneActivatorState>.Reader = TypePackRead_TeamZ_Code_Game_Cutscenes_CutsceneActivatorState;

            SerializationDictionary<global::TeamZ.GameSaving.States.ActivatorState>.Writer = TypePackWrite;
            SerializationDictionary<global::TeamZ.GameSaving.States.ActivatorState>.Reader = TypePackRead_TeamZ_GameSaving_States_ActivatorState;

            SerializationDictionary<global::TeamZ.GameSaving.States.PortalState>.Writer = TypePackWrite;
            SerializationDictionary<global::TeamZ.GameSaving.States.PortalState>.Reader = TypePackRead_TeamZ_GameSaving_States_PortalState;

            SerializationDictionary<global::TeamZ.GameSaving.States.DoorState>.Writer = TypePackWrite;
            SerializationDictionary<global::TeamZ.GameSaving.States.DoorState>.Reader = TypePackRead_TeamZ_GameSaving_States_DoorState;

            SerializationDictionary<global::TeamZ.GameSaving.States.GeneratorState>.Writer = TypePackWrite;
            SerializationDictionary<global::TeamZ.GameSaving.States.GeneratorState>.Reader = TypePackRead_TeamZ_GameSaving_States_GeneratorState;
        }

        public TypePackSerializer()
        {
            this.pool = global::System.Buffers.ArrayPool<byte>.Shared;
        }

        public TypePackSerializer(global::System.Buffers.ArrayPool<byte> pool)
        {
            this.pool = pool;
        }

        public TValue Deserialize<TValue>(byte[] bytes)
        {
            using (var memory = new global::System.IO.MemoryStream(bytes))
            using (var reader = new global::FastStream.FastReader(memory))
            {
                return SerializationDictionary<TValue>.Reader(reader);
            }
        }

        public TValue DeserializeFromStream<TValue>(global::System.IO.Stream stream)
        {
            using (var reader = new global::FastStream.FastReader(stream))
            {
                return SerializationDictionary<TValue>.Reader(reader);
            }
        }

        public byte[] SerializeObject<TValue>(TValue value)
        {
            using (var writer = new global::FastStream.FastMemoryWriter(this.pool))
            {
                SerializationDictionary<TValue>.Writer(writer, value);
                return writer.ToArray();
            }
        }

        public void SerializeToStream<TValue>(global::System.IO.Stream stream, TValue value)
        {
            using (var writer = new global::FastStream.FastMemoryWriter(this.pool))
            {
                SerializationDictionary<TValue>.Writer(writer, value);
                writer.FastCopyTo(stream);
            }
        }


        public byte[] Serialize(global::TeamZ.GameSaving.States.MonoBehaviourState value)
        {
            using (var writer = new global::FastStream.FastMemoryWriter(this.pool))
            {
                TypePackWrite(writer, value);
                return writer.ToArray();
            }
        }

        public byte[] Serialize(global::TeamZ.GameSaving.Interfaces.State value)
        {
            using (var writer = new global::FastStream.FastMemoryWriter(this.pool))
            {
                TypePackWrite(writer, value);
                return writer.ToArray();
            }
        }

        public byte[] Serialize(global::TeamZ.GameSaving.States.Charaters.LizardState value)
        {
            using (var writer = new global::FastStream.FastMemoryWriter(this.pool))
            {
                TypePackWrite(writer, value);
                return writer.ToArray();
            }
        }

        public byte[] Serialize(global::TeamZ.GameSaving.States.Charaters.HedgehogState value)
        {
            using (var writer = new global::FastStream.FastMemoryWriter(this.pool))
            {
                TypePackWrite(writer, value);
                return writer.ToArray();
            }
        }

        public byte[] Serialize(global::TeamZ.GameSaving.States.CameraState value)
        {
            using (var writer = new global::FastStream.FastMemoryWriter(this.pool))
            {
                TypePackWrite(writer, value);
                return writer.ToArray();
            }
        }

        public byte[] Serialize(global::TeamZ.GameSaving.States.CharacterControllerState value)
        {
            using (var writer = new global::FastStream.FastMemoryWriter(this.pool))
            {
                TypePackWrite(writer, value);
                return writer.ToArray();
            }
        }

        public byte[] Serialize(global::TeamZ.GameSaving.States.EntityState value)
        {
            using (var writer = new global::FastStream.FastMemoryWriter(this.pool))
            {
                TypePackWrite(writer, value);
                return writer.ToArray();
            }
        }

        public byte[] Serialize(global::TeamZ.GameSaving.States.GameObjectState value)
        {
            using (var writer = new global::FastStream.FastMemoryWriter(this.pool))
            {
                TypePackWrite(writer, value);
                return writer.ToArray();
            }
        }

        public byte[] Serialize(global::TeamZ.GameSaving.States.GameState value)
        {
            using (var writer = new global::FastStream.FastMemoryWriter(this.pool))
            {
                TypePackWrite(writer, value);
                return writer.ToArray();
            }
        }

        public byte[] Serialize(global::TeamZ.GameSaving.States.LevelObjectState value)
        {
            using (var writer = new global::FastStream.FastMemoryWriter(this.pool))
            {
                TypePackWrite(writer, value);
                return writer.ToArray();
            }
        }

        public byte[] Serialize(global::TeamZ.GameSaving.States.LootBoxState value)
        {
            using (var writer = new global::FastStream.FastMemoryWriter(this.pool))
            {
                TypePackWrite(writer, value);
                return writer.ToArray();
            }
        }

        public byte[] Serialize(global::TeamZ.Assets.GameSaving.States.RenderState value)
        {
            using (var writer = new global::FastStream.FastMemoryWriter(this.pool))
            {
                TypePackWrite(writer, value);
                return writer.ToArray();
            }
        }

        public byte[] Serialize(global::TeamZ.Assets.UI.Speech.SpeechBubbleState value)
        {
            using (var writer = new global::FastStream.FastMemoryWriter(this.pool))
            {
                TypePackWrite(writer, value);
                return writer.ToArray();
            }
        }

        public byte[] Serialize(global::TeamZ.Code.Game.Players.PlayerServiceState value)
        {
            using (var writer = new global::FastStream.FastMemoryWriter(this.pool))
            {
                TypePackWrite(writer, value);
                return writer.ToArray();
            }
        }

        public byte[] Serialize(global::UnityEngine.Vector2 value)
        {
            using (var writer = new global::FastStream.FastMemoryWriter(this.pool))
            {
                TypePackWrite(writer, value);
                return writer.ToArray();
            }
        }

        public byte[] Serialize(global::UnityEngine.Vector3 value)
        {
            using (var writer = new global::FastStream.FastMemoryWriter(this.pool))
            {
                TypePackWrite(writer, value);
                return writer.ToArray();
            }
        }

        public byte[] Serialize(global::UnityEngine.Quaternion value)
        {
            using (var writer = new global::FastStream.FastMemoryWriter(this.pool))
            {
                TypePackWrite(writer, value);
                return writer.ToArray();
            }
        }

        public byte[] Serialize(global::UnityEngine.Color value)
        {
            using (var writer = new global::FastStream.FastMemoryWriter(this.pool))
            {
                TypePackWrite(writer, value);
                return writer.ToArray();
            }
        }

        public byte[] Serialize(global::TeamZ.Code.Game.Cutscenes.CutsceneActivatorState value)
        {
            using (var writer = new global::FastStream.FastMemoryWriter(this.pool))
            {
                TypePackWrite(writer, value);
                return writer.ToArray();
            }
        }

        public byte[] Serialize(global::TeamZ.GameSaving.States.ActivatorState value)
        {
            using (var writer = new global::FastStream.FastMemoryWriter(this.pool))
            {
                TypePackWrite(writer, value);
                return writer.ToArray();
            }
        }

        public byte[] Serialize(global::TeamZ.GameSaving.States.PortalState value)
        {
            using (var writer = new global::FastStream.FastMemoryWriter(this.pool))
            {
                TypePackWrite(writer, value);
                return writer.ToArray();
            }
        }

        public byte[] Serialize(global::TeamZ.GameSaving.States.DoorState value)
        {
            using (var writer = new global::FastStream.FastMemoryWriter(this.pool))
            {
                TypePackWrite(writer, value);
                return writer.ToArray();
            }
        }

        public byte[] Serialize(global::TeamZ.GameSaving.States.GeneratorState value)
        {
            using (var writer = new global::FastStream.FastMemoryWriter(this.pool))
            {
                TypePackWrite(writer, value);
                return writer.ToArray();
            }
        }


        private static void TypePackWrite(global::FastStream.FastMemoryWriter stream0, global::TeamZ.GameSaving.States.Charaters.LizardState value)
        {
            if (value == null)
            {
                stream0.Write(TypePackObjects.Null);
                return;
            }

            stream0.Write(TypePackObjects.TeamZ_GameSaving_States_Charaters_LizardState);
            stream0.TypePackWrite(value.Health);
            stream0.TypePackWrite(value.Armor);
            stream0.TypePackWrite(value.PunchDamage);
            stream0.TypePackWrite(value.KickDamage);
            stream0.TypePackWrite(value.Name);
            stream0.TypePackWrite(value.RunSpeed);
            stream0.TypePackWrite(value.CreepSpeed);
            stream0.TypePackWrite(value.StrikeSpeed);
            stream0.TypePackWrite(value.JumpSpeed);
            stream0.TypePackWrite(value.JumpForce);
        }

        private static global::TeamZ.GameSaving.States.Charaters.LizardState TypePackRead_TeamZ_GameSaving_States_Charaters_LizardState(global::FastStream.FastReader stream0)
        {
            var kind = stream0.ReadUInt16();
            if (kind == TypePackObjects.Null)
            {
                return default;
            }

            if (kind != TypePackObjects.TeamZ_GameSaving_States_Charaters_LizardState)
            {
                TypePackExceptions.WrongBytes();
                return default;
            }

            return TypePackRead_TeamZ_GameSaving_States_Charaters_LizardState_WithoutValidation(stream0);
        }

        private static global::TeamZ.GameSaving.States.Charaters.LizardState TypePackRead_TeamZ_GameSaving_States_Charaters_LizardState_WithoutValidation(global::FastStream.FastReader stream0)
        {
            var result = new global::TeamZ.GameSaving.States.Charaters.LizardState();
            result.Health = stream0.TypePackRead_Int32();
            result.Armor = stream0.TypePackRead_Int32();
            result.PunchDamage = stream0.TypePackRead_Int32();
            result.KickDamage = stream0.TypePackRead_Int32();
            result.Name = stream0.TypePackRead_String();
            result.RunSpeed = stream0.TypePackRead_Single();
            result.CreepSpeed = stream0.TypePackRead_Single();
            result.StrikeSpeed = stream0.TypePackRead_Single();
            result.JumpSpeed = stream0.TypePackRead_Single();
            result.JumpForce = stream0.TypePackRead_Single();

            return result;
        }



        private static void TypePackWrite(global::FastStream.FastMemoryWriter stream0, global::TeamZ.GameSaving.States.Charaters.HedgehogState value)
        {
            if (value == null)
            {
                stream0.Write(TypePackObjects.Null);
                return;
            }

            stream0.Write(TypePackObjects.TeamZ_GameSaving_States_Charaters_HedgehogState);
            stream0.TypePackWrite(value.Health);
            stream0.TypePackWrite(value.Armor);
            stream0.TypePackWrite(value.PunchDamage);
            stream0.TypePackWrite(value.KickDamage);
            stream0.TypePackWrite(value.Name);
            stream0.TypePackWrite(value.RunSpeed);
            stream0.TypePackWrite(value.CreepSpeed);
            stream0.TypePackWrite(value.StrikeSpeed);
            stream0.TypePackWrite(value.JumpSpeed);
            stream0.TypePackWrite(value.JumpForce);
        }

        private static global::TeamZ.GameSaving.States.Charaters.HedgehogState TypePackRead_TeamZ_GameSaving_States_Charaters_HedgehogState(global::FastStream.FastReader stream0)
        {
            var kind = stream0.ReadUInt16();
            if (kind == TypePackObjects.Null)
            {
                return default;
            }

            if (kind != TypePackObjects.TeamZ_GameSaving_States_Charaters_HedgehogState)
            {
                TypePackExceptions.WrongBytes();
                return default;
            }

            return TypePackRead_TeamZ_GameSaving_States_Charaters_HedgehogState_WithoutValidation(stream0);
        }

        private static global::TeamZ.GameSaving.States.Charaters.HedgehogState TypePackRead_TeamZ_GameSaving_States_Charaters_HedgehogState_WithoutValidation(global::FastStream.FastReader stream0)
        {
            var result = new global::TeamZ.GameSaving.States.Charaters.HedgehogState();
            result.Health = stream0.TypePackRead_Int32();
            result.Armor = stream0.TypePackRead_Int32();
            result.PunchDamage = stream0.TypePackRead_Int32();
            result.KickDamage = stream0.TypePackRead_Int32();
            result.Name = stream0.TypePackRead_String();
            result.RunSpeed = stream0.TypePackRead_Single();
            result.CreepSpeed = stream0.TypePackRead_Single();
            result.StrikeSpeed = stream0.TypePackRead_Single();
            result.JumpSpeed = stream0.TypePackRead_Single();
            result.JumpForce = stream0.TypePackRead_Single();

            return result;
        }



        private static void TypePackWrite(global::FastStream.FastMemoryWriter stream0, global::TeamZ.GameSaving.States.CameraState value)
        {
            if (value == null)
            {
                stream0.Write(TypePackObjects.Null);
                return;
            }

            stream0.Write(TypePackObjects.TeamZ_GameSaving_States_CameraState);
            stream0.TypePackWrite(value.PlayerId);
            TypePackWrite(stream0, value.Position);
        }

        private static global::TeamZ.GameSaving.States.CameraState TypePackRead_TeamZ_GameSaving_States_CameraState(global::FastStream.FastReader stream0)
        {
            var kind = stream0.ReadUInt16();
            if (kind == TypePackObjects.Null)
            {
                return default;
            }

            if (kind != TypePackObjects.TeamZ_GameSaving_States_CameraState)
            {
                TypePackExceptions.WrongBytes();
                return default;
            }

            return TypePackRead_TeamZ_GameSaving_States_CameraState_WithoutValidation(stream0);
        }

        private static global::TeamZ.GameSaving.States.CameraState TypePackRead_TeamZ_GameSaving_States_CameraState_WithoutValidation(global::FastStream.FastReader stream0)
        {
            var result = new global::TeamZ.GameSaving.States.CameraState();
            result.PlayerId = stream0.TypePackRead_Guid();
            result.Position = TypePackRead_UnityEngine_Vector3(stream0);

            return result;
        }



        private static void TypePackWrite(global::FastStream.FastMemoryWriter stream0, global::TeamZ.GameSaving.States.CharacterControllerState value)
        {
            if (value == null)
            {
                stream0.Write(TypePackObjects.Null);
                return;
            }

            stream0.Write(TypePackObjects.TeamZ_GameSaving_States_CharacterControllerState);
            stream0.TypePackWrite((int)value.CurrentDirection);
            stream0.TypePackWrite(value.IsClimbed);
            stream0.TypePackWrite(value.IsKeyUpWasPressed);
        }

        private static global::TeamZ.GameSaving.States.CharacterControllerState TypePackRead_TeamZ_GameSaving_States_CharacterControllerState(global::FastStream.FastReader stream0)
        {
            var kind = stream0.ReadUInt16();
            if (kind == TypePackObjects.Null)
            {
                return default;
            }

            if (kind != TypePackObjects.TeamZ_GameSaving_States_CharacterControllerState)
            {
                TypePackExceptions.WrongBytes();
                return default;
            }

            return TypePackRead_TeamZ_GameSaving_States_CharacterControllerState_WithoutValidation(stream0);
        }

        private static global::TeamZ.GameSaving.States.CharacterControllerState TypePackRead_TeamZ_GameSaving_States_CharacterControllerState_WithoutValidation(global::FastStream.FastReader stream0)
        {
            var result = new global::TeamZ.GameSaving.States.CharacterControllerState();
            result.CurrentDirection = (global::TeamZ.Code.Game.Characters.CharacterControllerScript.Direction)stream0.TypePackRead_Int32();
            result.IsClimbed = stream0.TypePackRead_Boolean();
            result.IsKeyUpWasPressed = stream0.TypePackRead_Boolean();

            return result;
        }



        private static void TypePackWrite(global::FastStream.FastMemoryWriter stream0, global::TeamZ.GameSaving.States.EntityState value)
        {
            if (value == null)
            {
                stream0.Write(TypePackObjects.Null);
                return;
            }

            stream0.Write(TypePackObjects.TeamZ_GameSaving_States_EntityState);
            stream0.TypePackWrite(value.Id);
            stream0.TypePackWrite(value.AssetGuid);
            TypePackWrite(stream0, value.Scale);
            TypePackWrite(stream0, value.Rotation);
            TypePackWrite(stream0, value.Position);
            stream0.TypePackWrite(value.LevelId);
        }

        private static global::TeamZ.GameSaving.States.EntityState TypePackRead_TeamZ_GameSaving_States_EntityState(global::FastStream.FastReader stream0)
        {
            var kind = stream0.ReadUInt16();
            if (kind == TypePackObjects.Null)
            {
                return default;
            }

            if (kind != TypePackObjects.TeamZ_GameSaving_States_EntityState)
            {
                TypePackExceptions.WrongBytes();
                return default;
            }

            return TypePackRead_TeamZ_GameSaving_States_EntityState_WithoutValidation(stream0);
        }

        private static global::TeamZ.GameSaving.States.EntityState TypePackRead_TeamZ_GameSaving_States_EntityState_WithoutValidation(global::FastStream.FastReader stream0)
        {
            var result = new global::TeamZ.GameSaving.States.EntityState();
            result.Id = stream0.TypePackRead_Guid();
            result.AssetGuid = stream0.TypePackRead_String();
            result.Scale = TypePackRead_UnityEngine_Vector3(stream0);
            result.Rotation = TypePackRead_UnityEngine_Quaternion(stream0);
            result.Position = TypePackRead_UnityEngine_Vector3(stream0);
            result.LevelId = stream0.TypePackRead_Guid();

            return result;
        }



        private static void TypePackWrite(global::FastStream.FastMemoryWriter stream0, global::TeamZ.GameSaving.States.GameObjectState value)
        {
            if (value == null)
            {
                stream0.Write(TypePackObjects.Null);
                return;
            }

            stream0.Write(TypePackObjects.TeamZ_GameSaving_States_GameObjectState);
            TypePackWrite(stream0, value.Entity);
            stream0.TypePackWrite(value.MonoBehaviousStates, (stream1, value1) => TypePackWrite(stream1, value1));
        }

        private static global::TeamZ.GameSaving.States.GameObjectState TypePackRead_TeamZ_GameSaving_States_GameObjectState(global::FastStream.FastReader stream0)
        {
            var kind = stream0.ReadUInt16();
            if (kind == TypePackObjects.Null)
            {
                return default;
            }

            if (kind != TypePackObjects.TeamZ_GameSaving_States_GameObjectState)
            {
                TypePackExceptions.WrongBytes();
                return default;
            }

            return TypePackRead_TeamZ_GameSaving_States_GameObjectState_WithoutValidation(stream0);
        }

        private static global::TeamZ.GameSaving.States.GameObjectState TypePackRead_TeamZ_GameSaving_States_GameObjectState_WithoutValidation(global::FastStream.FastReader stream0)
        {
            var result = new global::TeamZ.GameSaving.States.GameObjectState();
            result.Entity = TypePackRead_TeamZ_GameSaving_States_EntityState(stream0);
            result.MonoBehaviousStates = stream0.TypePackReadArray(stream1 => TypePackRead_TeamZ_GameSaving_States_MonoBehaviourState(stream1));

            return result;
        }



        private static void TypePackWrite(global::FastStream.FastMemoryWriter stream0, global::TeamZ.GameSaving.States.GameState value)
        {
            if (value == null)
            {
                stream0.Write(TypePackObjects.Null);
                return;
            }

            stream0.Write(TypePackObjects.TeamZ_GameSaving_States_GameState);
            stream0.TypePackWrite(value.LevelId);
            stream0.TypePackWrite(value.GameObjectsStates, (stream1, value1) => TypePackWrite(stream1, value1));
            stream0.TypePackWrite(value.VisitedLevels, (stream1, value1) => stream1.TypePackWrite(value1));
            TypePackWrite(stream0, value.PlayerServiceState);
        }

        private static global::TeamZ.GameSaving.States.GameState TypePackRead_TeamZ_GameSaving_States_GameState(global::FastStream.FastReader stream0)
        {
            var kind = stream0.ReadUInt16();
            if (kind == TypePackObjects.Null)
            {
                return default;
            }

            if (kind != TypePackObjects.TeamZ_GameSaving_States_GameState)
            {
                TypePackExceptions.WrongBytes();
                return default;
            }

            return TypePackRead_TeamZ_GameSaving_States_GameState_WithoutValidation(stream0);
        }

        private static global::TeamZ.GameSaving.States.GameState TypePackRead_TeamZ_GameSaving_States_GameState_WithoutValidation(global::FastStream.FastReader stream0)
        {
            var result = new global::TeamZ.GameSaving.States.GameState();
            result.LevelId = stream0.TypePackRead_Guid();
            result.GameObjectsStates = stream0.TypePackReadCollection<global::System.Collections.Generic.List<global::TeamZ.GameSaving.States.GameObjectState>, global::TeamZ.GameSaving.States.GameObjectState>(values => new global::System.Collections.Generic.List<global::TeamZ.GameSaving.States.GameObjectState>(values), stream1 => TypePackRead_TeamZ_GameSaving_States_GameObjectState(stream1));
            result.VisitedLevels = stream0.TypePackReadCollection<global::System.Collections.Generic.HashSet<global::System.Guid>, global::System.Guid>(values => new global::System.Collections.Generic.HashSet<global::System.Guid>(values), stream1 => stream1.TypePackRead_Guid());
            result.PlayerServiceState = TypePackRead_TeamZ_Code_Game_Players_PlayerServiceState(stream0);

            return result;
        }



        private static void TypePackWrite(global::FastStream.FastMemoryWriter stream0, global::TeamZ.GameSaving.States.LevelObjectState value)
        {
            if (value == null)
            {
                stream0.Write(TypePackObjects.Null);
                return;
            }

            stream0.Write(TypePackObjects.TeamZ_GameSaving_States_LevelObjectState);
            stream0.TypePackWrite(value.Strength);
            stream0.TypePackWrite(value.IsDestructible);
            stream0.TypePackWrite(value.IsOnlyMovable);
            TypePackWrite(stream0, value.HighlightingColor);
            stream0.TypePackWrite(value.IsAlreadyExploded);
        }

        private static global::TeamZ.GameSaving.States.LevelObjectState TypePackRead_TeamZ_GameSaving_States_LevelObjectState(global::FastStream.FastReader stream0)
        {
            var kind = stream0.ReadUInt16();
            if (kind == TypePackObjects.Null)
            {
                return default;
            }

            if (kind != TypePackObjects.TeamZ_GameSaving_States_LevelObjectState)
            {
                TypePackExceptions.WrongBytes();
                return default;
            }

            return TypePackRead_TeamZ_GameSaving_States_LevelObjectState_WithoutValidation(stream0);
        }

        private static global::TeamZ.GameSaving.States.LevelObjectState TypePackRead_TeamZ_GameSaving_States_LevelObjectState_WithoutValidation(global::FastStream.FastReader stream0)
        {
            var result = new global::TeamZ.GameSaving.States.LevelObjectState();
            result.Strength = stream0.TypePackRead_Int32();
            result.IsDestructible = stream0.TypePackRead_Boolean();
            result.IsOnlyMovable = stream0.TypePackRead_Boolean();
            result.HighlightingColor = TypePackRead_UnityEngine_Vector3(stream0);
            result.IsAlreadyExploded = stream0.TypePackRead_Boolean();

            return result;
        }



        private static void TypePackWrite(global::FastStream.FastMemoryWriter stream0, global::TeamZ.GameSaving.States.LootBoxState value)
        {
            if (value == null)
            {
                stream0.Write(TypePackObjects.Null);
                return;
            }

            stream0.Write(TypePackObjects.TeamZ_GameSaving_States_LootBoxState);
            stream0.TypePackWrite(value.IsOpen);
            stream0.TypePackWrite(value.IsAvailable);
            stream0.TypePackWrite(value.IsNotEmpty);
        }

        private static global::TeamZ.GameSaving.States.LootBoxState TypePackRead_TeamZ_GameSaving_States_LootBoxState(global::FastStream.FastReader stream0)
        {
            var kind = stream0.ReadUInt16();
            if (kind == TypePackObjects.Null)
            {
                return default;
            }

            if (kind != TypePackObjects.TeamZ_GameSaving_States_LootBoxState)
            {
                TypePackExceptions.WrongBytes();
                return default;
            }

            return TypePackRead_TeamZ_GameSaving_States_LootBoxState_WithoutValidation(stream0);
        }

        private static global::TeamZ.GameSaving.States.LootBoxState TypePackRead_TeamZ_GameSaving_States_LootBoxState_WithoutValidation(global::FastStream.FastReader stream0)
        {
            var result = new global::TeamZ.GameSaving.States.LootBoxState();
            result.IsOpen = stream0.TypePackRead_Boolean();
            result.IsAvailable = stream0.TypePackRead_Boolean();
            result.IsNotEmpty = stream0.TypePackRead_Boolean();

            return result;
        }



        private static void TypePackWrite(global::FastStream.FastMemoryWriter stream0, global::TeamZ.Assets.GameSaving.States.RenderState value)
        {
            if (value == null)
            {
                stream0.Write(TypePackObjects.Null);
                return;
            }

            stream0.Write(TypePackObjects.TeamZ_Assets_GameSaving_States_RenderState);
            stream0.TypePackWrite(value.SortingLayerName);
            stream0.TypePackWrite(value.SortingLayerOrder);
        }

        private static global::TeamZ.Assets.GameSaving.States.RenderState TypePackRead_TeamZ_Assets_GameSaving_States_RenderState(global::FastStream.FastReader stream0)
        {
            var kind = stream0.ReadUInt16();
            if (kind == TypePackObjects.Null)
            {
                return default;
            }

            if (kind != TypePackObjects.TeamZ_Assets_GameSaving_States_RenderState)
            {
                TypePackExceptions.WrongBytes();
                return default;
            }

            return TypePackRead_TeamZ_Assets_GameSaving_States_RenderState_WithoutValidation(stream0);
        }

        private static global::TeamZ.Assets.GameSaving.States.RenderState TypePackRead_TeamZ_Assets_GameSaving_States_RenderState_WithoutValidation(global::FastStream.FastReader stream0)
        {
            var result = new global::TeamZ.Assets.GameSaving.States.RenderState();
            result.SortingLayerName = stream0.TypePackRead_String();
            result.SortingLayerOrder = stream0.TypePackRead_Int32();

            return result;
        }



        private static void TypePackWrite(global::FastStream.FastMemoryWriter stream0, global::TeamZ.Assets.UI.Speech.SpeechBubbleState value)
        {
            if (value == null)
            {
                stream0.Write(TypePackObjects.Null);
                return;
            }

            stream0.Write(TypePackObjects.TeamZ_Assets_UI_Speech_SpeechBubbleState);
            stream0.TypePackWrite(value.Messages, (stream1, value1) => stream1.TypePackWrite(value1));
            stream0.TypePackWrite(value.Duration);
            stream0.TypePackWrite((int)value.Character);
        }

        private static global::TeamZ.Assets.UI.Speech.SpeechBubbleState TypePackRead_TeamZ_Assets_UI_Speech_SpeechBubbleState(global::FastStream.FastReader stream0)
        {
            var kind = stream0.ReadUInt16();
            if (kind == TypePackObjects.Null)
            {
                return default;
            }

            if (kind != TypePackObjects.TeamZ_Assets_UI_Speech_SpeechBubbleState)
            {
                TypePackExceptions.WrongBytes();
                return default;
            }

            return TypePackRead_TeamZ_Assets_UI_Speech_SpeechBubbleState_WithoutValidation(stream0);
        }

        private static global::TeamZ.Assets.UI.Speech.SpeechBubbleState TypePackRead_TeamZ_Assets_UI_Speech_SpeechBubbleState_WithoutValidation(global::FastStream.FastReader stream0)
        {
            var result = new global::TeamZ.Assets.UI.Speech.SpeechBubbleState();
            result.Messages = stream0.TypePackReadArray(stream1 => stream1.TypePackRead_String());
            result.Duration = stream0.TypePackRead_Single();
            result.Character = (global::TeamZ.Assets.UI.Speech.CharacterSpeechBubble.CharacterType)stream0.TypePackRead_Int32();

            return result;
        }



        private static void TypePackWrite(global::FastStream.FastMemoryWriter stream0, global::TeamZ.Code.Game.Players.PlayerServiceState value)
        {
            if (value == null)
            {
                stream0.Write(TypePackObjects.Null);
                return;
            }

            stream0.Write(TypePackObjects.TeamZ_Code_Game_Players_PlayerServiceState);
            stream0.TypePackWrite(value.FirstPlayerEntityId, (stream1, value1) => stream1.TypePackWrite(value1));
            stream0.TypePackWrite(value.SecondPlayerEntityId, (stream1, value1) => stream1.TypePackWrite(value1));
        }

        private static global::TeamZ.Code.Game.Players.PlayerServiceState TypePackRead_TeamZ_Code_Game_Players_PlayerServiceState(global::FastStream.FastReader stream0)
        {
            var kind = stream0.ReadUInt16();
            if (kind == TypePackObjects.Null)
            {
                return default;
            }

            if (kind != TypePackObjects.TeamZ_Code_Game_Players_PlayerServiceState)
            {
                TypePackExceptions.WrongBytes();
                return default;
            }

            return TypePackRead_TeamZ_Code_Game_Players_PlayerServiceState_WithoutValidation(stream0);
        }

        private static global::TeamZ.Code.Game.Players.PlayerServiceState TypePackRead_TeamZ_Code_Game_Players_PlayerServiceState_WithoutValidation(global::FastStream.FastReader stream0)
        {
            var result = new global::TeamZ.Code.Game.Players.PlayerServiceState();
            result.FirstPlayerEntityId = stream0.TypePackReadNullable(stream1 => stream1.TypePackRead_Guid());
            result.SecondPlayerEntityId = stream0.TypePackReadNullable(stream1 => stream1.TypePackRead_Guid());

            return result;
        }



        private static void TypePackWrite(global::FastStream.FastMemoryWriter stream0, global::UnityEngine.Vector2 value)
        {
            if (value == null)
            {
                stream0.Write(TypePackObjects.Null);
                return;
            }

            stream0.Write(TypePackObjects.UnityEngine_Vector2);
            stream0.TypePackWrite(value.x);
            stream0.TypePackWrite(value.y);
        }

        private static global::UnityEngine.Vector2 TypePackRead_UnityEngine_Vector2(global::FastStream.FastReader stream0)
        {
            var kind = stream0.ReadUInt16();
            if (kind == TypePackObjects.Null)
            {
                return default;
            }

            if (kind != TypePackObjects.UnityEngine_Vector2)
            {
                TypePackExceptions.WrongBytes();
                return default;
            }

            return TypePackRead_UnityEngine_Vector2_WithoutValidation(stream0);
        }

        private static global::UnityEngine.Vector2 TypePackRead_UnityEngine_Vector2_WithoutValidation(global::FastStream.FastReader stream0)
        {
            var result = new global::UnityEngine.Vector2();
            result.x = stream0.TypePackRead_Single();
            result.y = stream0.TypePackRead_Single();

            return result;
        }



        private static void TypePackWrite(global::FastStream.FastMemoryWriter stream0, global::UnityEngine.Vector3 value)
        {
            if (value == null)
            {
                stream0.Write(TypePackObjects.Null);
                return;
            }

            stream0.Write(TypePackObjects.UnityEngine_Vector3);
            stream0.TypePackWrite(value.x);
            stream0.TypePackWrite(value.y);
            stream0.TypePackWrite(value.z);
        }

        private static global::UnityEngine.Vector3 TypePackRead_UnityEngine_Vector3(global::FastStream.FastReader stream0)
        {
            var kind = stream0.ReadUInt16();
            if (kind == TypePackObjects.Null)
            {
                return default;
            }

            if (kind != TypePackObjects.UnityEngine_Vector3)
            {
                TypePackExceptions.WrongBytes();
                return default;
            }

            return TypePackRead_UnityEngine_Vector3_WithoutValidation(stream0);
        }

        private static global::UnityEngine.Vector3 TypePackRead_UnityEngine_Vector3_WithoutValidation(global::FastStream.FastReader stream0)
        {
            var result = new global::UnityEngine.Vector3();
            result.x = stream0.TypePackRead_Single();
            result.y = stream0.TypePackRead_Single();
            result.z = stream0.TypePackRead_Single();

            return result;
        }



        private static void TypePackWrite(global::FastStream.FastMemoryWriter stream0, global::UnityEngine.Quaternion value)
        {
            if (value == null)
            {
                stream0.Write(TypePackObjects.Null);
                return;
            }

            stream0.Write(TypePackObjects.UnityEngine_Quaternion);
            stream0.TypePackWrite(value.x);
            stream0.TypePackWrite(value.y);
            stream0.TypePackWrite(value.z);
            stream0.TypePackWrite(value.w);
        }

        private static global::UnityEngine.Quaternion TypePackRead_UnityEngine_Quaternion(global::FastStream.FastReader stream0)
        {
            var kind = stream0.ReadUInt16();
            if (kind == TypePackObjects.Null)
            {
                return default;
            }

            if (kind != TypePackObjects.UnityEngine_Quaternion)
            {
                TypePackExceptions.WrongBytes();
                return default;
            }

            return TypePackRead_UnityEngine_Quaternion_WithoutValidation(stream0);
        }

        private static global::UnityEngine.Quaternion TypePackRead_UnityEngine_Quaternion_WithoutValidation(global::FastStream.FastReader stream0)
        {
            var result = new global::UnityEngine.Quaternion();
            result.x = stream0.TypePackRead_Single();
            result.y = stream0.TypePackRead_Single();
            result.z = stream0.TypePackRead_Single();
            result.w = stream0.TypePackRead_Single();

            return result;
        }



        private static void TypePackWrite(global::FastStream.FastMemoryWriter stream0, global::UnityEngine.Color value)
        {
            if (value == null)
            {
                stream0.Write(TypePackObjects.Null);
                return;
            }

            stream0.Write(TypePackObjects.UnityEngine_Color);
            stream0.TypePackWrite(value.r);
            stream0.TypePackWrite(value.g);
            stream0.TypePackWrite(value.b);
            stream0.TypePackWrite(value.a);
        }

        private static global::UnityEngine.Color TypePackRead_UnityEngine_Color(global::FastStream.FastReader stream0)
        {
            var kind = stream0.ReadUInt16();
            if (kind == TypePackObjects.Null)
            {
                return default;
            }

            if (kind != TypePackObjects.UnityEngine_Color)
            {
                TypePackExceptions.WrongBytes();
                return default;
            }

            return TypePackRead_UnityEngine_Color_WithoutValidation(stream0);
        }

        private static global::UnityEngine.Color TypePackRead_UnityEngine_Color_WithoutValidation(global::FastStream.FastReader stream0)
        {
            var result = new global::UnityEngine.Color();
            result.r = stream0.TypePackRead_Single();
            result.g = stream0.TypePackRead_Single();
            result.b = stream0.TypePackRead_Single();
            result.a = stream0.TypePackRead_Single();

            return result;
        }



        private static void TypePackWrite(global::FastStream.FastMemoryWriter stream0, global::TeamZ.Code.Game.Cutscenes.CutsceneActivatorState value)
        {
            if (value == null)
            {
                stream0.Write(TypePackObjects.Null);
                return;
            }

            stream0.Write(TypePackObjects.TeamZ_Code_Game_Cutscenes_CutsceneActivatorState);
            stream0.TypePackWrite(value.AssetGuid);
        }

        private static global::TeamZ.Code.Game.Cutscenes.CutsceneActivatorState TypePackRead_TeamZ_Code_Game_Cutscenes_CutsceneActivatorState(global::FastStream.FastReader stream0)
        {
            var kind = stream0.ReadUInt16();
            if (kind == TypePackObjects.Null)
            {
                return default;
            }

            if (kind != TypePackObjects.TeamZ_Code_Game_Cutscenes_CutsceneActivatorState)
            {
                TypePackExceptions.WrongBytes();
                return default;
            }

            return TypePackRead_TeamZ_Code_Game_Cutscenes_CutsceneActivatorState_WithoutValidation(stream0);
        }

        private static global::TeamZ.Code.Game.Cutscenes.CutsceneActivatorState TypePackRead_TeamZ_Code_Game_Cutscenes_CutsceneActivatorState_WithoutValidation(global::FastStream.FastReader stream0)
        {
            var result = new global::TeamZ.Code.Game.Cutscenes.CutsceneActivatorState();
            result.AssetGuid = stream0.TypePackRead_String();

            return result;
        }



        private static void TypePackWrite(global::FastStream.FastMemoryWriter stream0, global::TeamZ.GameSaving.States.ActivatorState value)
        {
            if (value == null)
            {
                stream0.Write(TypePackObjects.Null);
                return;
            }

            stream0.Write(TypePackObjects.TeamZ_GameSaving_States_ActivatorState);
            stream0.TypePackWrite(value.IsActive);
            stream0.TypePackWrite(value.Name);
            stream0.TypePackWrite(value.IsActivated);
        }

        private static global::TeamZ.GameSaving.States.ActivatorState TypePackRead_TeamZ_GameSaving_States_ActivatorState(global::FastStream.FastReader stream0)
        {
            var kind = stream0.ReadUInt16();
            if (kind == TypePackObjects.Null)
            {
                return default;
            }

            if (kind != TypePackObjects.TeamZ_GameSaving_States_ActivatorState)
            {
                TypePackExceptions.WrongBytes();
                return default;
            }

            return TypePackRead_TeamZ_GameSaving_States_ActivatorState_WithoutValidation(stream0);
        }

        private static global::TeamZ.GameSaving.States.ActivatorState TypePackRead_TeamZ_GameSaving_States_ActivatorState_WithoutValidation(global::FastStream.FastReader stream0)
        {
            var result = new global::TeamZ.GameSaving.States.ActivatorState();
            result.IsActive = stream0.TypePackRead_Boolean();
            result.Name = stream0.TypePackRead_String();
            result.IsActivated = stream0.TypePackRead_Boolean();

            return result;
        }



        private static void TypePackWrite(global::FastStream.FastMemoryWriter stream0, global::TeamZ.GameSaving.States.PortalState value)
        {
            if (value == null)
            {
                stream0.Write(TypePackObjects.Null);
                return;
            }

            stream0.Write(TypePackObjects.TeamZ_GameSaving_States_PortalState);
            stream0.TypePackWrite(value.IsActive);
            stream0.TypePackWrite(value.SceneName);
            stream0.TypePackWrite(value.Location);
            stream0.TypePackWrite(value.Name);
        }

        private static global::TeamZ.GameSaving.States.PortalState TypePackRead_TeamZ_GameSaving_States_PortalState(global::FastStream.FastReader stream0)
        {
            var kind = stream0.ReadUInt16();
            if (kind == TypePackObjects.Null)
            {
                return default;
            }

            if (kind != TypePackObjects.TeamZ_GameSaving_States_PortalState)
            {
                TypePackExceptions.WrongBytes();
                return default;
            }

            return TypePackRead_TeamZ_GameSaving_States_PortalState_WithoutValidation(stream0);
        }

        private static global::TeamZ.GameSaving.States.PortalState TypePackRead_TeamZ_GameSaving_States_PortalState_WithoutValidation(global::FastStream.FastReader stream0)
        {
            var result = new global::TeamZ.GameSaving.States.PortalState();
            result.IsActive = stream0.TypePackRead_Boolean();
            result.SceneName = stream0.TypePackRead_String();
            result.Location = stream0.TypePackRead_String();
            result.Name = stream0.TypePackRead_String();

            return result;
        }



        private static void TypePackWrite(global::FastStream.FastMemoryWriter stream0, global::TeamZ.GameSaving.States.DoorState value)
        {
            if (value == null)
            {
                stream0.Write(TypePackObjects.Null);
                return;
            }

            stream0.Write(TypePackObjects.TeamZ_GameSaving_States_DoorState);
            stream0.TypePackWrite(value.Name);
            stream0.TypePackWrite((int)value.Type);
            stream0.TypePackWrite((int)value.State);
        }

        private static global::TeamZ.GameSaving.States.DoorState TypePackRead_TeamZ_GameSaving_States_DoorState(global::FastStream.FastReader stream0)
        {
            var kind = stream0.ReadUInt16();
            if (kind == TypePackObjects.Null)
            {
                return default;
            }

            if (kind != TypePackObjects.TeamZ_GameSaving_States_DoorState)
            {
                TypePackExceptions.WrongBytes();
                return default;
            }

            return TypePackRead_TeamZ_GameSaving_States_DoorState_WithoutValidation(stream0);
        }

        private static global::TeamZ.GameSaving.States.DoorState TypePackRead_TeamZ_GameSaving_States_DoorState_WithoutValidation(global::FastStream.FastReader stream0)
        {
            var result = new global::TeamZ.GameSaving.States.DoorState();
            result.Name = stream0.TypePackRead_String();
            result.Type = (global::TeamZ.Code.Game.Levels.DoorScript.DoorType)stream0.TypePackRead_Int32();
            result.State = (global::TeamZ.Code.Game.Levels.DoorScript.DoorState)stream0.TypePackRead_Int32();

            return result;
        }



        private static void TypePackWrite(global::FastStream.FastMemoryWriter stream0, global::TeamZ.GameSaving.States.GeneratorState value)
        {
            if (value == null)
            {
                stream0.Write(TypePackObjects.Null);
                return;
            }

            stream0.Write(TypePackObjects.TeamZ_GameSaving_States_GeneratorState);
            stream0.TypePackWrite(value.Name);
            stream0.TypePackWrite(value.IsActive);
            stream0.TypePackWrite(value.IsActivated);
            stream0.TypePackWrite(value.DoorId);
            stream0.TypePackWrite(value.PortalId);
        }

        private static global::TeamZ.GameSaving.States.GeneratorState TypePackRead_TeamZ_GameSaving_States_GeneratorState(global::FastStream.FastReader stream0)
        {
            var kind = stream0.ReadUInt16();
            if (kind == TypePackObjects.Null)
            {
                return default;
            }

            if (kind != TypePackObjects.TeamZ_GameSaving_States_GeneratorState)
            {
                TypePackExceptions.WrongBytes();
                return default;
            }

            return TypePackRead_TeamZ_GameSaving_States_GeneratorState_WithoutValidation(stream0);
        }

        private static global::TeamZ.GameSaving.States.GeneratorState TypePackRead_TeamZ_GameSaving_States_GeneratorState_WithoutValidation(global::FastStream.FastReader stream0)
        {
            var result = new global::TeamZ.GameSaving.States.GeneratorState();
            result.Name = stream0.TypePackRead_String();
            result.IsActive = stream0.TypePackRead_Boolean();
            result.IsActivated = stream0.TypePackRead_Boolean();
            result.DoorId = stream0.TypePackRead_Guid();
            result.PortalId = stream0.TypePackRead_Guid();

            return result;
        }




        private static global::System.Collections.Generic.Dictionary<Type, Action<global::FastStream.FastMemoryWriter, object>> writeMappingsForTeamZ_GameSaving_States_MonoBehaviourState
            = new global::System.Collections.Generic.Dictionary<Type, Action<global::FastStream.FastMemoryWriter, object>>
        {
            { typeof(global::TeamZ.GameSaving.States.Charaters.LizardState), (stream, value) => TypePackWrite(stream, (global::TeamZ.GameSaving.States.Charaters.LizardState)value) },
            { typeof(global::TeamZ.GameSaving.States.Charaters.HedgehogState), (stream, value) => TypePackWrite(stream, (global::TeamZ.GameSaving.States.Charaters.HedgehogState)value) },
            { typeof(global::TeamZ.GameSaving.States.CameraState), (stream, value) => TypePackWrite(stream, (global::TeamZ.GameSaving.States.CameraState)value) },
            { typeof(global::TeamZ.GameSaving.States.CharacterControllerState), (stream, value) => TypePackWrite(stream, (global::TeamZ.GameSaving.States.CharacterControllerState)value) },
            { typeof(global::TeamZ.GameSaving.States.EntityState), (stream, value) => TypePackWrite(stream, (global::TeamZ.GameSaving.States.EntityState)value) },
            { typeof(global::TeamZ.GameSaving.States.LevelObjectState), (stream, value) => TypePackWrite(stream, (global::TeamZ.GameSaving.States.LevelObjectState)value) },
            { typeof(global::TeamZ.GameSaving.States.LootBoxState), (stream, value) => TypePackWrite(stream, (global::TeamZ.GameSaving.States.LootBoxState)value) },
            { typeof(global::TeamZ.Assets.GameSaving.States.RenderState), (stream, value) => TypePackWrite(stream, (global::TeamZ.Assets.GameSaving.States.RenderState)value) },
            { typeof(global::TeamZ.Assets.UI.Speech.SpeechBubbleState), (stream, value) => TypePackWrite(stream, (global::TeamZ.Assets.UI.Speech.SpeechBubbleState)value) },
            { typeof(global::TeamZ.Code.Game.Cutscenes.CutsceneActivatorState), (stream, value) => TypePackWrite(stream, (global::TeamZ.Code.Game.Cutscenes.CutsceneActivatorState)value) },
            { typeof(global::TeamZ.GameSaving.States.ActivatorState), (stream, value) => TypePackWrite(stream, (global::TeamZ.GameSaving.States.ActivatorState)value) },
            { typeof(global::TeamZ.GameSaving.States.PortalState), (stream, value) => TypePackWrite(stream, (global::TeamZ.GameSaving.States.PortalState)value) },
            { typeof(global::TeamZ.GameSaving.States.DoorState), (stream, value) => TypePackWrite(stream, (global::TeamZ.GameSaving.States.DoorState)value) },
            { typeof(global::TeamZ.GameSaving.States.GeneratorState), (stream, value) => TypePackWrite(stream, (global::TeamZ.GameSaving.States.GeneratorState)value) },
        };
        private static void TypePackWrite(global::FastStream.FastMemoryWriter stream, global::TeamZ.GameSaving.States.MonoBehaviourState value)
        {
            if (value == null)
            {
                stream.Write(TypePackObjects.Null);
                return;
            }

            writeMappingsForTeamZ_GameSaving_States_MonoBehaviourState[value.GetType()](stream, value);
        }

        private static global::System.Collections.Generic.Dictionary<ushort, Func<global::FastStream.FastReader, object>> readMappingsForTeamZ_GameSaving_States_MonoBehaviourState
            = new global::System.Collections.Generic.Dictionary<ushort, Func<global::FastStream.FastReader, object>>
        {
            { TypePackObjects.TeamZ_GameSaving_States_Charaters_LizardState, stream => TypePackRead_TeamZ_GameSaving_States_Charaters_LizardState_WithoutValidation(stream)},
            { TypePackObjects.TeamZ_GameSaving_States_Charaters_HedgehogState, stream => TypePackRead_TeamZ_GameSaving_States_Charaters_HedgehogState_WithoutValidation(stream)},
            { TypePackObjects.TeamZ_GameSaving_States_CameraState, stream => TypePackRead_TeamZ_GameSaving_States_CameraState_WithoutValidation(stream)},
            { TypePackObjects.TeamZ_GameSaving_States_CharacterControllerState, stream => TypePackRead_TeamZ_GameSaving_States_CharacterControllerState_WithoutValidation(stream)},
            { TypePackObjects.TeamZ_GameSaving_States_EntityState, stream => TypePackRead_TeamZ_GameSaving_States_EntityState_WithoutValidation(stream)},
            { TypePackObjects.TeamZ_GameSaving_States_LevelObjectState, stream => TypePackRead_TeamZ_GameSaving_States_LevelObjectState_WithoutValidation(stream)},
            { TypePackObjects.TeamZ_GameSaving_States_LootBoxState, stream => TypePackRead_TeamZ_GameSaving_States_LootBoxState_WithoutValidation(stream)},
            { TypePackObjects.TeamZ_Assets_GameSaving_States_RenderState, stream => TypePackRead_TeamZ_Assets_GameSaving_States_RenderState_WithoutValidation(stream)},
            { TypePackObjects.TeamZ_Assets_UI_Speech_SpeechBubbleState, stream => TypePackRead_TeamZ_Assets_UI_Speech_SpeechBubbleState_WithoutValidation(stream)},
            { TypePackObjects.TeamZ_Code_Game_Cutscenes_CutsceneActivatorState, stream => TypePackRead_TeamZ_Code_Game_Cutscenes_CutsceneActivatorState_WithoutValidation(stream)},
            { TypePackObjects.TeamZ_GameSaving_States_ActivatorState, stream => TypePackRead_TeamZ_GameSaving_States_ActivatorState_WithoutValidation(stream)},
            { TypePackObjects.TeamZ_GameSaving_States_PortalState, stream => TypePackRead_TeamZ_GameSaving_States_PortalState_WithoutValidation(stream)},
            { TypePackObjects.TeamZ_GameSaving_States_DoorState, stream => TypePackRead_TeamZ_GameSaving_States_DoorState_WithoutValidation(stream)},
            { TypePackObjects.TeamZ_GameSaving_States_GeneratorState, stream => TypePackRead_TeamZ_GameSaving_States_GeneratorState_WithoutValidation(stream)},
        };
        private static global::TeamZ.GameSaving.States.MonoBehaviourState TypePackRead_TeamZ_GameSaving_States_MonoBehaviourState(global::FastStream.FastReader stream)
        {
            var kind = stream.ReadUInt16();
            if (kind == TypePackObjects.Null)
            {
                return default;
            }

            return (global::TeamZ.GameSaving.States.MonoBehaviourState)readMappingsForTeamZ_GameSaving_States_MonoBehaviourState[kind](stream);
        }


        private static global::System.Collections.Generic.Dictionary<Type, Action<global::FastStream.FastMemoryWriter, object>> writeMappingsForTeamZ_GameSaving_Interfaces_State
            = new global::System.Collections.Generic.Dictionary<Type, Action<global::FastStream.FastMemoryWriter, object>>
        {
            { typeof(global::TeamZ.Code.Game.Players.PlayerServiceState), (stream, value) => TypePackWrite(stream, (global::TeamZ.Code.Game.Players.PlayerServiceState)value) },
        };
        private static void TypePackWrite(global::FastStream.FastMemoryWriter stream, global::TeamZ.GameSaving.Interfaces.State value)
        {
            if (value == null)
            {
                stream.Write(TypePackObjects.Null);
                return;
            }

            writeMappingsForTeamZ_GameSaving_Interfaces_State[value.GetType()](stream, value);
        }

        private static global::System.Collections.Generic.Dictionary<ushort, Func<global::FastStream.FastReader, object>> readMappingsForTeamZ_GameSaving_Interfaces_State
            = new global::System.Collections.Generic.Dictionary<ushort, Func<global::FastStream.FastReader, object>>
        {
            { TypePackObjects.TeamZ_Code_Game_Players_PlayerServiceState, stream => TypePackRead_TeamZ_Code_Game_Players_PlayerServiceState_WithoutValidation(stream)},
        };
        private static global::TeamZ.GameSaving.Interfaces.State TypePackRead_TeamZ_GameSaving_Interfaces_State(global::FastStream.FastReader stream)
        {
            var kind = stream.ReadUInt16();
            if (kind == TypePackObjects.Null)
            {
                return default;
            }

            return (global::TeamZ.GameSaving.Interfaces.State)readMappingsForTeamZ_GameSaving_Interfaces_State[kind](stream);
        }

    }
}
