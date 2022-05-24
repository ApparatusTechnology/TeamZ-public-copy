using TeamZ.Code.Game.Characters;
using TeamZ.Helpers;
using UniRx;
using UnityEngine;

namespace TeamZ.Characters.MovementHandlers
{
    public static class ClimbingExtensions
    {
        public static void EnableClimbingActivation(this MovementHandler handler, CharacterControllerScript character)
        {
            var ladder = LayerMask.NameToLayer("Ladder");
            var activationTime = Time.time;

            character.UserInputProvider.Vertical
                .Where(o => o > 0 && Time.time - activationTime > 0.3f)
                .Subscribe(o =>
                {
                    var climbCollider = Physics2D.OverlapCircle(character.ClimbCheck.position, character.ClimbRadius,
                        character.SurfaceForClimbing);
                    if (climbCollider)
                    {
                        if (climbCollider.gameObject.layer == ladder)
                        {
                            handler.Next(new ClimbingHandler(climbCollider.bounds));
                        }

                        handler.Next(new ClimbingHandler());
                    }
                })
            .DisposeWith(handler);
        }

        public static void EnableClimbingDeactivation(this MovementHandler handler, CharacterControllerScript character)
        {
            handler.ChangeHandlerOnActivation<FlyHandler>(character.CanWalk);
        }
    }
}