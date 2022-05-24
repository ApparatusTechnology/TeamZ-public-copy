using System;
using TeamZ.Code.DependencyInjection;
using UnityEngine;

namespace TeamZ.GameSaving.MonoBehaviours
{
    [Serializable]
    public class EntityReference
    {
        private Dependency<EntitiesStorage> entities;
        
        private Guid entityId;
        
        [SerializeField]
        private Entity entity;

        public EntityReference()
        {
            
        }

        public EntityReference(Guid entityId)
        {
            this.entityId = entityId;
        }

        public Entity Entity
        {
            get
            {
                if (!this.entity)
                {
                    this.entity = this.entities.Value.Entities[this.entityId];
                }
                
                return entity;
            }
        }
    }
}