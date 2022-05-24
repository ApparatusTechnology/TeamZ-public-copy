﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamZ.Characters;
using TeamZ.Code;
using TeamZ.Code.DependencyInjection;
using TeamZ.Code.Game.Characters;
using TeamZ.Code.Game.Levels;
using TeamZ.Code.Game.Messages.GameSaving;
using TeamZ.Code.Game.Navigation;
using TeamZ.Code.Game.Notifications;
using TeamZ.Code.Game.Players;
using TeamZ.Code.Game.UserInput;
using TeamZ.Code.Helpers;
using TeamZ.Effects;
using TeamZ.GameSaving.Interfaces;
using TeamZ.GameSaving.MonoBehaviours;
using TeamZ.GameSaving.States;
using TeamZ.GameSaving.States.Charaters;
using TeamZ.UI;
using UniRx;
using UniRx.Async;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TeamZ.GameSaving
{
    public class GameController : IGameController
    {
        private UnityDependency<BlackScreen> BlackScreen;
        private UnityDependency<ViewRouter> ViewRouter;
        private UnityDependency<NotificationService> Notifications;
        private UnityDependency<BackgroundImage> BackgroundImage;
        private UnityDependency<LoadingText> LoadingText;
        private Dependency<UserInputMapper> UserInputMapper;
        private Dependency<PlayerService> PlayerService;
        private Dependency<EntitiesStorage> EntitiesStorage;
        private Dependency<LevelManager> LevelManager;
        private Dependency<GameStorage> Storage;

        private bool loading;
        private GameState lastKnowGameState;

        public HashSet<Guid> VisitedLevels { get; private set; }

        public GameController()
        {
            this.Loaded = new Subject<Unit>();
            this.VisitedLevels = new HashSet<Guid>();

            MessageBroker.Default.Receive<GameSaved>()
                .Subscribe(_ => this.Notifications.Value.ShowShortMessage("Game saved", true));

            MessageBroker.Default.Receive<LoadGameRequest>()
                .Subscribe(async o =>
                {
                    MessageBroker.Default.Publish(new ResumeGame(string.Empty));
                    this.ViewRouter.Value.ShowGameHUDView();
                    this.BackgroundImage.Value.Hide();
                    await this.BlackScreen.Value.ShowAsync();
                    await this.LoadSavedGameAsync(o.SlotName);
                    MessageBroker.Default.Publish(new GameLoaded());
                    await this.BlackScreen.Value.HideAsync();
                });

            this.Loaded
                .Subscribe(o => Dependency<NavigationService>.Resolve().Activate());
        }


        public Subject<Unit> Loaded
        {
            get;
        }


        public void BootstrapEntities(bool loaded = false)
        {
            this.EntitiesStorage.Value.Entities.Clear();
            this.EntitiesStorage.Value.Root = GameObject.Find("Root");
            foreach (var entity in GameObject.FindObjectsOfType<Entity>())
            {
                entity.LevelId = this.LevelManager.Value.CurrentLevel.Id;
                this.EntitiesStorage.Value.Entities.Add(entity.Id, entity);
            };

            //await this.SaveAsync("temp");
            //await this.LoadAsync("temp");
        }

        public void BootstrapFromEditor()
        {
            var levelBootstraper = GameObject.FindObjectOfType<LevelBootstraper>();
            this.LevelManager.Value.CurrentLevel = Level.All[levelBootstraper.LevelName];
        }

        public async Task LoadSavedGameAsync(string slotName)
        {
            if (this.loading)
            {
                return;
            }

            this.loading = true;

            this.BackgroundImage.Value.Hide();
            await this.BlackScreen.Value.ShowAsync();
            var gameState = await this.Storage.Value.LoadAsync(slotName);

            var level = Level.AllById[gameState.LevelId];
            var levelName = Texts.GetLevelText(level.Name);

            this.LoadingText.Value.DisplayNewText(levelName);
            await this.LoadGameStateAsync(gameState);

            await Task.Delay(1000);

            this.LoadingText.Value.HideText();
            await this.BlackScreen.Value.HideAsync();

            this.loading = false;
        }

        public async Task LoadAsync(Level level)
        {
            this.VisitedLevels.Clear();

            await this.LevelManager.Value.LoadAsync(level);

            var gameState = this.GetState();
            await this.Bootstrap(gameState);

            gameState.VisitedLevels.Add(level.Id);
        }

        public async Task LoadGameStateAsync(GameState gameState)
        {
            DependencyContainer.Instance.NewScope();

            var level = Level.AllById[gameState.LevelId];
            await this.LevelManager.Value.LoadAsync(level);
            await this.Bootstrap(gameState);

            this.Loaded.OnNext(Unit.Default);
        }

        public async Task SaveAsync(string slotName)
        {
            await this.SaveAsync(this.GetState(), slotName);
            MessageBroker.Default.Publish(new GameSaved());
        }

        public async Task SaveAsync(GameState gameState, string slotName)
        {
            await this.Storage.Value.SaveAsync(gameState, slotName);
        }

        public async void SwitchLevelAsync(Level level, string locationName)
        {
            await this.BlackScreen.Value.ShowAsync();
            this.LoadingText.Value.DisplayNewText(Texts.GetLevelText(level.Name));

            var time = DateTime.Now.ToTeamZDateTime();
            var beforeAutoSave = $"Switching to {level.Name} {time}";

            var gameState = this.GetState();
            await this.SaveAsync(gameState, beforeAutoSave);

            gameState.LevelId = level.Id;
            var mainCharacters = gameState.GameObjectsStates.
                Where(o => o.MonoBehaviousStates.OfType<CharacterState>().Any());

            foreach (var character in mainCharacters)
            {
                character.Entity.LevelId = level.Id;
            }

            Time.timeScale = 0;

            await this.LoadGameStateAsync(gameState);

            // TODO: Think about how set position before scene loading.
            var locationPosition = GameObject.FindObjectsOfType<Location>().
                First(o => o.name == locationName).transform.position;

            foreach (var character in this.EntitiesStorage.Value.Entities.Values.Where(o => o.GetComponent<ICharacter>() != null))
            {
                character.transform.position = locationPosition;
            }

            gameState = this.GetState();
            var afterAutoSave = $"Switched to {level.Name} {time}";

            this.VisitedLevels.Add(level.Id);
            await this.SaveAsync(gameState, afterAutoSave);

            Time.timeScale = 1;

            await Task.Delay(1000);

            this.LoadingText.Value.HideText();
            this.Loaded.OnNext(Unit.Default);
            await this.BlackScreen.Value.HideAsync();
        }

        public async Task LoadLastSavedGameAsync()
        {
            var lastSave = this.Storage.Value.Slots.OrderByDescending(o => o.Modified).First();
            await this.LoadSavedGameAsync(lastSave.Name);
        }

        public async Task StartNewGameAsync(params CharacterDescriptor[] characterDescriptors)
        {
            try
            {
                DependencyContainer.Instance.NewScope();

                MessageBroker.Default.Publish(new ResumeGame(string.Empty));

                await this.BlackScreen.Value.ShowAsync();
                this.BackgroundImage.Value.Hide();
                this.LoadingText.Value.DisplayNewText("Level 1: Laboratory \n Stage 1: Initializing system");
                this.ViewRouter.Value.ShowGameHUDView();

                await this.LoadAsync(Level.Laboratory);

                var startLocation = GameObject.FindObjectOfType<StartLocation>().transform.localPosition;

                foreach (var descriptor in characterDescriptors)
                {
                    await this.PlayerService.Value.AddPlayer(descriptor, startLocation);
                }

                MessageBroker.Default.Publish(new GameLoaded());

                await Task.Delay(1000);

                this.LoadingText.Value.HideText();

                await this.SaveAsync($"new game {this.FormDateTimeString()}");

                this.Loaded.OnNext(Unit.Default);
                await this.BlackScreen.Value.HideAsync();

            }
            catch (Exception ex )
            {
                Debug.Log(ex);
            }
        }


        public string FormDateTimeString()
        {
            var dateTimeString =
                DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "-" + DateTime.Now.Second.ToString() + "_" +
                DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString();
            return dateTimeString;
        }

        private async UniTask Bootstrap(GameState gameState)
        {
            this.EntitiesStorage.Value.Root = GameObject.Find("Root");
            if (gameState.VisitedLevels.Contains(this.LevelManager.Value.CurrentLevel.Id))
            {
                GameObject.DestroyImmediate(this.EntitiesStorage.Value.Root);
                this.EntitiesStorage.Value.Root = new GameObject("Root");
            }

            this.BootstrapEntities();
            this.EntitiesStorage.Value.Root.SetActive(false);

            await this.RestoreGameState(gameState);

            GC.Collect();

            this.EntitiesStorage.Value.Root.SetActive(true);

            this.VisitedLevels = gameState.VisitedLevels;
            this.PlayerService.Value.SetState(gameState.PlayerServiceState);
        }

        private GameState GetState()
        {
            Time.timeScale = 0;

            var gameState = new GameState();
            gameState.LevelId = this.LevelManager.Value.CurrentLevel.Id;
            gameState.GameObjectsStates = this.EntitiesStorage.Value.Entities.Values.
                Select(o => new GameObjectState().SetGameObject(o.gameObject)).ToList();

            gameState.VisitedLevels = this.VisitedLevels;
            gameState.PlayerServiceState = this.PlayerService.Value.GetState();

            if (this.lastKnowGameState != null)
            {
                var gameObjectsStatesPerId = gameState.GameObjectsStates.ToDictionary(o => o.Entity.Id);
                var gameObjectStatesFromPreviousLevels = this.lastKnowGameState.GameObjectsStates
                    .Where(o => o.Entity.LevelId != gameState.LevelId && !gameObjectsStatesPerId.ContainsKey(o.Entity.Id));

                gameState.GameObjectsStates.AddRange(gameObjectStatesFromPreviousLevels);
            }

            Time.timeScale = 1;

            this.lastKnowGameState = gameState;

            return gameState;
        }

        private async UniTask RestoreGameState(GameState gameState)
        {
            var cache = new Dictionary<string, GameObject>();
            var monoBehaviours = new LinkedList<IMonoBehaviourWithState>();

            var set = new HashSet<string>(gameState.GameObjectsStates.Select(o => o.Entity.AssetGuid));
            await Task.WhenAll(set
                .Select(o => new AssetReference(o).LoadAssetAsync<GameObject>().Task));
            
            foreach (var gameObjectState in gameState.GameObjectsStates.Where(o => o.Entity.LevelId == this.LevelManager.Value.CurrentLevel.Id))
            {
                var entity = gameObjectState.Entity;
                if (!cache.ContainsKey(entity.AssetGuid))
                {
                    var reference = new AssetReference(entity.AssetGuid);
                    var template = await reference.LoadAssetAsync<GameObject>().Task;
                    cache.Add(entity.AssetGuid, template);
                }

                var gameObject = GameObject.Instantiate<GameObject>(cache[entity.AssetGuid], this.EntitiesStorage.Value.Root.transform);

                var states = gameObjectState.MonoBehaviousStates.ToList();
                states.Add(entity);

                foreach (var monoBehaviour in gameObject.GetComponents<IMonoBehaviourWithState>())
                {
                    var stateType = monoBehaviour.GetStateType();
                    var monoBehaviourState = states.First(o => stateType.IsInstanceOfType(o));
                    monoBehaviour.SetState(monoBehaviourState);
                    monoBehaviours.AddLast(monoBehaviour);
                }

                var prefab = gameObject.GetComponent<Entity>();
                this.EntitiesStorage.Value.Entities.Add(prefab.Id, prefab);
            }

            foreach (var monoBehaviour in monoBehaviours)
            {
                monoBehaviour.Loaded();
            }
        }
    }
}