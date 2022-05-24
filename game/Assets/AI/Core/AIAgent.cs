using System;
using System.Linq;
using UniRx;
using UnityEngine;

namespace TeamZ.AI.Core
{
    public class AIAgent : MonoBehaviour
    {
        private AIMindState currentState;

        public Mover Mover { get; private set; }
        public Sensor[] Sensors { get; private set; }

        private void Start()
        {
            this.Mover = this.GetComponentInChildren<Mover>() ??
                         this.GetComponent<Mover>();

            this.Sensors = this.GetComponentsInChildren<Sensor>()
                .Concat(this.GetComponents<Sensor>())
                .ToArray();
        }

        public void SetMindState(AIMindState state)
        {
            this.currentState?.Dispose();

            this.currentState = state;
            this.currentState.NextState
                .Subscribe(o => this.SetMindState(o)).AddTo(this);
            
            this.currentState.Activate(this);
        }

        private void OnDestroy()
        {
            this.currentState?.Dispose();
        }
    }
}