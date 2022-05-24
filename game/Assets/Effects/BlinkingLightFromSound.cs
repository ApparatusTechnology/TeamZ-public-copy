using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace TeamZ.Effects
{
    public class BlinkingLightFromSound : MonoBehaviour
    {
        public float BlinkPower;

        private new Light2D light;
        private AudioSource audioSource;

        private static Dictionary<int, float[]> audioClipCache
            = new Dictionary<int, float[]>();

        private IEnumerator Start()
        {
            this.light = GetComponentInChildren<Light2D>();
            this.audioSource = GetComponent<AudioSource>();
            yield return new WaitForSeconds(1f);

            var audioClipId = this.audioSource.clip.GetInstanceID();
            if (!audioClipCache.ContainsKey(audioClipId))
            {
                int samples = this.audioSource.clip.samples;
                var data = new float[samples];
                this.audioSource.clip.GetData(data, 0);
                audioClipCache[audioClipId] = data;
            }

            Observable.FromMicroCoroutine(() => this.FlashBySound(this.audioSource))
                .Subscribe()
                .AddTo(this);
        }

        private IEnumerator FlashBySound(AudioSource targetAudioSource)
        {
            var initialIntensity = this.light.intensity;
            var data = audioClipCache[targetAudioSource.clip.GetInstanceID()];
            while (this)
            {
                this.light.intensity = initialIntensity - initialIntensity * data[targetAudioSource.timeSamples] * this.BlinkPower;
                yield return null;
            }
        }
    }
}
