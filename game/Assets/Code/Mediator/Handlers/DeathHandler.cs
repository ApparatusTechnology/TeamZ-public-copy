using TeamZ.Code.DependencyInjection;
using TeamZ.Code.Game.Characters;
using TeamZ.Code.Helpers;
using TeamZ.Effects;
using TeamZ.GameSaving;
using UnityEngine;

namespace TeamZ.Code.Mediator.Handlers
{
	public class DeathHandler : IHandler<CharacterDead>
	{
		public UnityDependency<BlackScreen> BlackScreen { get; set; }
		public Dependency<GameController> GameController { get; set; }

		public async void Handle(CharacterDead characterDead)
		{
			var effect = this.BlackScreen.Value;
			var delay = effect.Delay;

			Time.timeScale = 0.5f;
			effect.Delay = 2;
			await effect.ShowAsync();
			effect.Delay = delay;
			Time.timeScale = 1;

			await this.GameController.Value.LoadLastSavedGameAsync();
		}
	}
}
