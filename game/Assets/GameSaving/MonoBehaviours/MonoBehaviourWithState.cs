﻿using System;
using TeamZ.GameSaving.Interfaces;
using TeamZ.GameSaving.States;
using UnityEngine;

namespace TeamZ.GameSaving.MonoBehaviours
{
    public abstract class MonoBehaviourWithState<TState> : MonoBehaviour, IMonoBehaviourWithState<TState>, IMonoBehaviourWithState
        where TState : MonoBehaviourState
    {
        MonoBehaviourState IMonoBehaviourWithState.GetState()
        {
            return this.GetState();
        }

        public abstract TState GetState();

        public Type GetStateType()
        {
            return typeof(TState);
        }

        void IMonoBehaviourWithState.SetState(MonoBehaviourState state)
        {
            this.SetState((TState)state);
        }

        public abstract void SetState(TState state);

        public virtual void Loaded()
        {

        }
    }
}