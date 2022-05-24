using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TeamZ.Code.Game.Characters;
using TeamZ.Helpers;
using UniRx;
using UnityEngine;
using UnityEngine.XR.WSA;

namespace TeamZ.Characters.MovementHandlers
{
    public class ClimbingHandler : MovementHandler
    {
        private Bounds bounds;

        public ClimbingHandler()
        {

        }

        public ClimbingHandler(Bounds bounds)
        {
            this.bounds = bounds;
        }

        public override void Init(CharacterControllerScript characterController)
        {
            MessageBroker.Default.Publish(new RunEnded(true));
            MessageBroker.Default.Publish(new RunHappened(true));

            characterController.RigidBody.gravityScale = 0.0f;

            if (this.bounds != default)
            {
                var transform = characterController.transform;
                var position = transform.position;
                position.x = this.bounds.center.x;
                transform.position = position;

                Observable.FromMicroCoroutine(() => this.MoveTo(characterController.gameObject, position, 0.5f));
            }

            characterController.ResetJump();
            this.EnableMomentSounds(characterController, true);
            this.EnableDirectionDetection(characterController);
            this.EnableClimbingDeactivation(characterController);
            this.EnableJump(characterController);

            var prevHorizontalValue = 0f;

            Observable
                  .EveryUpdate()
                  .Subscribe(_ =>
                  {
                      var climbing = Physics2D.OverlapCircle(characterController.ClimbCheck.position, characterController.ClimbRadius, characterController.SurfaceForClimbing);
                      
                      if (!climbing)
                      {
                          this.Next(new FlyHandler());
                          return;
                      }

                      var character = characterController.GetComponentInParent<ICharacter>();
                      var possibleMovement = this.CheckClimbingSurfaceBorders(characterController);
                      var horizontalValue = characterController.HorizontalValue.Value * character.CreepSpeed;
                      var verticalValue = characterController.VerticalValue.Value * character.CreepSpeed;

                      if (possibleMovement.right && horizontalValue > 0)
                      {
                          horizontalValue = 0;
                      }

                      if (possibleMovement.left && horizontalValue < 0)
                      {
                          horizontalValue = 0;
                      }

                      if (possibleMovement.top && verticalValue > 0)
                      {
                          verticalValue = 0;
                      }

                      if (possibleMovement.bottom && verticalValue < 0)
                      {
                          verticalValue = 0;
                      }

                      characterController.Animator.ClimbingSpeed(Mathf.Max(Mathf.Abs(horizontalValue), Mathf.Abs(verticalValue)));

                      var movement = new Vector2(horizontalValue, verticalValue);
                      characterController.RigidBody.velocity = movement;

                      var sqrMagnitude = movement.sqrMagnitude;
                      if (prevHorizontalValue == 0 && sqrMagnitude > 0)
                      {
                          MessageBroker.Default.Publish(new RunHappened(true));
                      }

                      if (prevHorizontalValue > 0 && sqrMagnitude == 0)
                      {
                          MessageBroker.Default.Publish(new RunEnded(true));
                      }

                      prevHorizontalValue = sqrMagnitude;
                  })
                  .DisposeWith(this);
        }

        private IEnumerator MoveTo(GameObject gameObject, Vector3 destination, float time)
        {
            var transform = gameObject.transform;
            var initialTime = Time.time;
            var initialPosition = transform.position;
            var postionDelta = destination - initialPosition;
            var timeToFinish = initialTime + time;

            do
            {
                gameObject.transform.position = initialPosition + postionDelta * ((initialTime - Time.time) / time);
                yield return null;
            }
            while (Vector3.Distance(transform.position, destination) < 0.2 && Time.time < timeToFinish);

            gameObject.transform.position = destination;
        }

        protected (bool top, bool right, bool bottom, bool left) CheckClimbingSurfaceBorders(CharacterControllerScript character)
        {
            var transform = character.transform;
            var localScale = transform.localScale;
            var characterSizeX = Math.Abs(localScale.x);
            var characterSizeY = Math.Abs(localScale.y);

            var hitLeft = Physics2D.Raycast(transform.position - Vector3.forward * 2 - new Vector3(characterSizeX / 2, 0, 0), Vector3.forward, 6.0f, character.SurfaceForClimbing);
            var hitRight = Physics2D.Raycast(transform.position - Vector3.forward * 2 + new Vector3(characterSizeX / 2, 0, 0), Vector3.forward, 6.0f, character.SurfaceForClimbing);
            var hitTop = Physics2D.Raycast(transform.position - Vector3.forward * 2 + new Vector3(0, characterSizeY / 0.8f, 0), Vector3.forward, 6.0f, character.SurfaceForClimbing);
            var hitBottom = Physics2D.Raycast(transform.position - Vector3.forward * 2 - new Vector3(0, characterSizeY / 1.5f, 0), Vector3.forward, 6.0f, character.SurfaceForClimbing);

            var climbingSurfaceOnLeftIsMissing = hitLeft.collider == null;
            var climbingSurfaceOnRightIsMissing = hitRight.collider == null;
            var climbingSurfaceOnTopIsMissing = hitTop.collider == null;
            var climbingSurfaceOnBottomIsMissing = hitBottom.collider == null;

            return (
                climbingSurfaceOnTopIsMissing,
                climbingSurfaceOnRightIsMissing,
                climbingSurfaceOnBottomIsMissing,
                climbingSurfaceOnLeftIsMissing);
        }
    }
}
