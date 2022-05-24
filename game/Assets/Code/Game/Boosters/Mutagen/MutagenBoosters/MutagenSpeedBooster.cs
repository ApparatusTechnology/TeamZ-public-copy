using TeamZ.Assets.UI.Speech;
using TeamZ.Assets.UI.Terminal;
using TeamZ.Code.Game.Characters;
using TeamZ.Code.Game.Players;
using TeamZ.Code.Helpers;
using UniRx.Async;
using UnityEngine;

namespace TeamZ.Code.Game.Boosters.Mutagen.MutagenBoosters
{
    // boosts run speed and jump speed and height
    public class MutagenSpeedBooster : MutagenBooster
    {
        private const float runSpeedMultiplier = 2.0f;
        private const float jumpSpeedMultiplier = 1.5f;
        private const float jumpHeightMultiplier = 1.5f;

        private UnityDependency<Terminal> Terminal;
        private UnityDependency<CharacterSpeechService> CharacterSpeechService;

        public override void Apply(ICharacter _character, int duration)
        {
            var player = GameObject.FindObjectOfType<Player>();
            this.CharacterSpeechService.Value.Speech(player, new[] { "I'm fucking quick!" }, 1).Forget();

            base.Apply(_character, duration);

            base.character.ApplyMutagen(this);

            base.character.RunSpeed *= runSpeedMultiplier;
            base.character.CreepSpeed *= runSpeedMultiplier;
            base.character.JumpSpeed *= jumpSpeedMultiplier;
            base.character.JumpForce *= jumpHeightMultiplier;

            base.OnMutagenFinished += MutagenSpeedBooster_OnMutagenFinished;

            this.Terminal.Value.PrintAndHideAsync($"You have used SPEED mutagen. Now you're run faster and jump higher.", 500, 1500, true).Forget();
        }

        private void MutagenSpeedBooster_OnMutagenFinished()
        {
            base.character.RunSpeed /= runSpeedMultiplier;
            base.character.CreepSpeed /= runSpeedMultiplier;
            base.character.JumpSpeed /= jumpSpeedMultiplier;
            base.character.JumpForce /= jumpHeightMultiplier;
        }
    }
}
