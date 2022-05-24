using System;
using System.Collections;
using System.Threading;
using TeamZ.Code.Game.Messages.GameSaving;
using TMPro;
using UniRx;
using UniRx.Async;
using UnityEngine;

namespace TeamZ.Assets.UI.Terminal
{
    public class Terminal : MonoBehaviour
    {
        public GameObject View;
        public float PrintDelay = 0.0010f;
        public AudioSource AudioSource;

        private TextMeshPro text;
        private float[] data;
        private Guid activeMessageid;

        public IEnumerator Start()
        {
            this.text = this.View.GetComponentInChildren<TextMeshPro>();

            yield return new WaitForSeconds(1);
            this.data = new float[this.AudioSource.clip.samples];
            this.AudioSource.clip.GetData(this.data, 0);

            MessageBroker.Default
                .Receive<GameLoaded>()
                .Subscribe(o => this.activeMessageid = Guid.NewGuid())
                .AddTo(this);
        }

        public async UniTask<Guid> PrintAsync(string message, int delayAfterLine, bool withAudio = false, CancellationToken token = default)
        {
            var currentMessageId = Guid.NewGuid();
            this.activeMessageid = currentMessageId;

            await this.ActiveAsync();

            var lines = message.Split(new[] { "\n" }, System.StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                if (token.IsCancellationRequested || this.activeMessageid != currentMessageId)
                {
                    return currentMessageId;
                }

                this.text.text = string.Empty;

                if (withAudio)
                {
                    this.AudioSource.Play();
                    await Observable.FromMicroCoroutine(() => this.PrintWithSoundMicroCoroutine(line, currentMessageId, token));
                    this.AudioSource.Pause();
                    await UniTask.Delay(delayAfterLine);

                    continue;
                }

                await Observable.FromMicroCoroutine(() => this.PrintMicroCoroutine(line, currentMessageId, token));
                await UniTask.Delay(delayAfterLine);
            }

            return currentMessageId;
        }

        public async UniTask PrintAndHideAsync(string message, int delayAfterLine, int milliseconds, bool withAudio = false, CancellationToken token = default)
        {
            await this.DeactivateAsync();

            if (!this.View.activeSelf)
            {
                var messageId = await this.PrintAsync(message, delayAfterLine, withAudio, token);
                await UniTask.Delay(milliseconds);
                if (this.activeMessageid != messageId)
                {
                    return;
                }

                await this.DeactivateAsync();
            }
        }

        private IEnumerator PrintWithSoundMicroCoroutine(string message, Guid currentMessageId, CancellationToken token)
        {
            for (int i = 0; i < message.Length; i++)
            {
                if (token.IsCancellationRequested || this.activeMessageid != currentMessageId)
                {
                    yield break;
                }

                while (Mathf.Abs(this.data[this.AudioSource.timeSamples]) < 0.05f && this.activeMessageid == currentMessageId)
                {
                    yield return null;
                }

                for (char j = message[i]; i < message.Length && j != ' '; ++i)
                {
                    if (token.IsCancellationRequested || this.activeMessageid != currentMessageId)
                    {
                        yield break;
                    }

                    j = message[i];
                    this.text.text += j;
                }

                if (i >= message.Length)
                {
                    yield break;
                }

                this.text.text += message[i];

                yield return null;
            }
        }

        public async UniTask ActiveAsync()
        {
            this.text.text = string.Empty;
            this.View.SetActive(true);
            await Observable.FromMicroCoroutine(() => this.Move(new Vector3(0, -0, 0)));
        }

        public async UniTask DeactivateAsync()
        {
            if (this.View.activeSelf)
            {
                await Observable.FromMicroCoroutine(() => this.Move(new Vector3(0, -7, 0)));
                this.View.SetActive(false);
            }
        }

        private IEnumerator Move(Vector3 target)
        {
            var initialTime = Time.realtimeSinceStartup;
            var initialPosition = this.View.transform.localPosition;
            while (Time.realtimeSinceStartup - initialTime < 0.25)
            {
                this.View.transform.localPosition = initialPosition + (target - initialPosition) * ((Time.realtimeSinceStartup - initialTime) / 0.25f);
                yield return null;
            }

            this.View.transform.localPosition = target;
        }

        private IEnumerator PrintMicroCoroutine(string message, Guid currentMessageId, CancellationToken token)
        {
            var startTime = Time.realtimeSinceStartup;

            for (int i = 0; i < message.Length; i++)
            {
                if (token.IsCancellationRequested || this.activeMessageid != currentMessageId)
                {
                    yield break;
                }

                this.text.text += message[i];
                while (Time.realtimeSinceStartup - startTime < this.PrintDelay * i)
                {
                    yield return null;
                }
            }
        }
    }
}