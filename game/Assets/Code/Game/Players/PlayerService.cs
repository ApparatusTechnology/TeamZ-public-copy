using System;
using System.Linq;
using TeamZ.Characters;
using TeamZ.Code.DependencyInjection;
using TeamZ.Code.Game.Characters;
using TeamZ.Code.Game.Characters.Lizard;
using TeamZ.Code.Game.UserInput;
using TeamZ.Code.Helpers;
using TeamZ.Code.Helpers.Extentions;
using TeamZ.GameSaving;
using TeamZ.GameSaving.Interfaces;
using TeamZ.GameSaving.MonoBehaviours;
using TeamZ.Helpers;
using UniRx;
using UniRx.Async;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TeamZ.Code.Game.Players
{
    public class PlayerServiceState : State
    {
        public virtual Guid? FirstPlayerEntityId { get; set; }

        public virtual Guid? SecondPlayerEntityId { get; set; }
    }


    public class PlayerService : StateProvider<PlayerServiceState>
    {
        UnityDependency<FirstPlayer> FirstPlayer;
        UnityDependency<SecondPlayer> SecondPlayer;
        UnityDependency<MainCamera> Camera;

        Dependency<EntitiesStorage> EntitiesStorage;
        Dependency<UserInputMapper> UserInputMapper;

        public PlayerService()
        {
            this.UserInputMapper.Value.UserInputProviders.ForEach(provider =>
            {
                provider.StartButton
                    .HoldFor(TimeSpan.FromSeconds(1))
                    .Do(_ => { Debug.Log("Adding new player within 2 seconds."); })
                    .HoldFor(TimeSpan.FromSeconds(2))
                    .Subscribe(_ =>
                    {
                        if (this.FirstPlayer && this.SecondPlayer)
                        {
                            Debug.Log("Two players exist.");
                            return;
                        }

                        if (!this.FirstPlayer)
                        {
                            this.HandlePlayerActivation(provider, this.FirstPlayer, this.SecondPlayer).Forget();
                            return;
                        }

                        if (!this.SecondPlayer)
                        {
                            this.HandlePlayerActivation(provider, this.SecondPlayer, this.FirstPlayer).Forget();
                        }
                    })
                    .DisposeWith(this);
            });
        }

        private async UniTask HandlePlayerActivation(UserInputProvider provider, Player activatedPlayer, Player anotherPlayer)
        {
            if (!activatedPlayer)
            {
                var characterController = anotherPlayer.GetComponent<CharacterControllerScript>();
                var newCharacterDescriptor = characterController is LizardController
                    ? CharactersList.Hedgehog
                    : CharactersList.Lizard;

                await this.AddPlayer(provider, newCharacterDescriptor, anotherPlayer.transform.localPosition);
                this.Camera.Value.SearchForPlayers().Forget();
                return;
            }
        }

        private void RemovePlayer(Player player)
        {
            player.gameObject.Destroy();
        }

        public async UniTask AddPlayer(UserInputProvider provider, CharacterDescriptor characterDescriptor, Vector3 localPosition)
        {
            var root = this.EntitiesStorage.Value.Root.transform;
            var characterTemplate = await Addressables.LoadAssetAsync<GameObject>(characterDescriptor.AssetPath).Task;

            var player = characterTemplate.Spawn(localPosition, root);
            var controller = player.GetComponent<CharacterControllerScript>();
            provider.enabled = true;
            controller.UserInputProvider = provider;
            
            if (!this.FirstPlayer)
            {
                player.gameObject.AddComponent<FirstPlayer>();
            }
            else
            {
                player.gameObject.AddComponent<SecondPlayer>();
            }
        }

        public void Bootstrap()
        {
            if (this.FirstPlayer && this.SecondPlayer)
            {
                this.FirstPlayer.Value.Controller.UserInputProvider =
                    this.UserInputMapper.Value.UserInputProviders[0];
                
                this.SecondPlayer.Value.Controller.UserInputProvider =
                    this.UserInputMapper.Value.UserInputProviders[1];
                
                return;
            }
            
            if (this.FirstPlayer)
            {
                this.FirstPlayer.Value.Controller.UserInputProvider =
                    this.UserInputMapper.Value.UserInputProviders[0];
            }
            
            if (this.SecondPlayer)
            {
                this.SecondPlayer.Value.Controller.UserInputProvider =
                    this.UserInputMapper.Value.UserInputProviders[0];
            }
        }

        public override PlayerServiceState GetState() => new PlayerServiceState
        {
            FirstPlayerEntityId = this.FirstPlayer.Value?.GetComponent<Entity>()?.Id,
            SecondPlayerEntityId = this.SecondPlayer.Value?.GetComponent<Entity>()?.Id,
        };

        public override void SetState(PlayerServiceState state)
        {
            if (state.FirstPlayerEntityId.HasValue &&
                this.EntitiesStorage.Value.Entities.TryGetValue(state.FirstPlayerEntityId.Value, out var firstPlayer))
            {
                firstPlayer.gameObject.AddComponent<FirstPlayer>();
            }

            if (state.SecondPlayerEntityId.HasValue &&
                this.EntitiesStorage.Value.Entities.TryGetValue(state.SecondPlayerEntityId.Value, out var secondPlayer))
            {
                secondPlayer.gameObject.AddComponent<SecondPlayer>();
            }
            
            this.Bootstrap();
        }

        public async UniTask AddPlayer(CharacterDescriptor characterDescriptor, Vector3 localPosition)
        {
            await this.AddPlayer(this.UserInputMapper.Value.UserInputProviders.First(), characterDescriptor, localPosition );
        }
    }
}