using System;
using System.Linq;
using TeamZ.Code.Game.Characters;
using TeamZ.Code.Game.Grab;
using TeamZ.Helpers;
using UniRx;
using UnityEngine;
using static TeamZ.Code.Game.Characters.CharacterControllerScript;

namespace TeamZ.Characters.MovementHandlers
{
    public class CarryHandler : MovementHandler
    {
        public CarryHandler()
        {
        }

        public override void Init(CharacterControllerScript characterController)
        {
            var grabber = characterController.GetComponent<Grabber>();

            characterController.PlatformCollision.Ignore.Value = false;
            characterController.RigidBody.gravityScale = 0.0f;
            characterController.ResetJump();
            
            this.EnableDirectionDetection(characterController);
            
            characterController.UserInputProvider.Vertical
                .Subscribe(value =>
                {
                    if (value > 0)
                    {
                        characterController.VerticalDirection.Value = Direction.Up;
                    }

                    if (value < 0)
                    {
                        characterController.VerticalDirection.Value = Direction.Down;
                    }
                })
            .DisposeWith(this);

            characterController.HorizontalValue
                .Subscribe(speed =>
                {
                    if (Math.Abs(speed) > 0.1)
                    {
                        characterController.Animator.WalkWithWeight();
                    }
                    else
                    {
                        characterController.Animator.IdleWithWeight();
                    }
                })
            .DisposeWith(this);
            
            Observable.EveryUpdate().Subscribe(_ =>
            {
                var groudHits = Physics2D.RaycastAll(characterController.GroundCheck.position, -characterController.transform.up, characterController.GroundRadius, characterController.WalkLayerMask);
                var hit = groudHits.FirstOrDefault(o => !o.collider.isTrigger);

                if (!hit)
                {
                    grabber?.Release();
                    this.Next(new FlyHandler());
                    return;
                }

                var character = characterController.GetComponentInParent<ICharacter>();
                var forward = -Vector2.Perpendicular(hit.normal);
                var velocity = forward * characterController.HorizontalValue.Value * character.CreepSpeed;

                characterController.RigidBody.velocity = velocity;

            }).DisposeWith(this);
        }
    }
}
