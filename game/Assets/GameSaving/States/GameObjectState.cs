using System.Collections.Generic;
using System.Linq;
using TeamZ.GameSaving.Interfaces;
using UnityEngine;

namespace TeamZ.GameSaving.States
{
    public class GameObjectState
    {
        public EntityState Entity
        {
            get;
            set;
        }

        public IEnumerable<MonoBehaviourState> MonoBehaviousStates
        {
            get;
            set;
        }

        public GameObjectState()
        {
        }

        public GameObjectState SetGameObject(GameObject gameObject)
        {
            var states = gameObject.GetComponents<IMonoBehaviourWithState>().
                                    Select(o => (MonoBehaviourState)o.GetState()).ToList();

            this.Entity = (EntityState)states.First(o => o is EntityState);
            states.Remove(this.Entity);
            this.MonoBehaviousStates = states;

            return this;
        }
    }
}