using TeamZ.Code.Game.Characters;

namespace TeamZ.Code.Game.Boosters.Mutagen.MutagenBoosters
{
    // activates night vision feature
    public class MutagenNightVisionBooster : MutagenBooster
    {
        public override void Apply(ICharacter character, int duration)
        {
            base.Apply(character, duration);
            character.ApplyMutagen(this);
        }
    }
}
