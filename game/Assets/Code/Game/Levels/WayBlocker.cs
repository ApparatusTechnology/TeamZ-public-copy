using UnityEngine;

namespace TeamZ.Assets.Code.Game.Levels
{
    class WayBlocker : MonoBehaviour
    {
        private BoxCollider2D blocker;
        private AudioSource audioSource;

        private void Awake()
        {
            this.blocker = this.GetComponent<BoxCollider2D>();
            this.audioSource = this.GetComponent<AudioSource>();
        }

        public void Enable()
        {
            this.blocker.enabled = true;
            this.audioSource.Play();
        }

        public void Disable()
        {
            this.blocker.enabled = false;
            this.audioSource.Pause();
        }
    }
}
