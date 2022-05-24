using System;
using TeamZ.Assets.UI.Terminal;
using TeamZ.Characters.MovementHandlers;
using TeamZ.Code.Helpers;
using TeamZ.Helpers;
using UniRx;
using UnityEngine;

namespace TeamZ.Code.Game.Characters.MovementHandlers
{
    public class DashHandler : MovementHandler
    {
        public static UnityDependency<Terminal> Terminal { get; set; }

        public override async void Init(CharacterControllerScript characterController)
        {
            if (characterController.Character.Stamina > characterController.Character.DashStaminaUsage)
            {
                var sign = characterController.HorizontalDirection.Value == CharacterControllerScript.Direction.Left ? 1 : -1;

                characterController.Animator.Dash();

                characterController.RigidBody.AddForce(new Vector2(sign * characterController.Character.DashImpulse, 0), ForceMode2D.Impulse);
                characterController.Character.Stamina -= characterController.Character.DashStaminaUsage;

                Observable.Timer(TimeSpan.FromSeconds(0.1))
                    .Subscribe(_ => this.Next(new FlyHandler()))
                    .DisposeWith(this);
            }
            else
            {
                this.Next(new WalkHandler(true));
                await Terminal.Value.PrintAndHideAsync("ENERGY IS NOT ENOUGH", 500, 1000, true);
            }
        }
    }
}