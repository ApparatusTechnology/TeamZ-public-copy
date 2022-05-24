using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TeamZ.Assets.Code.Game.Enemies.Core;
using TeamZ.Code.Game;
using UniRx;
using UniRx.Async;
using UnityEngine;

namespace TeamZ.Assets.Code.Game.Enemies.Turel
{
    [RequireComponent(typeof(ParticleSystem))]
    [RequireComponent(typeof(AudioSource))]
    public class BlasterGun : Gun
    {
        private Subject<IDamageable> hits = new Subject<IDamageable>();
        private ParticleSystem gunParticleSystem;
        private AudioSource gunAudioSource;

        public override IObservable<IDamageable> Hits 
            => this.hits;

        private void Start()
        {
            this.hits.AddTo(this);
            this.gunParticleSystem = this.GetComponent<ParticleSystem>();
            this.gunAudioSource = this.GetComponent<AudioSource>();
        }

        public override void Shoot(Vector3 target)
        {
            this.gunParticleSystem.Emit(1);
            this.gunAudioSource.Play();
        }

        private void OnParticleCollision(GameObject other)
        {
            if (this.LayerMask.Contains(other.layer) && other.GetComponent<IDamageable>() is IDamageable damageable)
            {
                this.hits.OnNext(damageable);
            }
        }
    }
}