using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DefaultNamespace;
using TeamZ.Code.Game.Characters;
using TeamZ.Helpers;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace TeamZ.Characters.MovementHandlers
{
    public abstract class MovementHandler : DisposableContainer
    {
        private Subject<MovementHandler> next = new Subject<MovementHandler>();
        public IObservable<MovementHandler> OnNext => this.next;
        public void Next(MovementHandler handler)
        {
            this.Dispose();
            this.next.OnNext(handler);
            this.next.OnCompleted();
            this.next = new Subject<MovementHandler>();
        }

        public abstract void Init(CharacterControllerScript character);
    }
}
