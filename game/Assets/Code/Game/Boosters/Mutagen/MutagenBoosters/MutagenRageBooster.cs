using TeamZ.Code.Game.Characters;

namespace TeamZ.Code.Game.Boosters.Mutagen.MutagenBoosters
{
    // boosts run and fight speed, and also increase health and fight power. disables strike cooldown
    // after usage health decreases by 50% of current health level
    public class MutagenRageBooster : MutagenBooster
    {
        private const float runSpeedMultiplier = 2.0f;
        private const float strikeSpeedMultiplier = 1.3f;
        private const float healthPowerdMultiplier = 2.0f;
        private const float fightPowerMultiplier = 1.5f;
        private const bool disableCooldown = true;

        public override void Apply(ICharacter _character, int duration)
        {
            base.Apply(_character, duration);

            base.character.ApplyMutagen(this);

            base.character.Health *= (int)healthPowerdMultiplier;
            base.character.Armor *= (int)healthPowerdMultiplier;
            base.character.PunchDamage *= (int)fightPowerMultiplier;
            base.character.KickDamage *= (int)fightPowerMultiplier;
            base.character.RunSpeed *= runSpeedMultiplier;

            base.OnMutagenFinished += MutagenRageBooster_OnMutagenFinished;
        }

        private void MutagenRageBooster_OnMutagenFinished()
        {
            base.character.PunchDamage /= (int)fightPowerMultiplier;
            base.character.KickDamage /= (int)fightPowerMultiplier;
            base.character.RunSpeed /= runSpeedMultiplier;
            base.character.Health /= 2;
        }
    }
}
