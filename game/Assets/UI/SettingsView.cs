using System.Linq;
using TeamZ.Code.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace TeamZ.UI
{
    public class SettingsView : View
    {
        public Toggle Toggle;
        public GameObject Checkbox;

        private UnityDependency<ViewRouter> Router;

        private void Start()
        {
            Selectable.allSelectablesArray.First().Select();
        }


        public void ToggleSounds(bool value)
        {
            if (!this.Checkbox.activeSelf)
            {
                AudioListener.volume = 0.5f;
            }
            else
            { 
                AudioListener.volume = 0;
            }

            this.Checkbox.SetActive(!this.Checkbox.activeSelf);
        }

        public void Back()
        {
            this.Router.Value.ShowMainView();
        }
    }
}