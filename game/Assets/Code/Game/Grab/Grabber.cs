using System;
using System.Collections;
using Code.Helpers.Extensions;
using TeamZ.Assets.Code.Game.Animators;
using TeamZ.Characters.MovementHandlers;
using TeamZ.Code.Game.Characters;
using TeamZ.Code.Game.Characters.Hedgehog;
using TeamZ.Code.Game.Highlighting;
using TeamZ.Code.Helpers.Extentions;
using UniRx;
using UnityEngine;

namespace TeamZ.Code.Game.Grab
{
    public class Grabber : MonoBehaviour
    {
        public Transform Root;
        public GameObject Template;

        private Grabbable target;
        private CharacterControllerScript controller;
        private IDisposable activationMonitoring;
        private IDisposable grabbing;

        private void Start()
        {
            this.controller = this.GetComponentInParent<CharacterControllerScript>();

            this.activationMonitoring?.Dispose();
            this.grabbing?.Dispose();

            this.activationMonitoring = this.controller.UserInputProvider.Activate
                .True()
                .Where(_ => this.target && this.grabbing is null)
                .Subscribe(async o =>
                {
                    var box = this.target;
                    var rigidbody = this.target.GetComponent<Rigidbody2D>();
                    rigidbody.simulated = false;

                    this.controller.MovementHandler.Value.Next(new FreezeHandler());
                    var (take, completed) = this.controller.Animator.Grab(box.transform.position);
                    await take;

                    var placeObjectToRoot = Observable.FromMicroCoroutine(() => this.Grab(box))
                        .Subscribe();

                    await completed;
                    this.controller.MovementHandler.Value.Next(new CarryHandler());

                    // press activate again
                    var deactivateGrab = this.controller.UserInputProvider.Activate
                        .PressAgain()
                        .First()
                        .Subscribe(async _ =>
                        {
                            this.controller.MovementHandler.Value.Next(new FreezeHandler());
                            var releaseTrack = this.controller.Animator.ReleaseBox();
                            await releaseTrack.OnEvent(AnimationEvents.ON_BOX_RELEASED);
                            this.grabbing.Dispose();

                            await releaseTrack.OnCompleted();
                            this.controller.MovementHandler.Value.Next(new WalkHandler());
                        });

                    // press punch
                    var throwObject = this.controller.UserInputProvider.Punch
                        .True()
                        .First()
                        .Subscribe(async _ =>
                        {
                            this.controller.MovementHandler.Value.Next(new FreezeHandler());
                            var track = this.controller.Animator.ThrowBox();
                            await track.OnEvent(AnimationEvents.ON_BOX_THROW);

                            var direction = (int)this.controller.HorizontalDirection.Value;
                            var directionVector = new Vector3(1.5f * direction, 0.5f, 0);
                            
                            rigidbody.AddForce(
                                directionVector * ((Hedgehog)this.controller.Character).ThrowImpulse,
                                ForceMode2D.Impulse);
                            this.grabbing.Dispose();

                            await track.OnCompleted();
                            this.controller.MovementHandler.Value.Next(new WalkHandler());
                        });

                    this.grabbing = Disposable
                        .Create(() =>
                        {
                            rigidbody.simulated = true;
                            box.transform.localRotation = Quaternion.identity;

                            this.grabbing = null;
                            throwObject.Dispose();
                            deactivateGrab.Dispose();
                            placeObjectToRoot.Dispose();
                        }).AddTo(this);
                });
        }

        public void Release()
        {
            this.grabbing.Dispose();
        }

        private IEnumerator Grab(Grabbable grabbable)
        {
            var grabbableTransform = grabbable.transform;
            while (grabbableTransform)
            {
                if (grabbable.LockRotation)
                {
                    grabbableTransform.position = this.Root.position + grabbable.Offset;
                    yield return null;
                }
                else
                {
                    grabbableTransform.SetPositionAndRotation(this.Root.position + grabbable.Offset,
                        this.Root.localRotation);
                    yield return null;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponentInParent<Grabbable>() is Grabbable grabbable &&
                !grabbable.GetComponent<Highlighter>())
            {
                if (this.target)
                {
                    var oldHighlighter = this.target.GetComponent<Highlighter>();
                    oldHighlighter.Destroy();
                }

                this.target = grabbable;
                var newHighlighter = this.target.gameObject.AddComponent<Highlighter>();
                newHighlighter.ShowOnTop = false;
                newHighlighter.Color = grabbable.HighlightingColor;
                newHighlighter.Template = this.Template;
                newHighlighter.Size = new Vector3(1.05f, 1.05f);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.GetComponentInParent<Grabbable>() is Grabbable grabbable)
            {
                if (this.target == grabbable)
                {
                    this.target.GetComponent<Highlighter>().Destroy();
                    this.target = null;
                }
            }
        }
    }
}