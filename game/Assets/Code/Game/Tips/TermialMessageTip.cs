using System;
using System.Threading;
using TeamZ.Assets.UI.Terminal;
using TeamZ.Code.Game.Tips.Core;
using TeamZ.Code.Helpers;
using UniRx.Async;
using UnityEngine;

namespace TeamZ.Code.Game.Tips
{
    public class TermialMessageTip : Tip
    {
        public bool WithAudio;

        [TextArea(3, 15)]
        public string Message;

        public int DelayAfterLine;

        private UnityDependency<Terminal> Terminal;
        private CancellationTokenSource tokenSource;

        protected async override void OnActivate()
        {
            this.tokenSource?.Cancel();
            var localToken = this.tokenSource = new CancellationTokenSource();

            await this.Terminal.Value.PrintAsync(this.Message, this.DelayAfterLine, this.WithAudio, localToken.Token);
        }

        protected async override void OnDeactivate()
        {
            this.tokenSource?.Cancel();
            await this.Terminal.Value.DeactivateAsync();
        }

        private void OnDestroy()
        {
            this.tokenSource?.Cancel();
        }
    }
}
