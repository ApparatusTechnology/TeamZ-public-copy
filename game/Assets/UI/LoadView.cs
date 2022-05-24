using System.Collections.Specialized;
using System.Linq;
using TeamZ.Code.DependencyInjection;
using TeamZ.Code.Helpers;
using TeamZ.GameSaving;
using TeamZ.UI.Load;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace TeamZ.UI
{
    public class LoadView : View
    {
        private Dependency<GameStorage> GameStorage;
        private UnityDependency<ViewRouter> Router;

        public GameObject Root;
        public GameObject LoadItemTemplate;

        public Selectable BackButton;

        public async void OnEnable()
        {
            await Observable.NextFrame();

            foreach (Transform loadItem in this.Root.transform)
            {
                loadItem.gameObject.Destroy();
            }

            var slots = this.GameStorage.Value.Slots.OrderByDescending(o => o.Modified).ToArray();
            var firstSlot = this.CreateButton(slots.First());
            var prev = firstSlot;

            foreach (var slot in slots.Skip(1))
            {
                prev = this.CreateButton(slot, prev);
            }

            firstSlot.Select();

            var navigation = this.BackButton.navigation;
            navigation.selectOnUp = firstSlot;
            navigation.selectOnLeft = firstSlot;
            navigation.selectOnRight = firstSlot;
            this.BackButton.navigation = navigation;
        }

        private Selectable CreateButton(GameSlot slot, Selectable prev = null)
        {
            var loadItem = GameObject.Instantiate<GameObject>(this.LoadItemTemplate, this.Root.transform);
            loadItem.GetComponent<LoadItemView>().SlotName = slot.Name;

            var selectable = loadItem.GetComponent<Selectable>();

            var currentNavigation = selectable.navigation;
            currentNavigation.mode = Navigation.Mode.Explicit;
            currentNavigation.selectOnUp = prev;
            currentNavigation.selectOnRight = this.BackButton;
            currentNavigation.selectOnLeft = this.BackButton;
            selectable.navigation = currentNavigation;

            if (prev)
            {
                var prevNavigation = prev.navigation;
                prevNavigation.selectOnDown = selectable;
                prev.navigation = prevNavigation;
            }

            return selectable;
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                this.Cancel();
            }
        }

        public void Cancel()
        {
            this.Router.Value.ShowMainView();
        }
    }
}