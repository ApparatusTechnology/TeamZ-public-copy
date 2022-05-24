using System.Collections;
using System.Threading.Tasks;
using UniRx;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

namespace TeamZ.Effects
{
	public class BlackScreen : MonoBehaviour
    {
        public float Delay;
        public Image Image;

        public async UniTask ShowAsync()
        {
            await Observable.FromMicroCoroutine(this.ShowInternal);
        }

        public async UniTask HideAsync()
        {
            await Observable.FromMicroCoroutine(this.HideInternal);
        }

        private IEnumerator HideInternal()
        {
			while (this.Image.color.a > 0)
            {
                var color = this.Image.color;

                color.a = Mathf.Clamp01(color.a - Time.unscaledDeltaTime / this.Delay);

				this.Image.color = color;

				yield return null;
            }
        }

        private IEnumerator ShowInternal()
        {
            while (this.Image.color.a < 1)
            {
                var color = this.Image.color;

				color.a = Mathf.Clamp01(color.a + Time.unscaledDeltaTime / this.Delay);

				this.Image.color = color;

				yield return null;
            }
        }
    }
}
