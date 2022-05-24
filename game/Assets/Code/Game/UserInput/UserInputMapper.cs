using System;
using System.Collections.Generic;
using System.Linq;
using TeamZ.Code.Game.Players;
using TeamZ.Code.Helpers;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.Utilities;

namespace TeamZ.Code.Game.UserInput
{
    public class UserInputMapper : IDisposable
    {
        public List<UserInputProvider> UserInputProviders { get; }

        public UserInputMapper()
        {
            var devices = Gamepad.all.OfType<InputDevice>().ToList();
            devices.Add(Keyboard.current);

            var userInputs = new GameObject("~UserInputProviders");
            GameObject.DontDestroyOnLoad(userInputs);

            this.UserInputProviders = new List<UserInputProvider>();
            foreach (var device in devices.Where(o => o != null))
            {
                var userInputProviderGameObject = new GameObject(device.displayName);
                userInputProviderGameObject.transform.SetParent(userInputs.transform);

                var playerInput = userInputProviderGameObject.AddComponent<PlayerInput>();
                playerInput.neverAutoSwitchControlSchemes = true;
                playerInput.actions = new TeamZInput().asset;
                playerInput.actions.Enable();

                var userInputProvider = userInputProviderGameObject.AddComponent<UserInputProvider>();

                this.Pair(playerInput.user, device, playerInput.actions.controlSchemes);

                this.UserInputProviders.Add(userInputProvider);
            }
        }

        private void Pair(InputUser user, InputDevice device, ReadOnlyArray<InputControlScheme> schemas,
            InputUserPairingOptions options = InputUserPairingOptions.UnpairCurrentDevicesFromUser)
        {
            InputUser.PerformPairingWithDevice(device, user, options);
        }

        public IEnumerable<UserInputProvider> GetPairedProviders()
        {
            return GameObject.FindObjectsOfType<Player>().Select(o => o.Controller.UserInputProvider);
        }
        
        public void Dispose()
        {
        }

        public void EnableInput()
        {
            foreach (var provider in this.UserInputProviders)
            {
                provider.enabled = true;
            }
        }

        public void DisableInput()
        {
            foreach (var provider in this.UserInputProviders)
            {
                provider.enabled = false;
            }
        }
    }
}