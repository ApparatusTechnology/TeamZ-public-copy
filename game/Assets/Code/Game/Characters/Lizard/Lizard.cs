using TeamZ.GameSaving.States.Charaters;

namespace TeamZ.Code.Game.Characters.Lizard
{
    public class Lizard : Character<LizardState>
    {
        // TODO: add specific Lizard properties and behavior

        public override LizardState GetState()
        {
            return new LizardState
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

        public override void SetState(LizardState state)
        {
            base.SetState(state);
        }
    }
}
