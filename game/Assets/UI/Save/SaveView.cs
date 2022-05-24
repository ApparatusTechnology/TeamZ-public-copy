using System.Linq;
using TeamZ.Code.DependencyInjection;
using TeamZ.Code.Helpers;
using TeamZ.GameSaving;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace TeamZ.UI.Save
{
	public class SaveView : View
	{
		public InputField SlotName;
		public Transform ItemsRoot;

		public GameObject SaveItemTemplate;

		private UnityDependency<ViewRouter> ViewRouter;
		private Dependency<GameStorage> Storage;
		private Dependency<GameController> GameController;

		private Subject<SaveItemView> clicks = new Subject<SaveItemView>();

		private void Start()
		{
			this.clicks.Subscribe(o => this.SlotName.text = o.SlotName);
		}

		private async void OnEnable()
		{
			await Observable.NextFrame();
			Selectable.allSelectablesArray.First().Select();

			if (!this.ItemsRoot)
			{
				return;
			}

			foreach (Transform saveItem in this.ItemsRoot)
			{
				saveItem.gameObject.Destroy();
			}

			foreach (var slot in this.Storage.Value.Slots)
			{
				AddSlot(slot.Name);
			}
		}

		private void AddSlot(string slot)
		{
			var loadItem = GameObject.Instantiate<GameObject>(this.SaveItemTemplate, this.ItemsRoot);
			var saveItemView = loadItem.GetComponent<SaveItemView>();
			saveItemView.SlotName = slot;
			saveItemView.Clicks.Subscribe(this.clicks);
		}

		public void Back()
		{
			this.ViewRouter.Value.ShowMainView();
		}

		public async void Save()
		{
			if (string.IsNullOrWhiteSpace(this.SlotName.text))
			{
				return;
			}

			await this.GameController.Value.SaveAsync(this.SlotName.text);
			this.AddSlot(this.SlotName.text);
		}
	}
}