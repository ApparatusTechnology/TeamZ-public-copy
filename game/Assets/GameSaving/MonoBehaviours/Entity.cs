using System;
using TeamZ.GameSaving.States;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TeamZ.GameSaving.MonoBehaviours
{
    public class Entity : MonoBehaviourWithState<EntityState>
    {
        public Guid Id = Guid.NewGuid();
        public Guid LevelId;
        
        [AssetReferenceUILabelRestriction("Entity")]
        public AssetReference Reference;


#if UNITY_EDITOR
        private void Start()
        {
            if (string.IsNullOrWhiteSpace(this.Reference?.AssetGUID))
            {
                Debug.LogError("Provide proper entity path. Check failed for: " + this.gameObject.name);
                return;
            }

            if (this.gameObject.transform.root?.name != "Root")
            {
                Debug.LogError("Place all entities in 'Root' game object. Check failed for: " + this.gameObject.name);
                return;
            }
        }
#endif


        public override EntityState GetState()
        {
            if (string.IsNullOrWhiteSpace(this.Reference?.AssetGUID))
            {
                Debug.LogError("Provide proper entity path. Check failed for: " + this.gameObject.name);
            }
            
            var cachedTransform = this.transform;
            return new EntityState
            {
                Id = this.Id,
                LevelId = this.LevelId,
                AssetGuid = this.Reference.AssetGUID,
                Position = cachedTransform.localPosition,
                Rotation = cachedTransform.localRotation,
                Scale = cachedTransform.localScale,
            };
        }

        public override void SetState(EntityState state)
        {
            this.Id = state.Id;
            this.Reference  = new AssetReference(state.AssetGuid);
            this.LevelId = state.LevelId;

            var cachedTransform = this.transform;
            cachedTransform.localPosition = state.Position;
            cachedTransform.localRotation = state.Rotation;
            cachedTransform.localScale = state.Scale;
        }

        public class EntityDestroyed
        {
            public Entity Entity { get; set; }
        }
        
        public class EntityCreated
        {
            public Entity Entity { get; set; }
        }

        private void Awake()
        {
            MessageBroker.Default.Publish(new EntityCreated { Entity = this });
        }

        private void OnDestroy()
        {
            MessageBroker.Default.Publish(new EntityDestroyed { Entity = this });
        }
    }

}