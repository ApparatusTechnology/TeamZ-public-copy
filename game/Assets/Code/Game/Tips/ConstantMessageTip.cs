using System;
using TeamZ.Code.Game.Notifications;
using TeamZ.Code.Game.Tips.Core;
using TeamZ.Code.Helpers;

namespace TeamZ.Code.Game.Tips
{
    public class ConstantMessageTip : Tip
    {
        public string Message;
        
        private UnityDependency<NotificationService> Notifications;
        private IDisposable message;

        protected override void OnActivate()
        {
            this.message = this.Notifications.Value.ShowMessage(this.Message, true);
        }

        protected override void OnDeactivate()
        {
            this.message?.Dispose();
        }

        private void OnDestroy()
        {
            this.message?.Dispose();
        }
    }
}
