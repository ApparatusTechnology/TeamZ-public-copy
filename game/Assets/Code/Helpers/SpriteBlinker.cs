using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace TeamZ.Assets.Code.Helpers
{
    public class SpriteBlinker : MonoBehaviour
    {
        public AnimationCurve AnimationCurve;

        private SpriteRenderer spriteRender;
        private int tintColor;
        private Color whiteColor;
        private IDisposable blinking;

        private void Awake()
        {
            this.spriteRender = this.GetComponent<SpriteRenderer>();
            this.tintColor = Shader.PropertyToID("_Tint");
            this.whiteColor = Color.white * 5;
        }

        public void Blink(float time, float speed)
        {
            this.blinking?.Dispose();
            this.blinking = Observable.FromMicroCoroutine(() => this.BlinkMicroCocoutine(time, speed)).Subscribe();
        }

        private IEnumerator BlinkMicroCocoutine(float time, float speed)
        {
            var currentTime = Time.time;
            var initialColor = this.spriteRender.material.GetColor(this.tintColor);
            while ((Time.time - currentTime) < time)
            {
                var progress = (Time.time - currentTime) / time;
                this.spriteRender.material.SetColor(this.tintColor, this.whiteColor * this.AnimationCurve.Evaluate(progress));
                yield return null;
            }

            this.spriteRender.material.SetColor(this.tintColor, initialColor);
        }
    }
}
