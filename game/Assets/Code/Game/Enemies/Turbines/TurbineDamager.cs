using System;
using TeamZ.Code.Game;
using TeamZ.Code.Game.Characters;
using TeamZ.Code.Helpers;
using UniRx;
using UnityEngine;

namespace TeamZ.Assets.Code.Game.Enemies.Turbines
{
    class TurbineDamager : MonoBehaviour, IDamager
    {
        public int Damage
        {
            get
            {
                return this.damage;
            }
            set
            {
                this.damage = value;
            }
        }

        public float DamageSpeed
        {
            get
            {
                return this.damageSpeed;
            }
            set
            {
                this.damageSpeed = value;
            }
        }

        [HideInInspector]
        public bool EnableDamaging { get; set; }

        [SerializeField]
        private int damage;

        [SerializeField]
        private float damageSpeed;

        private IDisposable damaging;
        private ReactiveCollection<ICharacter> characters;

        private void Awake()
        {
            this.EnableDamaging = true;
            this.characters = new ReactiveCollection<ICharacter>();

            this.characters.ObserveCountChanged().Subscribe(count =>
            {
                if (count == 1)
                {
                    this.MakeDamage(this.Damage);

                    return;
                }

                if (count == 0)
                {
                    this.damaging?.Dispose();
                }
            });
        }

        public void MakeDamage(int damage)
        {
            if (this.EnableDamaging)
            {
                // for the first time damage
                foreach (var character in this.characters)
                {
                    character.TakeDamage(this.Damage, 0);
                }
                ShakeMainCamera.Emit(this.DamageSpeed, 0.2f);

                this.damaging?.Dispose();

                this.damaging = Observable.Interval(TimeSpan.FromSeconds(this.DamageSpeed)).
                    Subscribe(o =>
                    {
                        foreach (var character in this.characters)
                        {
                            character.TakeDamage(this.Damage, 0);
                        }
                        ShakeMainCamera.Emit(this.DamageSpeed, 0.1f);
                    }).
                    AddTo(this);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponentInParent<ICharacter>() is ICharacter character)
            {
                this.characters.Add(character);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponentInParent<ICharacter>() is ICharacter character)
            {
                this.characters.Remove(character);
            }
        }
    }
}
