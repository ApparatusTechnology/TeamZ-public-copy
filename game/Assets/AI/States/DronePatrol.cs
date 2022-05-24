using System;
using System.Linq;
using System.Threading;
using TeamZ.AI.Core;
using TeamZ.Code.Game.Characters;
using TeamZ.Code.Game.Players;
using TeamZ.Code.Helpers;
using TeamZ.GameSaving.Interfaces;
using TeamZ.Helpers;
using UniRx;
using UnityEngine;

namespace TeamZ.AI.States
{
    public class DronePatrol : AIMindState
    {
        private int currentDestination;

        public Vector3[] Path
        {
            get;
            set;
        }

        public DronePatrol(Vector3[] path)
        {
            this.Path = path;
        }

        public override async void Activate(AIAgent agent)
        {
            agent.Sensors.ForEach(o => o.DetectedObjects.ObserveAdd().Subscribe(gameObject =>
            {
                if (gameObject.Value.GetPlayer() is Player player)
                {
                    this.Next(new DroneAttack(player.transform));
                }

            }).DisposeWith(this));

            while (!this.Disposed)
            {
                await agent.Mover.MoveAsync(this.Path, () => this.Disposed);
                await agent.Mover.MoveAsync(this.Path.Reverse(), () => this.Disposed);
            }

            Debug.Log("patrol stop");
        }
    }
}