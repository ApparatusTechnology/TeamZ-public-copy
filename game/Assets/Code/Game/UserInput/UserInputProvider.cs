using System;
using System.Linq;
using TeamZ.Code.DependencyInjection;
using TeamZ.Code.Game.Players;
using TeamZ.Code.Helpers;
using UniRx;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem.Users;
using UnityEngine.Video;

namespace TeamZ.Code.Game.UserInput
{
    public class UserInputProvider : MonoBehaviour
    {
        public ReactiveProperty<float> Horizontal { get; }
            = new ReactiveProperty<float>();

        public ReactiveProperty<float> Vertical { get; }
            = new ReactiveProperty<float>();

        public ReactiveProperty<bool> Jump { get; }
            = new ReactiveProperty<bool>();

        public ReactiveProperty<bool> Kick { get; }
            = new ReactiveProperty<bool>();

        public ReactiveProperty<bool> Punch { get; }
            = new ReactiveProperty<bool>();

        public ReactiveProperty<bool> Activate { get; }
            = new ReactiveProperty<bool>();

        public ReactiveProperty<bool> StartButton { get; }
            = new ReactiveProperty<bool>();

        public ReactiveProperty<bool> Cancel { get; }
            = new ReactiveProperty<bool>();

        public PlayerInput PlayerInput { get; set; }
 
        private void Start()
        {
            this.PlayerInput = this.GetComponent<PlayerInput>();
        }

        public void OnDisable()
        {
            this.Horizontal.Value = 0;
            this.Vertical.Value = 0;
        }

        private void MovePerformed(InputAction.CallbackContext @event)
        {
            if (!this.enabled || GameHelper.IsPaused)
            {
                return;
            }

            var move = @event.ReadValue<Vector2>();
            this.Horizontal.Value = move.x;
            this.Vertical.Value = move.y;
        }

        public void OnMove(InputValue value)
        {
            if (!this.enabled || GameHelper.IsPaused)
            {
                return;
            }

            var vector = value.Get<Vector2>();
            this.Horizontal.Value = vector.x;
            this.Vertical.Value = vector.y;
        }

        public void OnInteract(InputValue value)
        {
            if (!this.enabled || GameHelper.IsPaused)
            {
                return;
            }

            this.Activate.Value = value.Get<float>() > 0;
        }

        public void OnJump(InputValue value)
        {
            if (!this.enabled || GameHelper.IsPaused)
            {
                return;
            }

            this.Jump.Value = value.Get<float>() > 0;
        }

        public void OnKick(InputValue value)
        {
            if (!this.enabled || GameHelper.IsPaused)
            {
                return;
            }

            this.Kick.Value = value.Get<float>() > 0;
        }

        public void OnPunch(InputValue value)
        {
            if (!this.enabled || GameHelper.IsPaused)
            {
                return;
            }

            this.Punch.Value = value.Get<float>() > 0;
        }

        public void OnStart(InputValue value)
        {
            var realValue = value.Get<float>();
            this.StartButton.Value = realValue > 0;
        }
    }
}