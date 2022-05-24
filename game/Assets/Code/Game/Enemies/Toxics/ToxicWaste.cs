using System;
using TeamZ.Code.Game.Characters;
using UniRx;
using UnityEngine;

namespace TeamZ.Code.Game.Enemies.Toxics
{
    public class ToxicWaste : MonoBehaviour, IDamager
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

        public double Speed
        {
            get
            {
                return this.speed;
            }
            set
            {
                this.speed = value;
            }
        }

        [SerializeField]
        private int damage;

        [SerializeField]
        private double speed;

        private IDisposable damaging;
        private ReactiveCollection<ICharacter> characters;

        private void Start()
        {
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

        public void MakeDamage(int damage)
        {
            // for the first time damage
            foreach (var character in this.characters)
            {
                character.TakeDamage(this.Damage, 0);
            }

            this.damaging?.Dispose();
            this.damaging = Observable.Interval(TimeSpan.FromSeconds(this.Speed)).
                Subscribe(o =>
                {
                    foreach (var character in this.characters)
                    {
                        character.TakeDamage(this.Damage, 0);
                    }
                }).
                AddTo(this);
        }
    }
}
