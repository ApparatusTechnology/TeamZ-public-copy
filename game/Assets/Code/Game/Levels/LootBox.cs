using TeamZ.Assets.UI.Speech;
using TeamZ.Assets.UI.Terminal;
using TeamZ.Code.Game.Activation.Core;
using TeamZ.Code.Game.Players;
using TeamZ.Code.Helpers;
using TeamZ.GameSaving.MonoBehaviours;
using TeamZ.GameSaving.States;
using UniRx;
using UniRx.Async;
using UnityEngine;

namespace TeamZ.Code.Game.Levels
{
    public class LootBox : MonoBehaviourWithState<LootBoxState>, IActivable
    {
        public BoolReactiveProperty IsOpen = new BoolReactiveProperty(false);

        public Sprite[] VisualStates;

        public bool IsAvailable
        {
            get { return this.isAvailable; }
            set { this.isAvailable = value; }
        }

        public bool IsNotEmpty
        {
            get { return this.isNotEmpty; }
            set { this.isNotEmpty = value; }
        }

        [SerializeField]
        private bool isAvailable = false;

        [SerializeField]
        private bool isNotEmpty = false;

        protected SpriteRenderer Renderer2D;

        private UnityDependency<Terminal> Terminal;
        private UnityDependency<CharacterSpeechService> CharacterSpeechService;

        // Start is called before the first frame update
        protected void Start()
        {
            this.Renderer2D = this.GetComponent<SpriteRenderer>();

            if (this.Renderer2D != null && this.VisualStates.Length > 0)
            {
                this.Renderer2D.sprite = this.VisualStates[0];
            }

            this.IsOpen.Subscribe(value => { this.SwitchVisualState(); });
        }

        private async void TryOpenBox()
        {
            if (this.IsAvailable)
            {
                this.IsOpen.Value = !this.IsOpen.Value;

                if (this.IsOpen.Value)
                {
                    // TEMPORARY CODE
                    if (!this.IsNotEmpty)
                    {
                        await this.Terminal.Value.PrintAndHideAsync($"Box is empty.", 500, 1000, true);
                        var player = GameObject.FindObjectOfType<Player>();
                        await this.CharacterSpeechService.Value.Speech(player, new[] {"Shit!"}, 1);
                    }
                }
            }
            else
            {
                await this.Terminal.Value.PrintAndHideAsync($"You can't open this box.", 500, 1000, true);
            }
        }

        private void SwitchVisualState()
        {
            if (this.IsOpen.Value)
            {
                this.Renderer2D.sprite = this.VisualStates[0];
            }
            else
            {
                this.Renderer2D.sprite = this.VisualStates[1];
            }
        }

        public override LootBoxState GetState()
        {
            return new LootBoxState
            {
                IsOpen = this.IsOpen.Value,
                IsAvailable = this.IsAvailable,
                IsNotEmpty = this.IsNotEmpty
            };
        }

        public override void SetState(LootBoxState state)
        {
            this.IsOpen.Value = state.IsOpen;
            this.IsAvailable = state.IsAvailable;
            this.IsNotEmpty = state.IsNotEmpty;
        }

        public void Activate()
        {
            this.TryOpenBox();
        }
    }
}