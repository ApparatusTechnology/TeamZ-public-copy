using UnityEngine;

namespace TeamZ.Code.Game.Tips.Core
{
    public abstract class Tip : MonoBehaviour
    {
        public bool ActivateOnce = false;

        public void Activate()
        {
            this.OnActivate();
        }

        public void Deactivate()
        {
            this.OnDeactivate();

            if (this.ActivateOnce)
            {
                this.gameObject.Destroy();
            }
        }

        protected abstract void OnActivate();
        protected abstract void OnDeactivate();
    }
}
