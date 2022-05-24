using System;
using System.Collections;
using TeamZ.Assets.UI.Terminal;
using TeamZ.Code.Game.Characters;
using TeamZ.Code.Helpers;
using TeamZ.Helpers;
using UniRx;
using UniRx.Async;
using UnityEngine;

namespace TeamZ.Characters.MovementHandlers
{
    public class DeathHandler : MovementHandler
    {
        public override void Init(CharacterControllerScript characterController)
        {
            MessageBroker.Default.Publish(new RunEnded(false));
            MessageBroker.Default.Publish(new RunEnded(true));

            characterController.RigidBody.gravityScale = 2;
            characterController.RigidBody.velocity = Vector2.zero;

            characterController.Animator.Die();
        }
    }
}
