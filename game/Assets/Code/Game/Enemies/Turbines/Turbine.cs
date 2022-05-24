using System;
using System.Collections;
using Spine.Unity;
using TeamZ.Assets.Code.Game.Levels;
using TeamZ.Assets.UI.Terminal;
using TeamZ.Code.Game.Activation.Core;
using TeamZ.Code.Helpers;
using TeamZ.GameSaving.MonoBehaviours;
using TeamZ.GameSaving.States;
using UniRx;
using UniRx.Async;
using UnityEngine;

namespace TeamZ.Assets.Code.Game.Enemies.Turbines
{
    public class Turbine : MonoBehaviourWithState<ActivatorState>, IActivable
    {
        public bool IsActive = true;

        public float AnimationSpeed
        {
            get { return this.animationSpeed; }
            set { this.animationSpeed = value; }
        }

        public float SlowdownSpeed = 2;

        [HideInInspector]
        public bool Rotating
        {
            get 
            { 
                return this.rotating; 
            }
            set
            {
                this.rotating = value;
                this.rotatingCoroutine?.Dispose();
                this.rotatingCoroutine = Observable.FromMicroCoroutine(() => this.AnimationCoroutine()).Subscribe();
            }
        }

        private UnityDependency<Terminal> Terminal;

        [SerializeField]
        private float animationSpeed;

        private SkeletonAnimation turbineAnimation;

        private bool rotating = true;
        private IDisposable rotatingCoroutine;

        private IEnumerator AnimationCoroutine()
        {
            while (true)
            {
                if (this.rotating)
                {
                    this.turbineAnimation.state.TimeScale += this.SlowdownSpeed * Time.deltaTime;
                    if (this.turbineAnimation.state.TimeScale > this.AnimationSpeed)
                    {
                        this.turbineAnimation.state.TimeScale = this.AnimationSpeed;
                        yield break;
                    }
                }
                else
                {
                    this.turbineAnimation.state.TimeScale -= this.AnimationSpeed * this.SlowdownSpeed * Time.deltaTime;
                    if (this.turbineAnimation.state.TimeScale < 0)
                    {
                        this.turbineAnimation.state.TimeScale = 0;
                        yield break;
                    }
                }

                yield return null;
            }
        }

        public void Activate()
        {
            if (!this.IsActive)
            {
                this.Terminal.Value.PrintAndHideAsync("The switch is broken!", 500, 2000, true).Forget();
                return;
            }

            var damager = GetComponentInChildren<TurbineDamager>();
            var blocker = GetComponentInChildren<WayBlocker>();
            var smokes = GetComponentsInChildren<ParticleSystem>();

            this.Rotating = !this.Rotating;
            damager.EnableDamaging = !damager.EnableDamaging;

            if (this.Rotating)
            {
                blocker.Enable();

                foreach (var smoke in smokes)
                {
                    smoke.Play();
                }
            }
            else
            {
                blocker.Disable();

                foreach (var smoke in smokes)
                {
                    smoke.Stop();
                }
            }
        }

        private void Awake()
        {
            this.turbineAnimation = GetComponentInChildren<SkeletonAnimation>();
        }

        private void Start()
        {
            this.turbineAnimation.state.SetAnimation(0, "turbine_idle", true).TimeScale = this.AnimationSpeed;
            var blocker = GetComponentInChildren<WayBlocker>();
            var smokes = GetComponentsInChildren<ParticleSystem>();
            var damager = GetComponentInChildren<TurbineDamager>();
            damager.EnableDamaging = this.Rotating;
            
            if (this.Rotating)
            {
                blocker.Enable();

                foreach (var smoke in smokes)
                {
                    smoke.Play();
                }
            }
            else
            {
                blocker.Disable();

                foreach (var smoke in smokes)
                {
                    smoke.Stop();
                }
            }
        }

        public override ActivatorState GetState()
        {
            return new ActivatorState
            {
                Name = this.name,
                IsActive = this.IsActive,
                IsActivated = this.Rotating
            };
        }

        public override void SetState(ActivatorState state)
        {
            this.name = state.Name;
            this.IsActive = state.IsActive;
            this.Rotating = state.IsActivated;
        }
    }
}