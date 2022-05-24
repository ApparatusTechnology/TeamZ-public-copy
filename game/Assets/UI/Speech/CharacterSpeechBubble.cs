using TeamZ.Code.Game.Characters;
using TeamZ.Code.Game.Players;
using TeamZ.Code.Helpers;
using TeamZ.GameSaving.MonoBehaviours;
using TeamZ.GameSaving.States;
using UniRx;
using UniRx.Async;
using UnityEngine;

namespace TeamZ.Assets.UI.Speech
{
    public class SpeechBubbleState : MonoBehaviourState
    {
        public string[] Messages { get; set; }
        public float Duration { get; set; }
        public CharacterSpeechBubble.CharacterType Character { get; set; }
    }

    public class CharacterSpeechBubble : MonoBehaviourWithState<SpeechBubbleState>
    {
        public enum CharacterType
        {
            Any,
            Lizard,
            Hedgehog
        }

        public string[] Messages;
        public float Duration;
        public GameObject SpeechBoxPrefab;
        public CharacterType Character;
        public bool NeedLockForCutscene = false;
        public bool NeedDestroyAfterRead = true;

        private UnityDependency<CharacterSpeechService> characterSpeechService;
        private UnityDependency<Terminal.Terminal> Terminal;
        private bool activated;

        private async void OnTriggerEnter2D(Collider2D collision)
        {
            if (activated)
            {
                return;
            }
            
            if (collision.GetComponentInParent<Player>() is Player player)
            {
                var name = collision.GetComponentInParent<ICharacter>()?.CharacterName;

                if (this.Character.ToString() == name || this.Character == CharacterType.Any)
                {
                    var controller = collision.GetComponentInParent<CharacterControllerScript>();

                    if (this.Messages.Length > 1 || this.NeedLockForCutscene)
                    {
                        controller.Animator.Idle();
                        controller.UserInputEnabled.Value = false;

                        this.Terminal.Value.PrintAndHideAsync("Press JUMP to skip dialog.", 150, 1000).Forget();

                        controller.UserInputProvider.Jump
                            .Where(o => o)
                            .FirstOrDefault()
                            .Subscribe(skip =>
                            {
                                controller.UserInputEnabled.Value = true;
                                this.characterSpeechService.Value.Destroy();
                                this.DestroyGameObject();
                            })
                            .AddTo(this);
                    }

                    this.activated = true;
                    await this.characterSpeechService.Value.Speech(player, this.SpeechBoxPrefab, this.Messages,
                        this.Duration);
                    controller.UserInputEnabled.Value = true;
                }

                if (this.NeedDestroyAfterRead)
                {
                    this.DestroyGameObject();
                }
            }
        }

        public override SpeechBubbleState GetState()
            => new SpeechBubbleState
            {
                Messages = this.Messages,
                Character = this.Character,
                Duration = this.Duration,
            };

        public override void SetState(SpeechBubbleState state)
        {
            this.Messages = state.Messages;
            this.Character = state.Character;
            this.Duration = state.Duration;
        }
    }
}