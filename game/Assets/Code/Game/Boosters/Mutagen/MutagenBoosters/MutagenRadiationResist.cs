using TeamZ.Code.Game.Characters;

namespace TeamZ.Code.Game.Boosters.Mutagen.MutagenBoosters
{
    // increases protection against radiation
    public class MutagenRadiationResist : MutagenBooster
    {
        public override void Apply(ICharacter character, int duration)
        {
            base.Apply(character, duration);
            character.ApplyMutagen(this);
        }
    }
}
