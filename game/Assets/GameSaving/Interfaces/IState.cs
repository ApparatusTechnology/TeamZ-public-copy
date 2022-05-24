using TeamZ.Code.Game.Players;
using TeamZ.Helpers;

namespace TeamZ.GameSaving.Interfaces
{
    public abstract class State
    {
    }

    public interface IStateProvider
    {
        State GetState();

        void SetState(State state);
    }

    public interface IStateProvider<TState> : IStateProvider
        where TState : State
    {
        new TState GetState();

        void SetState(TState state);
    }

    public abstract class StateProvider<TState> : DisposableContainer, IStateProvider<TState>
        where TState : State
    {
        public abstract TState GetState();

        public abstract void SetState(TState state);

        void IStateProvider.SetState(State state)
        {
            this.SetState((TState)state);
        }

        State IStateProvider.GetState()
        {
            return this.GetState();
        }
    }
}
