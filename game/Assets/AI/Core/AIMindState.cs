
using System;
using TeamZ.Helpers;
using UniRx;

namespace TeamZ.AI.Core
{
    public abstract class AIMindState : DisposableContainer
    {
        private Subject<AIMindState> nextState;

        public IObservable<AIMindState> NextState
            => this.nextState = new Subject<AIMindState>();

        public AIMindState()
        {
            this.nextState.DisposeWith(this);
        }
      
        protected void Next(AIMindState newState)
        {
            this.Dispose();

            this.nextState.OnNext(newState);
            this.nextState.OnCompleted();
        }

        public abstract void Activate(AIAgent agent);
    }
}