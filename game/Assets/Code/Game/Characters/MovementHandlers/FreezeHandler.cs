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
    public class FreezeHandler : MovementHandler
    {
        private UnityDependency<Terminal> Terminal;

        public override void Init(CharacterControllerScript characterController)
        {
            MessageBroker.Default.Publish(new RunEnded(false));
            MessageBroker.Default.Publish(new RunEnded(true));

            characterController.RigidBody.gravityScale = 2;
            characterController.RigidBody.velocity = Vector2.zero;
        }
    }

    public class SkipAction
    {
        public string Name { get; }

        public SkipAction(string name = null)
        {
            this.Name = name;
        }
    }
}