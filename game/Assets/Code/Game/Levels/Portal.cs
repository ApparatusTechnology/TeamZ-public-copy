using System;
using TeamZ.Code.DependencyInjection;
using TeamZ.Code.Game.Activation.Core;
using TeamZ.Code.Inspectors;
using TeamZ.GameSaving;
using TeamZ.Assets.UI.Terminal;
using TeamZ.Code.Helpers;
using UniRx;
using UnityEngine;
using UniRx.Async;
using TeamZ.GameSaving.MonoBehaviours;
using TeamZ.GameSaving.States;
using UnityEngine.SceneManagement;
using UnityEditor;

namespace TeamZ.Code.Game.Levels
{
    [ExecuteInEditMode]
    public class Portal : MonoBehaviourWithState<PortalState>, IActivable
    {
		Dependency<GameController> gameController;

        public string Location;
        public bool IsActivated;

        [SerializeField]
        private string sceneName;

        private UnityDependency<Terminal> Terminal;

        public void Activate()
        {
            if (!this.IsActivated)
            {
                this.Terminal.Value.PrintAndHideAsync("The door is deactivated!", 500, 1000, true).Forget();
                return;
            }

#if UNITY_EDITOR
            if (string.IsNullOrWhiteSpace(this.sceneName))
            {
                throw new InvalidOperationException($"Level with name {this.sceneName} does not exist.");
            }
#endif

            if (!Level.All.ContainsKey(this.sceneName))
            {
                throw new InvalidOperationException($"Level with name {this.sceneName} does not exist.");
            }

			MessageBroker.Default.Publish(new PortalToNextLevelHappened());
			gameController.Value.SwitchLevelAsync(Level.All[this.sceneName], this.Location);
        }

        public override PortalState GetState()
        {
            return new PortalState
            {
                Name = this.name,
                IsActive = this.IsActivated,
                Location = this.Location,
                SceneName = this.sceneName
            };
        }

        public override void SetState(PortalState state)
        {
            this.name = state.Name;
            this.IsActivated = state.IsActive;
            this.Location = state.Location;
            this.sceneName = state.SceneName;
        }
    }

	public class PortalToNextLevelHappened
	{
		public PortalToNextLevelHappened()
		{
		}
	}
}