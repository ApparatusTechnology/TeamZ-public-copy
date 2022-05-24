using TeamZ.GameSaving.States.Charaters;
using UnityEngine;

namespace TeamZ.Code.Game.Characters.Hedgehog
{
    public class Hedgehog : Character<HedgehogState>
    {
        // throwing boxes feature available only for Hedgehog
        [SerializeField]
        private int throwImpulse = 30;

        public int ThrowImpulse
        {
            get => throwImpulse;
            private set => throwImpulse = value;
        }

        public override HedgehogState GetState()
        {
            return new HedgehogState
            {
                Armor = this.Armor,
                PunchDamage = this.PunchDamage,
                KickDamage = this.KickDamage,
                Health = this.Health,
                Name = this.CharacterName,
                RunSpeed = this.RunSpeed,
                CreepSpeed = this.CreepSpeed,
                StrikeSpeed = this.StrikeSpeed,
                JumpSpeed = this.JumpSpeed,
                JumpForce = this.JumpForce
            };
        }

        public override void SetState(HedgehogState state)
        {
            base.SetState(state);
        }
    }
}
