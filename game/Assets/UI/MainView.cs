using System;
using System.Linq;
using TeamZ.Characters;
using TeamZ.Code;
using TeamZ.Code.DependencyInjection;
using TeamZ.Code.Game.Levels;
using TeamZ.Code.Game.UserInput;
using TeamZ.Code.Helpers;
using TeamZ.Effects;
using TeamZ.GameSaving;
using UniRx;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

namespace TeamZ.UI
{
    public class MainView : View
    {
        public readonly Dependency<GameController> GameController;
        public readonly Dependency<LevelManager> LevelManager;
        public readonly UnityDependency<ViewRouter> ViewRouter;

        private UnityDependency<BackgroundImage> backgroundImage;

        public GameObject SaveButton;
        public GameObject ExitButton;
        public GameObject CloseButton;

        public GameObject Hedhehog;
        public GameObject Lizard;

        public void Start()
        {
            this.backgroundImage.Value.Show();
        }

        private async void OnEnable()
        {
            await Observable.NextFrame();

            if (this.SaveButton)
            {
                this.SaveButton.SetActive(this.LevelManager.Value.CurrentLevel != null);
            }

            if (this.CloseButton)
            {
                this.CloseButton.SetActive(this.LevelManager.Value.CurrentLevel != null);
            }

            Selectable.allSelectablesArray.First().Select();
        }

        public void Next()
        {
            this.Lizard.SetActive(!this.Lizard.activeSelf);
            this.Hedhehog.SetActive(!this.Hedhehog.activeSelf);
        }

        public void Play()
        {
            this.GameController.Value
                .StartNewGameAsync(this.Hedhehog.activeSelf ? CharactersList.Hedgehog : CharactersList.Lizard).Forget();
        }

        public void Load()
        {
            this.ViewRouter.Value.ShowView(this.ViewRouter.Value.LoadView);
        }

        public void Save()
        {
            var date = this.GameController.Value.FormDateTimeString();
            this.GameController.Value.SaveAsync($"User save {date}").Forget();
            this.SaveButton.SetActive(false);
            Selectable.allSelectablesArray.First().Select();
        }

        public void Settings()
        {
            this.ViewRouter.Value.ShowView(this.ViewRouter.Value.SettingsView);
        }

        public void About()
        {
        }

        public void Close()
        {
            this.backgroundImage.Value.Hide();
            this.ViewRouter.Value.ShowGameHUDView();
            MessageBroker.Default.Publish(new ResumeGame(this.LevelManager.Value.CurrentLevel.Name));
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}