using System.Linq;
using TeamZ.Characters;
using TeamZ.Code;
using TeamZ.Code.DependencyInjection;
using TeamZ.Code.Game.Levels;
using TeamZ.Code.Game.Players;
using TeamZ.Code.Helpers;
using TeamZ.UI;
using UniRx;
using UniRx.Async;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TeamZ.GameSaving.MonoBehaviours
{
	public class LevelBootstraper : MonoBehaviour
	{
		public string LevelName;

		private UnityDependency<Main> main;
		private UnityDependency<ViewRouter> router;
		private Dependency<GameController> gameController;
		private Dependency<LevelManager> levelManager;
        private Dependency<PlayerService> playerService;

        private async void Start()
		{
			if (!this.main)
			{
				await SceneManager.LoadSceneAsync("Core", LoadSceneMode.Additive);
				await Observable.NextFrame();
                await Load();
			}
		}

        public async UniTask Load()
        {
	        var level = Level.All[this.LevelName];
	        if (this.levelManager.Value.CurrentLevel == level)
	        {
		        return;
	        }
	        
	        this.levelManager.Value.CurrentLevel = level;
	        this.gameController.Value.VisitedLevels.Add(level.Id);
	        this.gameController.Value.BootstrapEntities(true);

	        var startLocation = GameObject.FindObjectsOfType<Location>().First().transform.localPosition;

	        await this.playerService.Value.AddPlayer(CharactersList.Hedgehog, startLocation);
	        this.gameController.Value.Loaded.OnNext(Unit.Default);

	        this.router.Value.ShowGameHUDView();
        }
	}
}