using System;
using TeamZ.Assets.UI.Terminal;
using TeamZ.Code.Game.Characters;
using TeamZ.Code.Helpers;
using TeamZ.Code.Helpers.Extentions;
using TeamZ.Helpers;
using UniRx;
using UniRx.Async;
using UnityEngine;

namespace TeamZ.Characters.MovementHandlers
{
    public class CutsceneHandler : MovementHandler
    {
        private UnityDependency<Terminal> Terminal;

        public override void Init(CharacterControllerScript characterController)
        {
            characterController.RigidBody.gravityScale = 2;
            characterController.RigidBody.velocity = Vector2.zero;

            var time = Time.time;
            characterController.UserInputProvider.Horizontal
                .Where(_ => Time.time - time >= 0.5)
                .Subscribe(value =>
                {
                    if (value != 0)
                    {
                        this.Terminal.Value.PrintAndHideAsync($"Wait a minute... Or press JUMP for skip.", 100, 1000, true).Forget();
                    }
                })
                .DisposeWith(this);

            characterController.UserInputProvider.Jump
                .HoldFor(TimeSpan.FromSeconds(0.5f))
                .Subscribe(value =>
                {
                    MessageBroker.Default.Publish(new SkipAction());
                    
                    if (value)
                    {
                        this.Terminal.Value.PrintAndHideAsync($"Cutscene was skipped...", 100, 1000, true).Forget();
                    }
                })
                .DisposeWith(this);
        }
    }
}