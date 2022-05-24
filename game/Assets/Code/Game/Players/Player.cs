using System;
using TeamZ.Code.Game.Characters;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TeamZ.Code.Game.Players
{
    public class Player : MonoBehaviour
    {
        private void Awake()
        {
            this.Controller = this.GetComponent<CharacterControllerScript>();
        }

        public CharacterControllerScript Controller { get; private set; }
    }
}
