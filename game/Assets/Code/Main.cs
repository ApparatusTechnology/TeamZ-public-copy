using System;
using System.Linq;
using TeamZ.Assets.Code.Helpers;
using TeamZ.Code.DependencyInjection;
using TeamZ.Code.Game.Characters;
using TeamZ.Code.Game.Levels;
using TeamZ.Code.Game.Navigation;
using TeamZ.Code.Game.Players;
using TeamZ.Code.Game.UserInput;
using TeamZ.Code.Helpers;
using TeamZ.Code.Helpers.Extentions;
using TeamZ.Code.Mediator;
using TeamZ.Code.Mediator.Handlers;
using TeamZ.Effects;
using TeamZ.GameSaving;
using TeamZ.GameSaving.MonoBehaviours;
using TeamZ.Helpers;
using TeamZ.UI;
using UniRx;
using UniRx.Async;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TeamZ.Code
{
    public class Main : MonoBehaviour
    {
        private readonly UnityDependency<ViewRouter> viewRouter;
        private readonly UnityDependency<BackgroundImage> backgroundImage;
        private readonly UnityDependency<LevelBootstraper> levelBootstrapper;
        public readonly Dependency<UserInputMapper> UserInput;

        private readonly Dependency<GameController> gameController;
        private readonly Dependency<LevelManager> levelManager;

        private async void Start()
        {
            Application.targetFrameRate = 60;

            this.RegisterHandlers();
            this.RegisterDependencies(DependencyContainer.Instance);

            this.gameController.Value.Loaded.Subscribe(_ => this.Loaded());

            await UniTask.DelayFrame(1);
            MessageBroker.Default.Publish(new PauseGame());

            if (!this.levelBootstrapper)
            {
                this.viewRouter.Value.ShowMainView();
            }
            else
            {
                await this.levelBootstrapper.Value.Load();
            }

            this.UserInput.Value.UserInputProviders
                .Select(o => o.StartButton.Select(oo => (Provider: o, Value: o.StartButton.Value)))
                .SubscribeMany(pair =>
                {
                    if (!pair.Value)
                    {
                        return;    
                    }
                    
                    if (this.levelManager.Value.CurrentLevel == null)
                    {
                        return;
                    }
                    
                    if (!this.UserInput.Value.GetPairedProviders().Contains(pair.Provider))
                    {
                        return;
                    }
                    
                    if (this.viewRouter.Value.CurrentView is MainView)
                    {
                        this.backgroundImage.Value.Hide();
                        this.viewRouter.Value.ShowGameHUDView();
                        MessageBroker.Default.Publish(new ResumeGame(this.levelManager.Value.CurrentLevel.Name));
                        return;
                    }

                    MessageBroker.Default.Publish(new PauseGame());
                    this.backgroundImage.Value.Show();
                    this.viewRouter.Value.ShowMainView();
                })
                .AddTo(this);
        }

        private void RegisterDependencies(DependencyContainer container)
        {
            container.Add<GameController>();
            container.Add<GameStorage>();
            container.Add<LevelManager>();
            container.Add<EntitiesStorage>();
            container.Add<AudioSourcePull>();
            container.Add<UserInputMapper>();
            container.AddScoped<PlayerService>();
            container.AddScoped<NavigationService>();
        }

        private void RegisterHandlers()
        {
            Mediator.Mediator.Instance.Add(new DeathHandler());
            Mediator.Mediator.Instance.Add<PauseGame>(o =>
            {
                Time.timeScale = 0;
            });
            Mediator.Mediator.Instance.Add<ResumeGame>(o =>
            {
                Time.timeScale = 1;
            });
        }

        private void Loaded()
        {
            MessageBroker.Default.Publish(new ResumeGame(this.levelManager.Value.CurrentLevel.Name));
        }

        public async void Update()
        {
            if (Input.GetKeyUp(KeyCode.F5))
            {
                await this.gameController.Value.SaveAsync("test");
            }

            if (Input.GetKeyUp(KeyCode.F9))
            {
                await this.gameController.Value.LoadSavedGameAsync("test");
            }
        }

        private void OnDestroy()
        {
            Mediator.Mediator.Instance.Reset();
            DependencyContainer.Instance.Reset();
        }
    }

    public class PauseGame : ICommand
    {
        public PauseGame()
        {
        }
    }

    public class ResumeGame : ICommand
    {
        public string Level;

        public ResumeGame(string level = "")
        {
            this.Level = level;
        }
    }
}