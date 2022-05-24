using System;
using System.Collections.Generic;
using TeamZ.Code.Game.Highlighting;
using TeamZ.Code.Helpers;
using TeamZ.GameSaving.States;
using UniRx;
using UniRx.Async;
using UnityEngine;

namespace TeamZ.Code.Game.Levels
{
    public class Explodable : LevelObject
    {
        public float ExplosionPower = 2000;
        public int ExplosionDamage = 30;
        public float ExplosionRadius = 15;
        public float DropVelocityLimit = 1;

        public ParticleSystem Smoke;
        public ParticleSystem Fire;
        public ParticleSystem Explode1;
        public ParticleSystem Explode2;
        public ParticleSystem Explode3;

        private bool isFirstTimeDamaged = true;
        private Collision2D floorCollision = null;

        private bool isAlreadyExploded = false;

        private async void Explode()
        {
            // cancel all barrel effects
            this.Fire.Stop();
            this.Smoke.Play();
            this.Explode1.Play();
            this.Explode2.Play();
            this.Explode3.Play();

            MessageBroker.Default.Publish(new BurningEnded());

            foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
            {
                if (renderer.name.Contains("sticker") || renderer.name.Contains("ConstantHighlighter"))
                {
                    renderer.enabled = false;
                }
                else
                {
                    renderer.sortingLayerName = "CharacterOverlay";
                    renderer.sortingLayerID = 7;
                    renderer.sortingOrder = 0;
                }
            }

            Destroy(this.gameObject.GetComponent<Highlighter>());
            // cancel all barrel effects

            if (this.isAlreadyExploded)
            {
                return;
            }

            MessageBroker.Default.Publish(new ExplosionHappened());
            MessageBroker.Default.Publish(new ShakeMainCamera() { Amplitude = 1, Time = 0.5f });

            // decrease barrel colliders size
            var barrelColliders = this.GetComponentsInChildren<BoxCollider2D>();

            foreach (var collider in barrelColliders)
            {
                collider.size = new Vector2(collider.size.x, collider.size.y / 5);
                collider.transform.position = new Vector3(collider.transform.position.x, collider.transform.position.y - (collider.size.y + collider.size.y / 2), 0);
            }
            // decrease barrel colliders size

            int finalLayerMask = 8;
            int[] activeLayersToInteraction = { 9, 10, 13, 14 }; // level object, enemy, character1, character2

            foreach (int layer in activeLayersToInteraction)
            {
                int layerMask = 1 << layer;
                finalLayerMask |= layerMask;
            }

            Collider2D[] colliders =
                Physics2D.OverlapCircleAll(this.transform.position, this.ExplosionRadius, finalLayerMask);

            foreach (var hit in colliders)
            {
                if (hit.attachedRigidbody != null)
                {
                    if (hit.attachedRigidbody.name.Contains("greenbox"))
                    {
                        this.ExplosionPower *= 10;
                    }

                    hit.attachedRigidbody.AddExplosionForce(this.ExplosionPower, this.transform.position,
                        this.ExplosionRadius);

                    if (hit.attachedRigidbody.name.Contains("greenbox"))
                    {
                        this.ExplosionPower /= 10;
                    }
                }
            }

            await UniTask.Delay(200);

            Fight2D.Action(this.transform.position, this.ExplosionRadius, activeLayersToInteraction, true, this.ExplosionDamage, 0);

            this.isAlreadyExploded = true;
        }

        public override void TakeDamage(int damage, int impulse)
        {
            base.TakeDamage(damage, 0);

            if (this.isFirstTimeDamaged)
            {
                this.Fire.Play();

                MessageBroker.Default.Publish(new BurningHappened());
                Observable.EveryFixedUpdate().Subscribe(_ => OnUpdate());
                this.isFirstTimeDamaged = false;
            }
        }

        private void OnUpdate()
        {
            if (this.Strength.Value > 0)
            {
                base.TakeDamage(1, 0);
            }
        }

        protected override void StrengthTooLow()
        {
            this.Explode();
        }

        protected void OnCollisionEnter2D(Collision2D collision)
        {
            List<int> layersForCollisionExplosion = new List<int> { 8, 9, 10, 13, 14 };   // ground, level object, enemy, character1, character2
            var explodableObjectRB = this.GetComponent<Rigidbody2D>();

            if (layersForCollisionExplosion.Contains(collision.gameObject.layer) && explodableObjectRB &&
                (Math.Abs(explodableObjectRB.velocity.x) >= this.DropVelocityLimit || Math.Abs(explodableObjectRB.velocity.y) >= this.DropVelocityLimit))
            {
                if (this.Strength.Value > 0)
                {
                    // BOOM
                    this.isFirstTimeDamaged = false;
                    this.Strength.Value = 0;
                }
            }

            // ground
            if (collision.gameObject.layer == 8)
            {
                this.floorCollision = collision;
            }
        }

        protected void OnCollisionExit2D(Collision2D collision)
        {
            // ground
            if (collision.gameObject.layer == 8)
            {
                this.floorCollision = null;
            }
        }

        protected void FixedUpdate()
        {
            if (this.floorCollision != null && this.Strength.Value <= 0)
            {
                base.StrengthTooLow();
            }
        }

        public override LevelObjectState GetState()
        {
            var baseState = base.GetState();

            baseState.IsAlreadyExploded = this.isAlreadyExploded;

            return baseState;
        }

        public override void SetState(LevelObjectState state)
        {
            this.isAlreadyExploded = state.IsAlreadyExploded;

            base.SetState(state);
        }
    }

    public class ExplosionHappened { }

    public class BurningHappened {}

    public class BurningEnded {}
}