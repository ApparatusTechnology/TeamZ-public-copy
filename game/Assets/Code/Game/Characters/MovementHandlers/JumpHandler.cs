using TeamZ.Assets.UI.Terminal;
using TeamZ.Code.Game.Characters;
using TeamZ.Code.Helpers;
using UniRx;
using UnityEngine;

namespace TeamZ.Characters.MovementHandlers
{
    public class JumpHandler : MovementHandler
    {
        public static UnityDependency<Terminal> Terminal { get; set; }

        public override async void Init(CharacterControllerScript characterController)
        {
            var character = characterController.GetComponentInParent<ICharacter>();

            if (character.Stamina > character.JumpStaminaUsage)
            {
                var (prepare, completed) = characterController.Animator.JumpStart();
                await prepare;

                characterController.RigidBody.velocity = Vector2.zero;
                character.Stamina = Mathf.Max(character.Stamina - character.JumpStaminaUsage, 0);
                characterController.RigidBody.AddForce(new Vector2(0.0f, character.JumpForce));
                characterController.Character.JumpsLeft--;

                MessageBroker.Default.Publish(new JumpHappened());

                await completed;
                this.Next(new FlyHandler());
            }
            else
            {
                this.Next(new WalkHandler(true));
                await Terminal.Value.PrintAndHideAsync("ENERGY IS NOT ENOUGH", 500, 1000, true);
            }
        }
    }
}
