using System;
using System.Linq;
using TeamZ.Characters;
using TeamZ.Code.DependencyInjection;
using TeamZ.Code.Game.Characters;
using TeamZ.Code.Game.Notifications;
using TeamZ.Code.Game.UserInput;
using TeamZ.Code.Helpers;
using TeamZ.Code.Helpers.Extentions;
using TeamZ.GameSaving;
using UniRx;
using UnityEngine.UI;

namespace TeamZ.UI
{
    public class CharacterSelectionView : View
    {
        Dependency<GameController> GameController;
        Dependency<UserInputMapper> UserInputMapper;
        UnityDependency<NotificationService> NotificationService;

        public CharacterDescriptor firstUserSelection = null;
        public CharacterDescriptor secondUserSelection = null;

        private CharacterDescriptor[] selectedCharacters;

        private void OnEnable()
        {
            Observable.NextFrame().Subscribe(_ => Selectable.allSelectablesArray.First().Select());

            // this.UserInputMapper.Value.FirstPlayerInputProvider?.Value.Horizontal
            //     .Skip(0)
            //     .Where(o => o < -0.5f || o > 0.5f)
            //     .Select(o => o < 0 ? CharactersList.Lizard : CharactersList.Hedgehog)
            //     .Subscribe(o =>
            //     {
            //         this.firstUserSelection = o;
            //         this.NotificationService.Value.ShowShortMessage($"First player selectecs {o.Name}", false);
            //     })
            //     .AddTo(this);
            //
            // this.UserInputMapper.Value.SecondPlayerInputProvider?.Value.Horizontal
            //     .Skip(0)
            //     .Where(o => o < -0.5f || o > 0.5f)
            //     .Select(o => o < 0 ? CharactersList.Lizard : CharactersList.Hedgehog)
            //     .Subscribe(o =>
            //     {
            //         this.secondUserSelection = o;
            //         this.NotificationService.Value.ShowShortMessage($"Second player selects {o.Name}", false);
            //     })
            //     .AddTo(this);
            //
            // this.UserInputMapper.Value.FirstPlayerInputProvider?.Value.Start
            //     .Merge(this.UserInputMapper.Value.SecondPlayerInputProvider?.Value.Start)
            //     .HoldFor(TimeSpan.FromSeconds(3))
            //     .Subscribe(async _ =>
            //     {
            //         if (this.firstUserSelection is null && this.secondUserSelection is null)
            //         {
            //             this.NotificationService.Value.ShowShortMessage("Chose your character", false);
            //             return;
            //         }
            //
            //         if (this.firstUserSelection == this.secondUserSelection)
            //         {
            //             this.NotificationService.Value.ShowShortMessage("Same characters are not allowed", false);
            //             return;
            //         }
            //
            //         this.selectedCharacters = new[] { this.firstUserSelection, this.secondUserSelection }.Where(o => o != null).ToArray();
            //         await this.GameController.Value.StartNewGameAsync(this.selectedCharacters);
            //     })
            //     .AddTo(this);

        }

        public void SelectLizard(bool isActive)
        {
            if (isActive)
            {
                if (this.firstUserSelection == null)
                {
                    this.firstUserSelection = CharactersList.Lizard;
                    this.NotificationService.Value.ShowShortMessage($"First player selects {this.firstUserSelection.Name}", false);
                }
                else if (this.secondUserSelection == null)
                {
                    this.secondUserSelection = CharactersList.Lizard;
                    this.NotificationService.Value.ShowShortMessage($"Second player selects {this.secondUserSelection.Name}", false);
                }
            }
            else
            {
                if (this.firstUserSelection == CharactersList.Lizard)
                {
                    this.firstUserSelection = null;
                }

                if (this.secondUserSelection == CharactersList.Lizard)
                {
                    this.secondUserSelection = null;
                }
            }
        }

        public void SelectHedgehog(bool isActive)
        {
            if (isActive)
            {
                if (this.firstUserSelection == null)
                {
                    this.firstUserSelection = CharactersList.Hedgehog;
                    this.NotificationService.Value.ShowShortMessage($"First player selectecs {this.firstUserSelection.Name}", false);
                }
                else if (this.secondUserSelection == null)
                {
                    this.secondUserSelection = CharactersList.Hedgehog;
                    this.NotificationService.Value.ShowShortMessage($"Second player selectecs {this.secondUserSelection.Name}", false);
                }
            }
            else
            {
                if (this.firstUserSelection == CharactersList.Hedgehog)
                {
                    this.firstUserSelection = null;
                }

                if (this.secondUserSelection == CharactersList.Hedgehog)
                {
                    this.secondUserSelection = null;
                }
            }
        }

        public async void StartGame()
        {
            if (this.firstUserSelection is null && this.secondUserSelection is null)
            {
                this.NotificationService.Value.ShowShortMessage("Chose your character", false);
                return;
            }

            if (this.firstUserSelection == this.secondUserSelection)
            {
                this.NotificationService.Value.ShowShortMessage("Same characters are not allowed", false);
                return;
            }

            this.selectedCharacters = new[] { this.firstUserSelection, this.secondUserSelection }.Where(o => o != null).ToArray();

            await this.GameController.Value.StartNewGameAsync(this.selectedCharacters);
        }
    }
}
