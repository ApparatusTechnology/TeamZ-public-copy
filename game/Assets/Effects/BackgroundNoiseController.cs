using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamZ.Code.Helpers;
using UniRx;
using UnityEngine;

namespace TeamZ.Assets.Effects
{
    [Serializable]
    public class EnviromentNoise
    {
        public AudioClip Clip;
        public bool Shake;
        public float ShakeAmplitude;
    }

    [RequireComponent(typeof(AudioSource))]
    public class BackgroundNoiseController : MonoBehaviour
    {
        public EnviromentNoise[] Noises;

        public float MinDelay = 7;
        public float MaxDelay = 17;

        private AudioSource AudioSource;

        private void Start()
        {
            this.AudioSource = this.GetComponent<AudioSource>();
            this.ScheduleNoise();
        }


        public void ScheduleNoise()
        {
            var value = UnityEngine.Random.Range(this.MinDelay, this.MaxDelay);
            Observable.Timer(TimeSpan.FromSeconds(value))
                .Do(_ => this.ScheduleNoise())
                .Subscribe(_ =>
                {
                    var noise = this.Noises.SelectRandom();
                    this.AudioSource.PlayOneShot(noise.Clip);
                    if (noise.Shake)
                    {
                        MessageBroker.Default.Publish(new ShakeMainCamera { Time = noise.Clip.length, Amplitude = noise.ShakeAmplitude });
                    }
                })
                .AddTo(this);
        }
    }
}
