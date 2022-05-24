using TeamZ.Code.Game.Characters;

namespace TeamZ.Code.Game.Boosters.Mutagen.MutagenBoosters
{
    // boosts health and fight power, but decrease run speed. disables strike cooldown
    public class MutagenPowerBooster : MutagenBooster
    {
        private const float healthPowerdMultiplier = 2.0f;
        private const float fightPowerMultiplier = 1.5f;
        private const float runSpeedMultiplier = 0.8f;
        private const bool disableCooldown = true;

        public override void Apply(ICharacter _character, int duration)
        {
            base.Apply(_character, duration);

            base.character.ApplyMutagen(this);

            base.character.Health *= (int)healthPowerdMultiplier;
            base.character.PunchDamage *= (int)fightPowerMultiplier;
            base.character.KickDamage *= (int)fightPowerMultiplier;
            base.character.RunSpeed *= runSpeedMultiplier;
            base.character.CreepSpeed *= runSpeedMultiplier;

            base.OnMutagenFinished += MutagenPowerBooster_OnMutagenFinished;
        }

        private void MutagenPowerBooster_OnMutagenFinished()
        {
            base.character.PunchDamage /= (int)fightPowerMultiplier;
            base.character.KickDamage /= (int)fightPowerMultiplier;
            base.character.RunSpeed /= runSpeedMultiplier;
            base.character.CreepSpeed /= runSpeedMultiplier;
        }
    }
}
