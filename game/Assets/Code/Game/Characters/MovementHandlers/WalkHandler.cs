using System;
using System.Linq;
using TeamZ.Code.Game.Activation.Core;
using TeamZ.Code.Game.Characters;
using TeamZ.Code.Helpers.Extentions;
using TeamZ.Helpers;
using UniRx;
using UnityEngine;
using static TeamZ.Code.Game.Characters.CharacterControllerScript;

namespace TeamZ.Characters.MovementHandlers
{
    public class WalkHandler : MovementHandler
    {
        private bool IS_LANDED = false;

        public WalkHandler(bool isLanded = false)
        {
            IS_LANDED = isLanded;
        }

        public override void Init(CharacterControllerScript characterController)
        {
            characterController.PlatformCollision.Ignore.Value = false;
            characterController.RigidBody.gravityScale = 0.0f;
            characterController.ResetJump();

            this.EnableJump(characterController);
            this.EnableFallDown(characterController);
            this.EnableMomentSounds(characterController, false);
            this.EnableDirectionDetection(characterController);
            this.EnableClimbingActivation(characterController);
            this.EnableDash(characterController);

            characterController.UserInputProvider.Punch
                .True()
                .ThrottleFirst(TimeSpan.FromSeconds(0.25))
                .Subscribe(o =>
                {
                    characterController.Attacks.Value = FightMode.Punch;
                    characterController.Animator.Punch();
                    characterController.Attack();
                })
                .DisposeWith(this);

            characterController.UserInputProvider.Kick
                .True()
                .ThrottleFirst(TimeSpan.FromSeconds(0.5))
                .Subscribe(o =>
                {
                    characterController.Attacks.Value = FightMode.Kick;
                    characterController.Animator.Kick();
                    characterController.Attack();
                })
                .DisposeWith(this);

            characterController.UserInputProvider.Activate
                .True()
                .Subscribe(activate =>
                {
                    var hits = Physics.RaycastAll(characterController.transform.position - Vector3.forward, Vector3.forward);

                    var firstActivable = hits
                        .Select(o => o.collider.gameObject.GetComponent<IActivable>())
                        .FirstOrDefault(o => o != null);

                    if (firstActivable != null)
                    {
                        MessageBroker.Default.Publish(new ActivationHappened());
                        firstActivable?.Activate();
                    }
                })
                .DisposeWith(this);

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
                        characterController.Animator.Run();
                    }
                    else
                    {
                        if (IS_LANDED)
                        {
                            MessageBroker.Default.Publish(new LandingHappened());
                        }
                        characterController.Animator.Idle(IS_LANDED);
                        IS_LANDED = false;
                    }
                })
                .DisposeWith(this);

            Observable.EveryUpdate().Subscribe(_ =>
            {
                var groudHits = Physics2D.RaycastAll(characterController.GroundCheck.position,
                    -characterController.transform.up, characterController.GroundRadius,
                    characterController.WalkLayerMask);
                var hit = groudHits.FirstOrDefault(o => !o.collider.isTrigger);

                if (!hit)
                {
                    this.Next(new FlyHandler());
                    return;
                }

                var character = characterController.GetComponentInParent<ICharacter>();
                var forward = -Vector2.Perpendicular(hit.normal);
                var velocity = forward * characterController.HorizontalValue.Value * character.RunSpeed;

                characterController.RigidBody.velocity = velocity;
            }
            ).DisposeWith(this);
        }
    }
}