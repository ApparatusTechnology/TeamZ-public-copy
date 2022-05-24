using TeamZ.Code.Game.Characters;

namespace TeamZ.Code.Game.Boosters.Mutagen.MutagenBoosters
{
    // activates disguises
    public class MutagenDisguiseBooster : MutagenBooster
    {
        private const bool activateDisguise = true;
        private readonly int[] disguiseLevels = { 10, 30, 50, 70, 90 };

        public override void Apply(ICharacter character, int duration)
        {
            base.Apply(character, duration);
            character.ApplyMutagen(this);
        }
    }
}
