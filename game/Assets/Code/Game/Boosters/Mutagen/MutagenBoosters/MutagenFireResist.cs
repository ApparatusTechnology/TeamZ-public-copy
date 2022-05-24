using TeamZ.Code.Game.Characters;

namespace TeamZ.Code.Game.Boosters.Mutagen.MutagenBoosters
{
    // increases protection against fire
    public class MutagenFireResist : MutagenBooster
    {
        public override void Apply(ICharacter character, int duration)
        {
            base.Apply(character, duration);
            character.ApplyMutagen(this);
        }
    }
}
