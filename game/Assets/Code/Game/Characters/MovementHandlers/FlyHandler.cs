using System.Linq;
using TeamZ.Code.Game.Characters;
using TeamZ.Helpers;
using UniRx;
using UnityEngine;

namespace TeamZ.Characters.MovementHandlers
{
    public class FlyHandler : MovementHandler
    {
        public override void Init(CharacterControllerScript characterController)
        {
            MessageBroker.Default.Publish(new RunEnded(false));
            MessageBroker.Default.Publish(new RunEnded(true));

            characterController.RigidBody.gravityScale = 2.0f;

            this.EnableDirectionDetection(characterController);
            this.EnableClimbingActivation(characterController);
            this.EnableJump(characterController);

            characterController.Animator.JumpIdle();

            Observable.EveryFixedUpdate().Subscribe(_ =>
            {
                var character = characterController.GetComponentInParent<ICharacter>();
                var horizontal = characterController.HorizontalValue.Value;

                characterController.RigidBody.velocity = new Vector2(horizontal * character.RunSpeed, characterController.RigidBody.velocity.y);

                var walkSurfaces = Physics2D.OverlapCircleAll(characterController.GroundCheck.position, characterController.GroundRadius, characterController.WalkLayerMask);
                var realWalkSurface = walkSurfaces
                    .FirstOrDefault(o =>
                    {
                        var platformToIgnore = 
                            characterController.PlatformCollision.Ignore.Value && 
                            characterController.PlatformCollision.Mask.Contains(o.gameObject.layer);
                        
                        return !o.isTrigger && !platformToIgnore;
                    });

                if (realWalkSurface)
                {
                    this.Next(new WalkHandler(true));
                }
            })
            .DisposeWith(this);
        }
    }
}
