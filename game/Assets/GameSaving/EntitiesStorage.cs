using System;
using TeamZ.Code.Game.Characters.Lizard;
using TeamZ.Code.Helpers;
using TeamZ.GameSaving.MonoBehaviours;
using UniRx;
using UnityEngine;

namespace TeamZ.GameSaving
{
    public class EntitiesStorage
    {
        public EntitiesStorage()
        {
            this.Entities = new ReactiveDictionary<Guid, Entity>();
            MessageBroker.Default.Receive<Entity.EntityDestroyed>().Subscribe(o => this.Entities.Remove(o.Entity.Id));
        }

        public GameObject Root
        {
            get;
            set;
        }


        public UnityDependency<Lizard> Lizard { get; }
        public UnityDependency<Lizard> Hedgehog { get;  }

        public ReactiveDictionary<Guid, Entity> Entities
        {
            get;
        }
    }
}
