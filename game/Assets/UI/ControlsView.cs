using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamZ.Characters;
using TeamZ.Code.DependencyInjection;
using TeamZ.Code.Game.Players;
using TeamZ.Code.Game.UserInput;
using TeamZ.GameSaving;
using TeamZ.UI;
using UniRx;
using UniRx.Async;
using UnityEngine;

namespace TeamZ.Assets.UI
{
    public class ControlsView : View
    {
        public readonly Dependency<UserInputMapper> UserInputMapper;
        public readonly Dependency<GameController> GameController;

        private void Start()
        {
            
        }

        private void OnEnable()
        {
            this.UserInputMapper.Value.EnableInput();
        }

        private void OnDisable()
        {
            this.UserInputMapper.Value.DisableInput();
        }
    }
}
