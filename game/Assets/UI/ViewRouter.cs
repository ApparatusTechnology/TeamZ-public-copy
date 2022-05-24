using TeamZ.Assets.UI;
using TeamZ.Code.Helpers;
using TeamZ.UI.Save;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem.UI;

namespace TeamZ.UI
{
    public class View : MonoBehaviour
    {
    }

    public class ViewRouter : MonoBehaviour
    {
        public GameObject View;

        public HUDController GameHUDView;
        public MainView MainView;
        public LoadView LoadView;
        public SaveView SaveView;
        public SettingsView SettingsView;
        public ControlsView Controls;
        public CharacterSelectionView CharacterSelectionView;

        public UnityDependency<MainMenuBackgroundImage> BackgroundImage;

        public View CurrentView { get; private set; }

        public void ShowGameHUDView()
        {
            this.EnableTips();
            this.ShowView(this.GameHUDView);

            this.BackgroundImage.Value.Deactivate();
        }

        public void ShowMainView()
        {
            this.DisableTips();
            this.ShowView(this.MainView);
            this.BackgroundImage.Value.Activate();
        }

        public void ShowView(View viewTemplate)
        {
            this.DestroyPreviousView();
            Observable.NextFrame().Subscribe(_ =>
            {
                var view = GameObject.Instantiate(viewTemplate);
                view.transform.SetParent(this.View.transform, false);
                this.CurrentView = view;
            });
        }

        private void DestroyPreviousView()
        {
            foreach (Transform view in this.View.transform)
            {
                GameObject.DestroyImmediate(view.gameObject);
            }
        }

        private void DisableTips()
        {
            foreach (Transform view in this.transform)
            {
                if (view.name == "Tips")
                {
                    view.gameObject.SetActive(false);
                }
            }
        }

        private void EnableTips()
        {
            foreach (Transform view in this.transform)
            {
                if (view.name == "Tips")
                {
                    view.gameObject.SetActive(true);
                }
            }
        }
    }
}