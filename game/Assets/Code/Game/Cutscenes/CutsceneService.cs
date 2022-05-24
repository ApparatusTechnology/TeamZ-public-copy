using System;
using System.Linq;
using System.Threading;
using TeamZ.Characters.MovementHandlers;
using TeamZ.Code.Game.Players;
using TeamZ.Code.Helpers;
using TeamZ.Code.Helpers.Extentions;
using TeamZ.Effects;
using UniRx;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

namespace TeamZ.Code.Game.Cutscenes
{
    public class CutsceneService : MonoBehaviour
    {
        public Image View;
        public UnityDependency<BlackScreen> BlackScreen;
        public bool IsCutsceneActive = false;

        public async UniTask ShowCutscene(CutsceneActivator activator)
        {
            if (this.IsCutsceneActive)
            {
                return;
            }

            var players = GameObject.FindObjectsOfType<Player>();
            var handlers = players
                .Select(o => o.Controller.MovementHandler.Value)
                .ToArray();

            foreach (var player in players)
            {
                player.Controller.MovementHandler.Value.Next(new CutsceneHandler());
            }

            var tokenSource = new CancellationTokenSource();
            var skipActionSubscription = MessageBroker.Default
                .Receive<SkipAction>()
                .Subscribe(o => tokenSource.Cancel());

            this.IsCutsceneActive = true;

            await this.BlackScreen.Value.ShowAsync();
            MessageBroker.Default.Publish(new PauseGame());
            this.View.gameObject.SetActive(true);

            var config = await activator.Config.LoadAssetAsync<CutsceneConfig>().Task;

            foreach (var page in config.Pages)
            {
                this.View.sprite = page.Sprite;

                await this.BlackScreen.Value.HideAsync();
                await UniTaskFixed.Delay(TimeSpan.FromSeconds(page.Duration), tokenSource.Token);
                
                await this.BlackScreen.Value.ShowAsync();

                if (tokenSource.Token.IsCancellationRequested)
                {
                    Debug.Log("break");
                    break;
                }
            }

            if (activator.NeedDestroyAfterShow)
            {
                activator.gameObject.Destroy();
            }

            this.View.gameObject.SetActive(false);
            MessageBroker.Default.Publish(new ResumeGame());
            await this.BlackScreen.Value.HideAsync();

            skipActionSubscription.Dispose();
            for (int i = 0; i < players.Length; i++)
            {
                players[i].Controller.MovementHandler.Value.Next(handlers[i]);
            }

            this.IsCutsceneActive = false;
        }
    }
}