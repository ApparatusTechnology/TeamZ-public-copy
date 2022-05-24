using System;
using TeamZ.AI.Core;
using TeamZ.Assets.Code.Game.Animators;
using TeamZ.Assets.UI.Terminal;
using TeamZ.Code.Game;
using TeamZ.Code.Game.Characters;
using TeamZ.Code.Game.Enemies;
using TeamZ.Code.Helpers;
using TeamZ.Helpers;
using UniRx;
using UniRx.Async;
using UnityEngine;

namespace TeamZ.AI.States
{
    public class DroneAttack : AIMindState
    {
        private Transform target;

        public DroneAttack(Transform transform)
        {
            this.target = transform;
        }

        public UnityDependency<Terminal> Terminal { get; set; }

        public async override void Activate(AIAgent agent)
        {
            var enemyData = agent.GetComponent<Enemy>();
            await this.Terminal.Value.PrintAndHideAsync("Enemy detected... Eliminate!", 500, 2000, true);
            // @TODO: Add warning sound

            await agent.gameObject.Move(this.target.position, 0.5f);
            // @TODO: Add fly sound

            var damageable = this.target.GetComponent<IDamageable>();
            this.TryToAttack(agent, damageable);

            Observable.Interval(TimeSpan.FromSeconds(2))
                .Subscribe(_ => this.TryToAttack(agent, damageable))
                .DisposeWith(this);
        }

        private void TryToAttack(AIAgent agent, IDamageable damageable)
        {
            if (!agent)
            {
                return;
            }

            if (Vector3.Distance(agent.transform.position, this.target.position) < 1)
            {
                damageable.TakeDamage(10, 0);
            }
            else
            {
                this.Next(new DroneAttack(this.target));
            }
        }
    }
}