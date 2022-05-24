using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;
using UniRx.Async;
using UnityEngine;

namespace TeamZ.Assets.Code.Helpers
{
    public class ShakeableTransform : MonoBehaviour
    {
        public Vector3 position;
        public AnimationCurve AnimationCurve;

        private Transform cachedTransform;
        private IDisposable shaking;
        private IDisposable updating;

        private void Awake()
        {
            this.position = this.transform.position;
            this.cachedTransform = this.transform;

            this.updating?.Dispose();
            this.updating = Observable
                .FromMicroCoroutine(() => this.UpdateCoroutine())
                .Subscribe();
        }

        public void Shake(float time, float amplitude)
        {
            this.updating?.Dispose();
            this.shaking?.Dispose();
            this.shaking = Observable.FromMicroCoroutine(() => this.ShakeCoroutine(time, amplitude)).Subscribe();
        }

        private IEnumerator ShakeCoroutine(float time, float amplitude)
        {
            var currentTime = Time.time;
            while (Time.time - currentTime < time)
            {
                var progress = ((Time.time - currentTime) / time);
                var shift = UnityEngine.Random.insideUnitSphere * amplitude * this.AnimationCurve.Evaluate(progress);

                this.cachedTransform.position = this.position + shift;
                yield return null;
            }

            this.updating?.Dispose();
            this.updating = Observable.FromMicroCoroutine(() => this.UpdateCoroutine()).Subscribe();
        }

        private IEnumerator UpdateCoroutine()
        {
            while (true)
            {
                this.cachedTransform.position = this.position;
                yield return null;
            }
        }
    }
}