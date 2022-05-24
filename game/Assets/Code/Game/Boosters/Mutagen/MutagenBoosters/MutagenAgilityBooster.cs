using TeamZ.Code.Game.Characters;

namespace TeamZ.Code.Game.Boosters.Mutagen.MutagenBoosters
{
    // boosts run and fight speed and also jump speed and height. disables strike cooldown
    public class MutagenAgilityBooster : MutagenBooster
    {
        private const float runSpeedMultiplier = 1.5f;
        private const float strikeSpeedMultiplier = 1.3f;
        private const float jumpSpeedMultiplier = 1.5f;
        private const float jumpHeightMultiplier = 1.5f;
        private const bool disableCooldown = true;

        public override void Apply(ICharacter _character, int duration)
        {
            base.Apply(_character, duration);

            base.character.ApplyMutagen(this);

            base.character.RunSpeed *= runSpeedMultiplier;
            base.character.CreepSpeed *= runSpeedMultiplier;
            base.character.StrikeSpeed *= strikeSpeedMultiplier;
            base.character.JumpSpeed *= jumpSpeedMultiplier;
            base.character.JumpForce *= jumpHeightMultiplier;

            base.OnMutagenFinished += MutagenAgilityBooster_OnMutagenFinished;
        }

        private void MutagenAgilityBooster_OnMutagenFinished()
        {
            base.character.RunSpeed /= runSpeedMultiplier;
            base.character.CreepSpeed /= runSpeedMultiplier;
            base.character.StrikeSpeed /= strikeSpeedMultiplier;
            base.character.JumpSpeed /= jumpSpeedMultiplier;
            base.character.JumpForce /= jumpHeightMultiplier;
        }
    }
}
