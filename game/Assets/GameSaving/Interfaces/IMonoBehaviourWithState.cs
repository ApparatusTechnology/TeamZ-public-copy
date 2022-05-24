﻿using System;
using TeamZ.GameSaving.States;

namespace TeamZ.GameSaving.Interfaces
{
    public interface IMonoBehaviourWithState
    {
        Type GetStateType();

        void SetState(MonoBehaviourState state);
        MonoBehaviourState GetState();


        void Loaded();
    }

    public interface IMonoBehaviourWithState<TState> : IMonoBehaviourWithState
        where TState : MonoBehaviourState
    {
        new TState GetState();

        void SetState(TState state);
    }
}