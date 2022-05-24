using System;
using System.Linq;
using TeamZ.Assets.UI.Terminal;
using TeamZ.Code.Game.Characters;
using TeamZ.Code.Game.Characters.MovementHandlers;
using TeamZ.Code.Helpers;
using TeamZ.Code.Helpers.Extentions;
using TeamZ.Helpers;
using UniRx;
using UnityEngine;

namespace TeamZ.Characters.MovementHandlers
{
    public static class WalkExtensions
    {
        public static void EnableDirectionDetection(this MovementHandler handler, CharacterControllerScript character)
        {
            character.UserInputProvider.Horizontal
                .Subscribe(value =>
                {
                    if (Math.Abs((float) value) > 0.6f)
                    {
                        if (value > 0)
                        {
                            character.HorizontalDirection.Value = CharacterControllerScript.Direction.Left;
                        }

                        if (value < 0)
                        {
                            character.HorizontalDirection.Value = CharacterControllerScript.Direction.Right;
                        }
                    }
                })
                .DisposeWith(handler);
        }

        public static void ResetJump(this CharacterControllerScript characterController)
        {
            characterController.Character.JumpsLeft = characterController.Character.JumpsAllowed;
        }

        public static void EnableMomentSounds(this MovementHandler handler, CharacterControllerScript character,
            bool climbing)
        {
            var prevHorizontalValue = 0f;
            character.UserInputProvider.Horizontal
                .Subscribe(value =>
                {
                    var magnitude = Mathf.Abs(value);
                    if (prevHorizontalValue == 0 && magnitude > 0)
                    {
                        MessageBroker.Default.Publish(new RunHappened(climbing));
                    }

                    if (prevHorizontalValue > 0 && magnitude == 0)
                    {
                        MessageBroker.Default.Publish(new RunEnded(climbing));
                    }

                    prevHorizontalValue = magnitude;
                })
                .DisposeWith(handler);
        }

        public static void EnableJump(this MovementHandler handler, CharacterControllerScript character)
        {
            if (character.Character.JumpsLeft > 0)
            {
                var downNotPressed = character.UserInputProvider.Jump
                    .Where(_ => character.UserInputProvider.Vertical.Value >= 0);
                
                if (character.Character.JumpsLeft == character.Character.JumpsAllowed)
                {
                    Observable.Timer(TimeSpan.FromSeconds(character.Character.JumpCooldown))
                        .Subscribe(_ => handler.ChangeHandlerOnActivation<JumpHandler>(downNotPressed));
                    return;
                }

                handler.ChangeHandlerOnActivation<JumpHandler>(downNotPressed);
            }
        }
        
        public static void EnableFallDown(this MovementHandler handler, CharacterControllerScript character)
        {
            character.UserInputProvider.Jump
                .True()
                .Where(o => character.UserInputProvider.Vertical.Value < 0)
                .Subscribe(value =>
                {
                    character.PlatformCollision.Ignore.Value = true;
                    handler.Next(new FlyHandler());
                })
                .DisposeWith(handler);
        }

        public static void EnableDash(this MovementHandler handler,
            CharacterControllerScript character)
        {
            character.UserInputProvider.Horizontal
                .Where(o => o == 0 || Mathf.Abs(o) > 0.5)
                .Window(TimeSpan.FromSeconds(0.5))
                .Where(o => o.Any())
                .Where(values =>
                {
                    var lastDirection = CharacterControllerScript.Direction.Empty;
                    var movementSequence = 0f.With(values.Select(oo => oo.Value));
                    
                    foreach (var value in movementSequence.Window(2))
                    {
                        if (value.First.Value == 0)
                        {
                            var currentDirection = CharacterControllerScript.Direction.Empty;
                            if (value.Last.Value < 0)
                            {
                                currentDirection = CharacterControllerScript.Direction.Right;
                            }
                            
                            if (value.Last.Value > 0)
                            {
                                currentDirection = CharacterControllerScript.Direction.Left;
                            }

                            if (currentDirection != CharacterControllerScript.Direction.Empty && lastDirection == currentDirection)
                            {
                                return true;
                            }

                            lastDirection = currentDirection;
                        }
                    }

                    return false;
                })
                .Subscribe(_ => handler.Next(new DashHandler()))
                .DisposeWith(handler);
        }

        public static void ChangeHandlerOnActivation<THandler>(this MovementHandler handler,
            IObservable<bool> observableToListen)
            where THandler : MovementHandler, new()
        {
            observableToListen
                .Where(o => !o)
                .First()
                .Subscribe(_ =>
                {
                    observableToListen
                        .Where(o => o)
                        .BatchFrame()
                        .Subscribe(o => handler.Next(new THandler()))
                        .DisposeWith(handler);
                })
                .DisposeWith(handler);
        }
    }
}